using Avalonia.Controls;
using Window = Avalonia.Controls.Window;
using Avalonia.Threading;
using BazisAvaloniaGUI.Reflect;
using BazisGUI.Scene.Core;
using BazisGUI.Scene.Core.Rendering;
using BazisGUI.Scene.VBO;
using Geometry;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;
using Color = System.Drawing.Color;

namespace BazisAvaloniaGUI.Shell
{
    // Из GUI/Methods/Results/Reflect.cs, GUI/Methods/Scene/DisplayReflectionPlane.cs и DisplayBoundingBox.cs.
    // Буферы сцены доступны только в кадре отрисовки, поэтому операции с ними идут через scene.Surface.Invoke.
    internal partial class MainWindow
    {
        private async void отзеркаливаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as MenuItem;
                // CheckOnClick: MenuItem с ToggleType = CheckBox переключает IsChecked до события Click.
                if (btn.IsChecked)
                {
                    var reflect = new ReflectControl();
                    reflect.SetGlObjs(await scene.Surface.InvokeAsync(sceneController =>
                        sceneController.VboController.GetVBObjs().Select(x => x.ObjName).ToList()));

                    var reflectForm = new Window()
                    {
                        Name = "reflectForm",
                        Topmost = true,
                        Icon = Icon,
                        CanResize = false,
                        CanMaximize = false,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        Title = Resources.Reflect_Form_Text,
                        ShowInTaskbar = false,
                        FontFamily = FontFamily,
                        FontSize = FontSize,
                        Content = reflect
                    };

                    reflect.ShowObjs += (ar) =>
                    {
                        SetBackColorToAllObjects();

                        HideGeometryObj("DisplayBoundingBox");
                        DisplayBoundingBox(ar);

                        RequestRedraw();
                    };

                    reflect.CreateReflectObj += async (ar1, ar2) =>
                    {
                        var coef = ar2.ToArray();
                        var copyObjs = await scene.Surface.InvokeAsync(sceneController =>
                        {
                            var vboController = sceneController.VboController;
                            var copies = vboController.GetVBObjs().Count(x => x.ObjName.Contains($"{ar1}_copy"));
                            CreateReflectedVBObject(vboController, ar1, $"{ar1}_copy_{copies + 1}", coef);
                            return vboController.GetVBObjs().Where(x => x.ObjName.Contains($"{ar1}_copy")).Select(x => x.ObjName).ToList();
                        });

                        HideGeometryObj("DisplayReflectionPlane");
                        HideGeometryObj("DisplayBoundingBox");

                        reflect.SetGlObjs(copyObjs);
                        RequestRedraw();
                    };

                    reflect.UpdateReflectPlane += (s, p) =>
                    {
                        HideGeometryObj("DisplayReflectionPlane");
                        DisplayReflectionPlane(s, p);
                        RequestRedraw();
                    };

                    reflectForm.Closed += (o, ev) =>
                    {
                        btn.IsChecked = false;
                        HideGeometryObj("DisplayReflectionPlane");
                        HideGeometryObj("DisplayBoundingBox");

                        VBOController.DeleteAllVBObjects();
                        CreateVBObjects("Объекты");
                        RequestRedraw();
                    };
                    reflectForm.Show(this);
                    reflectForm.Position = Position;
                }
                else
                {
                    OwnedWindows.FirstOrDefault(x => x.Name == "reflectForm")?.Close();
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        /// <summary>Рамка плоскости отражения (BaseForm.DisplayReflectionPlane).</summary>
        public void DisplayReflectionPlane(string objName, float[] coeff)
        {
            var plane = coeff.ToArray();
            scene.Surface.Invoke(sceneController => sceneController.DisplayReflectionPlane(objName, plane));
        }

        /// <summary>Габаритная рамка объекта сцены (BaseForm.DisplayBoundingBox).</summary>
        private void DisplayBoundingBox(string objName)
        {
            scene.Surface.Invoke(sceneController =>
            {
                var vbo = sceneController.VboController.FindVBObj(objName);
                if (vbo != null)
                    sceneController.DisplayGeometryObject("DisplayBoundingBox", context => DrawBoundingBox(context, vbo));
            });
        }

        private static void DrawBoundingBox(IRenderContext context, VBObject vbo)
        {
            var position = context.Camera.Position;
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.Translate(-position._x, -position._y, -position._z);
            GL.MultMatrix(vbo.ModelMatrix);
            GL.Color3(0, 1f, 0);

            foreach (var item in vbo.BoundingBox.GetSidesPoints())
            {
                GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex3(item[0]._x, item[0]._y, item[0]._z);
                GL.Vertex3(item[1]._x, item[1]._y, item[1]._z);
                GL.Vertex3(item[2]._x, item[2]._y, item[2]._z);
                GL.Vertex3(item[3]._x, item[3]._y, item[3]._z);
                GL.End();
            }

            GL.PopMatrix();
        }

        /// <summary>
        /// Создает зеркальную (относительно плоскости) копию вбо-объекта, если задано имя оригинала и копии и коэффициенты плоскости.
        /// Вызывается в кадре сцены: матрица копии считается через стек матриц OpenGL, как в BaseForm.
        /// </summary>
        private void CreateReflectedVBObject(VBOController vboController, string srcVboName, string copyVboName, float[] coef)
        {
            try
            {
                if (vboController.Contains(copyVboName))
                    throw new Exception($"{Resources.Reflect_CreateReflectedVBObject_Exception_Part1} {copyVboName} {Resources.Reflect_CreateReflectedVBObject_Exception_Part2}");

                var srcVbo = vboController.FindVBObj(srcVboName);

                if (srcVbo == null)
                    throw new Exception($"{Resources.Reflect_CreateReflectedVBObject_Exception_Part1} {srcVboName} {Resources.Reflect_CreateReflectedVBObject_Exception_Part3}");

                var copyVbo = vboController.CopyVBObjects(srcVbo, copyVboName);
                vboController.AddVbo(copyVbo);

                // ищем координаты вектора нормали в главной СК
                var ncoef = TransformVector(coef, srcVbo.ModelMatrix);

                var normal = new Point3D(ncoef[0], ncoef[1], ncoef[2]);
                normal = Geometry.Vector.GetVectorNorm(normal);
                var plane = new Geometry.Plane(normal, coef[3]);

                var reflMatrix = GetReflectionMatrix(plane);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();

                GL.LoadMatrix(srcVbo.ModelMatrix);
                GL.MultMatrix(reflMatrix);
                GL.GetFloat(GetPName.ModelviewMatrix, copyVbo.ModelMatrix);
                GL.PopMatrix();
            }
            catch (Exception ex)
            {
                Dispatcher.UIThread.Post(() => console.PrintInfo(ex.Message, Color.Red));
            }
        }

        /// <summary>
        /// Метод переводит текущий вектор нормали в систему координат той модели на которую переключаемся
        /// </summary>
        private static Vector<float> TransformVector(float[] vector, float[] modelMatrix)
        {
            var mat = Matrix<float>.Build.Dense(4, 4, modelMatrix);
            mat = mat.Inverse();
            var vec = Vector<float>.Build.Dense(vector);
            vec = vec.Normalize(2);
            vec = mat.Multiply(vec);

            vec[0] = vec[0].Round(2);
            vec[1] = vec[1].Round(2);
            vec[2] = vec[2].Round(2);
            vec[3] = vec[3].Round(2);
            return vec;
        }

        public float[] GetReflectionMatrix(Geometry.Plane plane)
        {
            var reflection = new float[16];
            var x = -plane.Normal._x;
            var y = -plane.Normal._y;
            var z = -plane.Normal._z;
            var d = plane.Shifting;
            reflection[0] = 1 - 2 * x * x;
            reflection[1] = -2 * x * y;
            reflection[2] = -2 * x * z;
            reflection[3] = 0.0f;
            reflection[4] = -2 * x * y;
            reflection[5] = 1 - 2 * y * y;
            reflection[6] = -2 * y * z;
            reflection[7] = 0.0f;
            reflection[8] = -2 * x * z;
            reflection[9] = -2 * y * z;
            reflection[10] = 1 - 2 * z * z;
            reflection[11] = 0.0f;
            reflection[12] = -2 * x * d;
            reflection[13] = -2 * y * d;
            reflection[14] = -2 * z * d;
            reflection[15] = 1.0f;
            return reflection;
        }
    }
}
