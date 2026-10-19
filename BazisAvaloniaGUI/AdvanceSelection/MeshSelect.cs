using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Styling;
using BazisAvaloniaGUI;
using BazisAvaloniaGUI.Utilities;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace BazisAvaloniaGUI.AdvanceSelection
{
    /// <summary>
    /// Avalonia-аналог MeshSelect: режимы дополнительного выбора сеточных объектов —
    /// по наборам, в плоскости и по направлению.
    /// </summary>
    internal sealed class MeshSelect : UserControl
    {
        private SelectInDirectionEventArgs directionConfig;
        private SelectInPlainEventArgs plainConfig;

        private readonly Dictionary<SelectionType, (bool set, bool surface, bool direction, bool other)> _modes = new()
        {
            { SelectionType.Elements1D, (true, false, false, false) },
            { SelectionType.Elements2D, (true, true, false, true) },
            { SelectionType.Elements3D, (true, false, false, false) },
            { SelectionType.Nodes,      (true, true, true, true) }
        };
        public event Action CloseForm;
        private ObjType selectType;

        private readonly RadioButton rbtSet;
        private readonly RadioButton rbtSurface;
        private readonly RadioButton rbtDirection;
        private readonly TextBlock lblAngle;
        private readonly TextBox txbAngle;
        private readonly CheckBox chbChangeDirection;

        public MeshSelect(SelectionType selectedObjects)
        {
            Styles.Add(new StyleInclude(new Uri("avares://BazisAvaloniaGUI/"))
            {
                Source = new Uri("avares://BazisAvaloniaGUI/AdvanceSelection/AdvancedSelectionStyles.axaml")
            });
            var resources = new ResourceManager(typeof(MeshSelect));
            rbtSet = new RadioButton { Name = "rbtSet", Content = resources.GetString("rbtSet.Text"), GroupName = "meshSelect", IsChecked = true };
            rbtSurface = new RadioButton { Name = "rbtSurface", Content = resources.GetString("rbtSurface.Text"), GroupName = "meshSelect" };
            rbtDirection = new RadioButton { Name = "rbtDirection", Content = resources.GetString("rbtDirection.Text"), GroupName = "meshSelect" };
            foreach (var radio in new[] { rbtSet, rbtSurface, rbtDirection })
            {
                radio.Margin = new Thickness(8, 0, 0, 0);
                radio.VerticalAlignment = VerticalAlignment.Center;
                radio.IsCheckedChanged += Rbt_CheckedChanged;
            }

            lblAngle = new TextBlock { Name = "lblAngle", Text = resources.GetString("lblAngle.Text"), VerticalAlignment = VerticalAlignment.Center };
            lblAngle.Margin = new Thickness(8, 0, 0, 0);
            txbAngle = new TextBox
            {
                Name = "txbAngle",
                Text = resources.GetString("txbAngle.Text") ?? "5",
                Height = 20,
                MinHeight = 20,
                Padding = new Thickness(3, 0),
                Margin = new Thickness(0, 0, 20, 0),
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            chbChangeDirection = new CheckBox
            {
                Name = "chbChangeDirection",
                Content = resources.GetString("chbChangeDirection.Text"),
                Margin = new Thickness(8, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            chbChangeDirection.IsCheckedChanged += chbChangeDirection_CheckedChanged;

            // WinForms: клиентская область 225 x 150, пять строк по 30 px, колонки 30/70%.
            var generalPanel = new Grid
            {
                Name = "generalPanel",
                Width = 225,
                RowDefinitions = new RowDefinitions("30,30,30,30,30"),
                ColumnDefinitions = new ColumnDefinitions("3*,7*")
            };
            var radios = new[] { rbtSet, rbtSurface, rbtDirection };
            for (var row = 0; row < radios.Length; row++)
            {
                Grid.SetRow(radios[row], row);
                Grid.SetColumnSpan(radios[row], 2);
                generalPanel.Children.Add(radios[row]);
            }
            Grid.SetRow(lblAngle, 3);
            Grid.SetRow(txbAngle, 3);
            Grid.SetColumn(txbAngle, 1);
            Grid.SetRow(chbChangeDirection, 4);
            Grid.SetColumnSpan(chbChangeDirection, 2);
            generalPanel.Children.Add(lblAngle);
            generalPanel.Children.Add(txbAngle);
            generalPanel.Children.Add(chbChangeDirection);
            Content = generalPanel;

            SetAvailableModes(selectedObjects);
        }

        public void SetAvailableModes(SelectionType selectedObjects)
        {
            if (_modes.TryGetValue(selectedObjects, out var mode))
            {
                SetApply(mode.set, mode.surface, mode.direction, mode.other);
                selectType = Converters.ConvertSelectionTypeToObjType(selectedObjects);
                directionConfig = null;
                plainConfig = null;
            }
            else
                CloseForm?.Invoke();
        }

        public object GetSelectedAdditionalMode()
        {
            // Угол берётся при каждом выборе: раньше накопленная конфигурация сохраняла угол,
            // введённый до первого щелчка, и его изменение не действовало до смены режима.
            if (rbtDirection.IsChecked == true)
            {
                if (directionConfig == null)
                    directionConfig = new SelectInDirectionEventArgs(ObjType.Узел, chbChangeDirection.IsChecked == true, ParseAngle());
                else
                    directionConfig.Angle = ParseAngle();
                return directionConfig;
            }

            else if (rbtSurface.IsChecked == true)
            {
                if (plainConfig == null)
                    plainConfig = new SelectInPlainEventArgs(selectType, ParseAngle());
                else
                    plainConfig.Angle = ParseAngle();
                return plainConfig;
            }
            else
                return selectType;
        }

        private float ParseAngle()
        {
            // Пустое или нечисловое поле: вызов идёт из обработчика выбора на сцене, где FormatException
            // не перехватывается, — восстанавливаем угол по умолчанию, как сброс поля в других панелях.
            if (float.TryParse((txbAngle.Text ?? string.Empty).Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var angle))
                return angle;
            txbAngle.Text = DefaultAngle.ToString(CultureInfo.InvariantCulture);
            return DefaultAngle;
        }

        private const float DefaultAngle = 5;

        private void SetApply(bool set, bool surface, bool direction, bool other)
        {
            rbtSet.IsEnabled = set;
            rbtSurface.IsEnabled = surface;
            rbtDirection.IsEnabled = direction;
            lblAngle.IsEnabled = other;
            txbAngle.IsEnabled = other;
            chbChangeDirection.IsEnabled = other;

            // Отмеченный режим стал недоступен для нового типа объектов (например, «по направлению»
            // после перехода с узлов на 2D-элементы) — возвращаемся к выбору по наборам, доступному всегда.
            if ((rbtSurface.IsChecked == true && !surface) || (rbtDirection.IsChecked == true && !direction))
                rbtSet.IsChecked = true;
        }

        private void chbChangeDirection_CheckedChanged(object sender, EventArgs e)
        {
            if (directionConfig != null)
                directionConfig.Reverse = chbChangeDirection.IsChecked == true;
        }

        private void Rbt_CheckedChanged(object sender, EventArgs e)
        {
            directionConfig = null;
            plainConfig = null;
        }
    }
}
