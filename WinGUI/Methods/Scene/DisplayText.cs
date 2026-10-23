using WinGUI.Scene.Interfaces;
using System;
using Geometry;
using System.Drawing;
using System.Reflection;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using WinGUI.SettingsControls;

namespace WinGUI
{
    public partial class BaseForm
    {
        event Action DisplayText3DEvent;
        event Action DisplayText2DEvent;
        /// <summary>
        /// Метод шаблон. Использовать везде с оберткой для вывода текста
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        /// <param name="coord"></param>
        public void DisplayText3DTemplate(string str, Color color, Point3D coord)
        {
            GL.PushMatrix();
            GL.Translate(-Position._x, -Position._y, -Position._z);
            GL.Color3(color.R, color.G, color.B);
            GL.RasterPos3(coord._x, coord._y, coord._z);
            GL.PushAttrib(AttribMask.ListBit);//Избегаем пересечений списков, сохраняем старую базу
            GL.ListBase(FontBase);//Устанавливаем базу на fontBase
            var handle = GCHandle.Alloc(str, GCHandleType.Pinned);
            var ptr = handle.AddrOfPinnedObject();
            GL.CallLists(str.Length, ListNameType.UnsignedShort, ptr);
            handle.Free();
            GL.PopAttrib();//Возвращаем старую базу
            GL.PopMatrix();
        }


        public void HideText3D(string searchMethod)
        {
            //var list = PlugDisplayObjectEvent?.GetInvocationList();
            for (int i = 0; i < DisplayText3DEvent?.GetInvocationList().Count(); i++)
            {
                var del = DisplayText3DEvent.GetInvocationList()[i];
                if (del.Method.Name.Contains(searchMethod))
                {
                    DisplayText3DEvent -= (Action)del;
                    i--;
                }
            }
        }

        public void DisplayText3D(string str, Color color, Point3D coord)
        {
            var met = new Action(() =>
            {
                //if (settingsConfig.Transparency && !advanced3DClipper.IsEnable)
                //    averageColorRenderer.DoActionsBeforeDrawing(null, DrawElements.GeometryObjects);
                DisplayText3DTemplate(str, color, coord);
                //if (settingsConfig.Transparency && !advanced3DClipper.IsEnable)
                //    averageColorRenderer.DoActionsAfterDrawing(null, DrawElements.GeometryObjects);

            });
            
            DisplayText3DEvent += met;
        }
        /// <inheritdoc/>

        public void DisplayText2D(string str, Color color, Point2D coord)
        {
            var met = new Action(() =>
            {
                if (settingsConfig.Transparency && !advanced3DClipper.IsEnable)
                    averageColorRenderer.DoActionsBeforeDrawing(null, DrawElements.GeometryObjects);
                GL.MatrixMode(MatrixMode.Projection);
                GL.PushMatrix();
                GL.LoadIdentity();

                var viewport = new int[4];
                GL.GetInteger(GetPName.Viewport, viewport);

                GL.Ortho(0, viewport[2], 0, viewport[3], 0.1, 200);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                GL.LoadIdentity();


                GL.PushMatrix();
                var color = GetTextColor();
                GL.Color3(color.R, color.G, color.B);
                GL.RasterPos3(coord._x, coord._y, -5);
                GL.PushAttrib(AttribMask.ListBit);//Избегаем пересечений списков, сохраняем старую базу
                GL.ListBase(FontBase);//Устанавливаем базу на fontBase
                var handle = GCHandle.Alloc(str, GCHandleType.Pinned);
                var ptr = handle.AddrOfPinnedObject();
                GL.CallLists(str.Length, ListNameType.UnsignedShort, ptr);
                handle.Free();
                GL.PopAttrib();//Возвращаем старую базу
                GL.PopMatrix();

                GL.MatrixMode(MatrixMode.Projection);
                GL.PopMatrix();
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PopMatrix();
                if (settingsConfig.Transparency && !advanced3DClipper.IsEnable)
                    averageColorRenderer.DoActionsAfterDrawing(null, DrawElements.GeometryObjects);
            });
            DisplayText2DEvent += met;
        }
    }
}
