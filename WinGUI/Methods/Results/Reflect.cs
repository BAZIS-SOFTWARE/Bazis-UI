using WinGUI.Properties;
using WinGUI.Reflect;
using WinGUI.Scene.VBO;
using Geometry;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void отзеркаливаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as ToolStripMenuItem;
                if (btn.Checked)
                {
                    var reflect = new ReflectControl();
                    reflect.SetGlObjs(VBOController.GetVBObjs().Select(x => x.ObjName));

                    var reflectForm = new Form()
                    {
                        TopMost = true,
                        ShowIcon = true,
                        Icon = this.Icon,
                        Owner = Application.OpenForms[0],
                        MaximizeBox = false,
                        FormBorderStyle = FormBorderStyle.FixedSingle,
                        Text = Resources.Reflect_Form_Text
                    };
                    reflectForm.Controls.Add(reflect);
                    reflect.Dock = DockStyle.Fill;

                    reflect.ShowObjs += (ar) =>
                    {
                        SetBackColorToAllObjects();

                        var vbo = VBOController.FindVBObj(ar);

                        HideGeometryObj("DisplayBoundingBox");
                        DisplayBoundingBox(vbo);

                        RequestRedraw();
                    };

                    reflect.CreateReflectObj += (ar1, ar2) =>
                    {
                        var copyObjs = VBOController.GetVBObjs().Where(x => x.ObjName.Contains($"{ar1}_copy")).
                        Select(x => x.ObjName);
                        CreateReflectedVBObject(ar1, $"{ar1}_copy_{copyObjs.Count() + 1}", ar2);

                        HideGeometryObj("DisplayReflectionPlane");
                        HideGeometryObj("DisplayBoundingBox");

                        reflect.SetGlObjs(copyObjs);
                        RequestRedraw();
                    };

                    reflect.MatrixEvent += (s, ev) =>
                    {
                        var obj = VBOController.FindVBObj(s);
                        ev.Matrix = obj.ModelMatrix;
                    };

                    reflect.UpdateReflectPlane += (s, p) =>
                    {
                        HideGeometryObj("DisplayReflectionPlane");
                        DisplayReflectionPlane(p);
                        RequestRedraw();
                    };

                    reflectForm.FormClosing += (o, ev) =>
                    {
                        btn.Checked = false;
                        HideGeometryObj("DisplayReflectionPlane");
                        HideGeometryObj("DisplayBoundingBox");

                        // TODO
                        // определить: необходимо показывать все объекты или только нескрытые
                        // скорректировать решение на основе выводов выше
                        VBOController.DeleteAllVBObjects();
                        CreateVBObjects("Объекты");
                        RequestRedraw();
                    };
                    reflectForm.Show();

                    var location = PointToScreen(Point.Empty);
                    reflectForm.Location = location;


                }
                else
                {
                    var forms = Application.OpenForms.Cast<Form>().ToList();
                    var form = forms.Find(x => x.Name == "reflectForm");
                    if (form != null)
                    {
                        form.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        /// <summary>
        /// Создает зеркальную (относительно плоскости) копию вбо-объекта, если задано имя оригинала и копии и коэффициенты плоскости
        /// </summary>
        /// <param name="srcVboName">[In]Имя объекта источника или пустая строка как триггер отмены эвента рисования плоскости</param>
        /// <param name="copyVboName">[In]Имя объекта копии</param>
        /// <param name="coef">[In]Коэффициенты плоскости</param>
        public void CreateReflectedVBObject(string srcVboName, string copyVboName, float[] coef)
        {
            try
            {
                if (VBOController.Contains(copyVboName))
                    throw new Exception($"{Resources.Reflect_CreateReflectedVBObject_Exception_Part1} {copyVboName} {Resources.Reflect_CreateReflectedVBObject_Exception_Part2}");

                var srcVbo = VBOController.FindVBObj(srcVboName);

                if (srcVbo == null)
                    throw new Exception($"{Resources.Reflect_CreateReflectedVBObject_Exception_Part1} {srcVbo} {Resources.Reflect_CreateReflectedVBObject_Exception_Part3}");

                //var reflMatrix = GetReflectionMatrix(plane);//from stack

                var copyVbo = VBOController.CopyVBObjects(srcVbo, copyVboName);
                VBOController.AddVbo(copyVbo);

                // ищем координаты вектора нормали в главной СК
                var ncoef = TransformVector(coef,srcVbo.ModelMatrix);

                var normal = new Point3D(ncoef[0], ncoef[1], ncoef[2]);
                normal = Geometry.Vector.GetVectorNorm(normal);
                var plane = new Geometry.Plane(normal, coef[3]);

                var reflMatrix = GetReflectionMatrix(plane);//from stack
                                                            //DisplayReflectionPlane(src, plane);
                GL.MatrixMode(MatrixMode.Modelview);//переключение на видовая и модельная матрица
                GL.PushMatrix();
   
                GL.LoadMatrix(srcVbo.ModelMatrix);
                GL.MultMatrix(reflMatrix);
                GL.GetFloat(GetPName.ModelviewMatrix, copyVbo.ModelMatrix);
                GL.PopMatrix();

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
            
        }

        /// <summary>
        /// Метод переводит текущий вектор нормали в систему координат той модели на которую переключаемся
        /// </summary>
        /// <param name="vector">Текущий вектор нормали</param>
        /// <param name="modelMatrix">Текущая модельная матрица объекта</param>
        /// <returns>Вектор в системе координат выделенного объекта</returns>
        private Vector<float> TransformVector(float[] vector, float[] modelMatrix)
        {
            //var evnt = new MatrixEvent();
            //var name = comboBox1.SelectedItem.ToString();
            //MatrixEvent?.Invoke(name, evnt);
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

        //private void ReflectVbo(Geometry.Plane plane, VBObject copyVbo)
        //{
        //    float[,] reflection;
        //    float[] t;
        //    GetReflectionData(plane, out reflection, out t);
        //    //reflection[15] = 1.0f;

        //    var copyNorms = copyVbo.NormalsCoords;
        //    var copyCoords = copyVbo.PointsCoords;

        //    var length = copyVbo.CoordLength / 3;

        //    var rM = Matrix<float>.Build.DenseOfArray(reflection);
        //    var tV = Vector<float>.Build.Dense(t);
        //    for (int i = 0; i < length; i++)
        //    {
        //        var coords = new float[3];

        //        coords[0] = copyVbo.PointsCoords[3 * i + 0];
        //        coords[1] = copyVbo.PointsCoords[3 * i + 1];
        //        coords[2] = copyVbo.PointsCoords[3 * i + 2];

        //        var coordsv = Vector<float>.Build.Dense(coords);

        //        var rcoordsv = rM.Multiply(coordsv).Add(tV);

        //        copyVbo.PointsCoords[3 * i + 0] = rcoordsv[0];
        //        copyVbo.PointsCoords[3 * i + 1] = rcoordsv[1];
        //        copyVbo.PointsCoords[3 * i + 2] = rcoordsv[2];

        //        var normals = new float[3];

        //        normals[0] = copyVbo.NormalsCoords[3 * i + 0];
        //        normals[1] = copyVbo.NormalsCoords[3 * i + 1];
        //        normals[2] = copyVbo.NormalsCoords[3 * i + 2];

        //        var normalsv = Vector<float>.Build.Dense(normals);

        //        var rnormalsv = rM.Multiply(normalsv).Add(tV);

        //        copyVbo.NormalsCoords[3 * i + 0] = rcoordsv[0];
        //        copyVbo.NormalsCoords[3 * i + 1] = rcoordsv[1];
        //        copyVbo.NormalsCoords[3 * i + 2] = rcoordsv[2];
        //    }
        //}

        private static void GetReflectionData(Geometry.Plane plane, out float[,] reflection, out float[] t)
        {
            var n = Geometry.Vector.GetVectorLength(plane.Normal);

            var nx = plane.Normal._x / n;
            var ny = plane.Normal._y / n;
            var nz = plane.Normal._z / n;
            var d = plane.Shifting / n;

            reflection = new float[3, 3];
            t = new float[3];
            reflection[0, 0] = 1 - 2 * nx * nx;
            reflection[1, 0] = -2 * nx * ny;
            reflection[2, 0] = -2 * nx * nz;
            //reflection[3] = 0.0f;
            reflection[0, 1] = -2 * nx * ny;
            reflection[1, 1] = 1 - 2 * ny * ny;
            reflection[2, 1] = -2 * ny * nz;
            //reflection[7] = 0.0f;
            reflection[0, 2] = -2 * nx * nz;
            reflection[1, 2] = -2 * ny * nz;
            reflection[2, 2] = 1 - 2 * nz * nz;
            //reflection[11] = 0.0f;
            t[0] = -2 * nx * d;
            t[1] = -2 * ny * d;
            t[2] = -2 * nz * d;
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

        private void ChangeVBOColor(string ar, Color color)
        {
            var obj = VBOController.FindVBObj(ar);

            var colors = new float[obj.ColorLength];

            //var count = obj.ColorLength / 4;
            for (int i = 0; i < obj.ColorLength; i += 4)
            {
                colors[i] = Convert.ToInt32(color.R) / 255.0f;
                colors[i + 1] = Convert.ToInt32(color.G) / 255.0f;
                colors[i + 2] = Convert.ToInt32(color.B) / 255.0f;
                colors[i + 3] = Convert.ToInt32(color.A) / 255.0f;
            }
            obj.PointsColors = colors;
        }
    }
}
