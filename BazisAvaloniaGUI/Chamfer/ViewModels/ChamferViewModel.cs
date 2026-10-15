using BazisAvaloniaGUI.Chamfer.Models;
using BazisAvaloniaGUI.Chamfer.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Globalization;

namespace BazisAvaloniaGUI.Chamfer.ViewModels
{
    public partial class ChamferViewModel : ObservableObject
    {
        private readonly IChamferOperationService operationService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddCommand))]
        private string angleLength = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddCommand))]
        private string angle = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddCommand))]
        private string firstLength = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddCommand))]
        private string secondLength = string.Empty;
        private bool isReflected = false;
        public double ReflectScaleX => isReflected ? -1 : 1;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsAngleMode))]
        [NotifyPropertyChangedFor(nameof(IsLengthsMode))]
        [NotifyCanExecuteChangedFor(nameof(AddCommand))]
        private ChamferMode mode = ChamferMode.Angle;

        public ChamferViewModel(IChamferOperationService operationService)
        {
            this.operationService = operationService ?? throw new ArgumentNullException(nameof(operationService));
        }

        public bool IsAngleMode => Mode == ChamferMode.Angle;

        public bool IsLengthsMode => Mode == ChamferMode.Lengths;

        [RelayCommand]
        private void SelectMode(ChamferMode mode) => Mode = mode;
        
        [RelayCommand(CanExecute = nameof(CanAdd))]
        private void Add()
        {
            if (Mode == ChamferMode.Angle)
            {
                if (TryParseNumber(AngleLength, out var length) && TryParseNumber(Angle, out var angleValue))
                    operationService.AddByAngle(length, angleValue, isReflected);
            }
            else if (Mode == ChamferMode.Lengths)
            {
                if (TryParseNumber(FirstLength, out var length1) && TryParseNumber(SecondLength, out var length2))
                    operationService.AddByLengths(length1, length2, isReflected);
            }

            // Окно не закрывается здесь: операция выполняется асинхронно (Post в UI-контекст),
            // и при ошибке пользователь должен иметь возможность поправить параметры.
            // При успехе окно закрывает MainWindow.CreateChamfer через ChamferWindowService.Close.
        }

        [RelayCommand]
        private void Reflect()
        {
            isReflected = !isReflected;
            OnPropertyChanged(nameof(ReflectScaleX));
            CanAdd();
        }
        
        private bool CanAdd()
        {
            double angle = 0, angleValue = 0, length1 = 0, length2 = 0;
            var canAdd = Mode == ChamferMode.Angle
                ? TryParseNumber(AngleLength, out angle) && TryParseNumber(Angle, out angleValue)
                : TryParseNumber(FirstLength, out length1) && TryParseNumber(SecondLength, out length2);
            if (canAdd)
                if (Mode == ChamferMode.Angle)
                    operationService.Prewiew(angle, angleValue, true, isReflected);
                else
                    operationService.Prewiew(length1, length2, false, isReflected);
            else
                operationService.ClearPreview();
            return canAdd;
        }

        private static bool TryParseNumber(string text, out double value)
        {
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
                return true;

            return double.TryParse(text?.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }
    }
}
