using Avalonia.Controls;
using Avalonia.VisualTree;
using AvaloniaGUI.Localization;
using AvaloniaGUI.SettingsControls;
using SceneCore.Interfaces;
using Model.Interfaces;
using System;
using System.Linq;

namespace AvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var btn = sender as MenuItem;
            var name = Resources.BaseForm_настройкиToolStripMenuItem_Click_Settings;
            // CheckOnClick: MenuItem с ToggleType = CheckBox переключает IsChecked до события Click.
            if (btn.IsChecked)
            {
                var settings = new SettingsControl();
                // Сообщение о сохранении — только если на странице что-то изменили.
                RememberSavedConfig();
                SubscribeLeave(settings, Settings_Leave);
                settings.SetSettings(settingsConfig);
                SetSettingsToConfig(settings);
                TabButtonsService.AddControl(name, settings);
            }
            else
                TabButtonsService.RemoveControl(name);
        }

        private void Settings_Leave(object sender, EventArgs e)
        {
            SaveConfigIfChanged(showMessage: !isClosing);
        }

        /// <summary>
        /// Аналог события Control.Leave: фокус перешёл с элемента внутри <paramref name="control"/>
        /// на другой элемент этого же окна.
        /// </summary>
        /// <remarks>
        /// Потеря фокуса клавиатуры при активации другого окна (сообщения, диалога выбора цвета) событием
        /// Leave в WinForms не является. Если реагировать на неё, окно сообщения SaveConfig само снимает
        /// фокус со страницы и вызывает новое сохранение — сообщения появляются бесконечно.
        /// </remarks>
        private void SubscribeLeave(Control control, EventHandler leave)
        {
            var focusInside = false;
            control.GotFocus += (_, _) => focusInside = true;

            void OnWindowGotFocus(object sender, Avalonia.Interactivity.RoutedEventArgs e)
            {
                if (!focusInside || e.Source is not Avalonia.Visual focused || control.IsVisualAncestorOf(focused))
                    return;

                focusInside = false;
                leave(control, EventArgs.Empty);
            }

            AddHandler(GotFocusEvent, OnWindowGotFocus, Avalonia.Interactivity.RoutingStrategies.Bubble, handledEventsToo: true);
            // Страницу можно скрыть (переключение вкладок) и показать снова — подписка восстанавливается.
            control.AttachedToVisualTree += (_, _) =>
            {
                RemoveHandler(GotFocusEvent, OnWindowGotFocus);
                AddHandler(GotFocusEvent, OnWindowGotFocus, Avalonia.Interactivity.RoutingStrategies.Bubble, handledEventsToo: true);
            };
            control.DetachedFromVisualTree += (_, _) =>
            {
                RemoveHandler(GotFocusEvent, OnWindowGotFocus);
                // Страницу закрыли, не уводя с неё фокус: изменения тоже сохраняются (если они есть).
                if (focusInside)
                {
                    focusInside = false;
                    leave(control, EventArgs.Empty);
                }
            };
        }

        // Цвет фона, освещение, проекция, прозрачность отрисовки и положение источника света в BaseForm
        // передаются sceneController/averageColorRenderer; здесь — через ApplySceneSettings (SceneConnection.cs).
        private void SetSettingsToConfig(SettingsControl settings)
        {

            settings.SetSelectionGroupColorEvent += (ar) =>

            {
                settingsConfig.SelectGroupColor = ar;
            };
            settings.SetSelectionObjectColorEvent += (ar) =>
            {
                settingsConfig.SelectObjectColor = ar;
                ApplySelectionColor();
            };

            settings.SetNodeColorEvent += (ar) =>
            {
                settingsConfig.NodeColor = ar;
                var setInfo = project?.GetModelSetsInfo(ObjType.Узел).FirstOrDefault();
                if (setInfo != null)
                    project.SetColor(setInfo, ar);
            };

            // В BaseForm события цвета 2D/3D-элементов не обрабатывались: цвет задаётся наборам текущей модели,
            // как цвет узлов.
            settings.Set3DElemColorEvent += (ar) => SetElementsColor(ObjType.Элемент3D, ar);
            settings.Set2DElemColorEvent += (ar) => SetElementsColor(ObjType.Элемент2D, ar);

            settings.SetBackRibbersEvent += (ar) =>
            {
                settingsConfig.BackRibbers = ar;
                ApplySceneSettings();
            };

            settings.SetSolverPathEvent += (ar) =>
            {
                settingsConfig.SolverPath = ar;
            };
            settings.SetBackGroundColorEvent += (ar) =>
            {
                settingsConfig.BackGroundColor = ar;
                ApplySceneSettings();
            };


            settings.SetLightingEvent += (ar) =>
            {
                settingsConfig.Lighting = ar;
                ApplySceneSettings();
            };

            settings.SetTransparencyEvent += (ar) =>
            {
                settingsConfig.Transparency = ar;
                ApplySceneSettings();
                // Альфа ModelView задаётся только в режиме прозрачности (GetModelViewTransparency)
                project?.SetTransparency(GetModelViewTransparency());
                ClearAllDataOnScene();
                if (project != null)
                    CreateVBObjects("Объекты");
                RequestRedraw();
            };

            settings.SetOrtoProjectionEvent += (ar) =>
            {
                settingsConfig.Projection = ar ? ViewProjection.Parallel : ViewProjection.Perspective;
                ApplySceneSettings(applyProjection: true);
            };

            settings.SetTransparencyValueEvent += (ar1) =>
            {
                settingsConfig.TransparencyValue = ar1;
                if (project != null)
                    project.SetTransparency(GetModelViewTransparency());
            };

            settings.SetLightingIntensityEvent += (ar) =>
            {
                settingsConfig.LightingIntensity = ar;
                ApplySceneSettings(applyLight: true);
            };


            settings.SetLighterPositionEvent += (ar) =>
            {
                var kx = (float)(Bounds.Width / settings.Bounds.Width);
                var ky = (float)(Bounds.Height / settings.Bounds.Height);

                var x = ar.X * kx;
                var y = ar.Y * ky;

                settingsConfig.LighterPosition.X = (int)x;
                settingsConfig.LighterPosition.Y = (int)y;

                ApplySceneSettings(applyLight: true);
            };

            settings.SetLanguageEvent += (ar) =>
            {
                if (settingsConfig.Language != ar)
                {
                    MessageBox.Show(this, Resources.MainMenuEvents_ChangeLanguage_Message, Localization.Localization.GetAttentionCaption());
                    settingsConfig.Language = ar;
                }
            };
        }

        /// <summary>Цвет всех наборов элементов заданного типа в текущей модели.</summary>
        private void SetElementsColor(ObjType elementType, System.Drawing.Color color)
        {
            if (project == null)
                return;

            using (project.BeginViewUpdate())
                foreach (var setInfo in project.GetModelSetsInfo(elementType))
                    project.SetColor(setInfo, color);
        }
    }
}
