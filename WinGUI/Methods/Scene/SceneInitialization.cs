using SceneCore;
using SceneCore.Text;
using WinGUI.Scene.Interfaces;
using System;
using System.Collections.Generic;
using Geometry;
using System.Drawing;
using WinGUI.Scene.VBO;
using WinGUI.Scene;
using System.Reflection;
using OpenTK.Graphics.OpenGL;
using static WinGUI.Methods.PlatformSpecific.PlatformSpecific;
using System.Threading;

namespace WinGUI
{
    public partial class BaseForm
    {
        // Следующий шаг миграции (GUI/Documents/scene.avalonia.md, шаг 4): камера, VBOController,
        // рендер базиса/компаса/точки вращения/модельных объектов и матрицы вида/проекции
        // теперь считает SceneController (см. BaseForm.cs — VBOController/averageColorRenderer/
        // advanced3DClipper там же переведены в проброс к нему).
        //
        // DisplayGeometryObjectEvent/DisplayText3DEvent/DisplayText2DEvent/DisplayClipPlaneEvent
        // сознательно НЕ тронуты: на них напрямую завязаны ~20 файлов вне Methods/Scene
        // (Navigator, Console, Results, UtilityToolStrip и т.д.), полный перенос которых —
        // отдельный шаг с проверкой каждого сценария. DisplayObjects() по-прежнему их вызывает,
        // поэтому старый шрифт (FontBase/ChangeTextFont, ниже) тоже остаётся: это их источник
        // глифов. SceneController заводит для СВОИХ слоёв (компас) отдельный текстовый рендерер —
        // это два независимых диапазона display list'ов, конфликта нет.
        /// <summary>
        /// Результат последнего ScenePicker.SelectByPoint/SelectByRect внутри sceneController —
        /// см. SelectByPoint.cs/SelectByRect.cs. Core не знает о "project"/IModelView, поэтому
        /// сообщает о найденных объектах через InfoRequested, а не выполняет выбор сам.
        /// </summary>
        private readonly List<(string Name, IEnumerable<int> Numbers)> pickHits = new();

        public void SceneInitialization(object sender, EventArgs args)
        {
            if (scene.Profile != OpenTK.Windowing.Common.ContextProfile.Compatability)
                throw new Exception("Используется deprecated код, задайте для экземпляра класса GLControl свойство Profile = Compatability");

            sceneController = new SceneController(new WglBitmapFontTextRenderer());
            sceneController.GetCamera().Width = scene.Width;
            sceneController.GetCamera().Height = scene.Height;
            sceneController.Initialization();
            sceneController.InfoRequested += (s, e) => pickHits.Add((e.ObjsName, e.GetObjectsIndexes()));

            FontBase = GL.GenLists(1150);//кол-во глифов (элементов для рисования букв 256 - только латиница, 1150 - поддержка еще и кирилицы)
            ChangeTextFont();//Используем шрифт по-умолчанию

            if (sceneController.TextRenderer is WglBitmapFontTextRenderer wglTextRenderer)
                wglTextRenderer.AttachToCurrentContext(GetDeviceContext());

            Disposed += (s, e) =>
            {
                foreach (var obj in VBOController.GetVBObjs())
                    VBO.DeleteAllBuffers(obj);
                sceneController.Dispose();
                GL.DeleteLists(FontBase, 1150);
            };

            scene.Paint += (arg1, arg2) => DisplayObjects();
            scene.SizeChanged += GlControl_Resize;
            //scene.KeyDown += GlControl_KeyDown;
            scene.MouseDown += GlControl_MouseDown;
            scene.MouseUp += GlControl_MouseUp;
            scene.MouseMove += GlControl_MouseMove;
            //scene.MouseWheel += GlControl_MouseWheel;
            scene.MouseClick += scene_MouseClick;
        }

        /// <summary>
        /// Для корректного отображения шрифтов нужен HDC окна, созданного на этапе вызова метода scene.InitializeContexts();
        /// Однако оно приватное, мы можем получить его через рефлексию
        /// </summary>
        /// <returns>IntPtr - deviceContext</returns>
        private IntPtr GetDeviceContext()
        {
            scene.MakeCurrent();
            return GetCurrentDC();
        }
        /// <summary>
        /// Использовать шрифты по умолчанию или задать свой, для отображения текста
        /// </summary>
        /// <param name="fBase">Индекс сгенерированный с помощью комманды Gl.GenLists()</param>
        /// <param name="fontFamily">Семейство шрифтов например "Times New Roman"</param>
        /// <param name="size">Размер шрифта</param>
        /// <param name="style">Курсив(Italic), жирный(Bold) и т.д</param>
        private void ChangeTextFont(string fontFamily = "", float size = 8.25f, FontStyle style = FontStyle.Regular)
        {
            var hdc = GetDeviceContext();
            if (string.IsNullOrEmpty(fontFamily))
            {
                var status = UseFontBitmapsW(hdc, 0, 1150, FontBase);
                if (!status)
                    throw new Exception("Не удалось загрузить глифы для шрифта");
            }
            else
            {
                var font = new Font(fontFamily, size, style);
                var hFont = font.ToHfont();

                //Вызов системных функций, для корректной замены шрифта!
                var oldFont = SelectObject(hdc, hFont);//Делаем Swap шрифтов
                var status = UseFontBitmapsW(hdc, 0, 1150, FontBase);

                SelectObject(hdc, oldFont);//Делаем текущим старый шрифт
                DeleteObject(hFont);//Обязательно освобождаем неуправляемый ресурс
                if (!status)
                    throw new Exception("Не удалось загрузить глифы для шрифта");
            }
        }
    }
}
