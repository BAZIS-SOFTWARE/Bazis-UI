using Avalonia;
using global::Avalonia.Controls;
using global::Avalonia.Layout;
using global::Avalonia.Markup.Xaml.Styling;
using BazisGUI.Avalonia.Shell;
using System;
using System.Collections.Generic;
using System.Resources;

namespace BazisGUI.Avalonia.AdvanceSelection
{
    /// <summary>
    /// Avalonia-аналог GeomSelect: дополнительный выбор геометрии по объемлющим объектам
    /// (объёмам, поверхностям или кривым).
    /// </summary>
    internal sealed class GeomSelect : UserControl
    {
        private readonly Dictionary<SelectionType, (bool volume, bool surface, bool curve)> _modes = new()
        {
            { SelectionType.Points, (true, true, true) },
            { SelectionType.Curves, (true, true, false) },
            { SelectionType.Surfaces, (true, false, false) }
        };
        public event Action CloseForm;

        private readonly RadioButton rbtVolume;
        private readonly RadioButton rbtSurface;
        private readonly RadioButton rbtCurve;

        public GeomSelect(SelectionType selectedObjects)
        {
            Styles.Add(new StyleInclude(new Uri("avares://BazisGUI.Avalonia/"))
            {
                Source = new Uri("avares://BazisGUI.Avalonia/AdvanceSelection/AdvancedSelectionStyles.axaml")
            });
            var resources = new ResourceManager(typeof(GeomSelect));
            rbtVolume = new RadioButton { Name = "rbtVolume", Content = resources.GetString("rbtVolume.Text"), GroupName = "geomSelect", IsChecked = true };
            rbtSurface = new RadioButton { Name = "rbtSurface", Content = resources.GetString("rbtSurface.Text"), GroupName = "geomSelect" };
            rbtCurve = new RadioButton { Name = "rbtCurve", Content = resources.GetString("rbtCurve.Text"), GroupName = "geomSelect" };

            // WinForms: клиентская область 225 x 90, три строки по 30 px.
            var generalPanel = new Grid
            {
                Name = "generalPanel",
                Width = 225,
                RowDefinitions = new RowDefinitions("30,30,30")
            };
            var radios = new[] { rbtVolume, rbtSurface, rbtCurve };
            for (var row = 0; row < radios.Length; row++)
            {
                radios[row].Margin = new Thickness(8, 0, 0, 0);
                radios[row].VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(radios[row], row);
                generalPanel.Children.Add(radios[row]);
            }
            Content = generalPanel;

            SetAvailableModes(selectedObjects);
        }

        public void SetAvailableModes(SelectionType selectedObjects)
        {
            if (_modes.TryGetValue(selectedObjects, out var mode))
                SetApply(mode.volume, mode.surface, mode.curve);
            else
                CloseForm?.Invoke();
        }

        /// <summary>Размерность объемлющих объектов, по которым расширяется выбор; -1 — режим не выбран.</summary>
        public int GetSelectDimension()
        {
            if (rbtVolume.IsChecked == true)
                return 3;
            if (rbtSurface.IsChecked == true)
                return 2;
            if (rbtCurve.IsChecked == true)
                return 1;
            return -1;
        }

        private void SetApply(bool volume, bool surface, bool curve)
        {
            rbtVolume.IsEnabled = volume;
            rbtSurface.IsEnabled = surface;
            rbtCurve.IsEnabled = curve;

            // Отмеченная размерность стала недоступна для нового типа объектов (например, кривые
            // после перехода с точек на поверхности) — возвращаемся к объёмам, доступным всегда.
            if ((rbtSurface.IsChecked == true && !surface) || (rbtCurve.IsChecked == true && !curve))
                rbtVolume.IsChecked = true;
        }
    }
}
