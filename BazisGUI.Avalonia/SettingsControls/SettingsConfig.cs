using BazisGUI.SceneCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BazisGUI.Avalonia.SettingsControls
{
    [Serializable]
    public class SettingsConfig
    {
        //view settings
        public Color SelectObjectColor;
        public Color SelectGroupColor;
        public Color BackGroundColor;
        public Color NodeColor;
        public string SolverPath;
        public string SolverFile;
        public bool Lighting;
        public int LightingIntensity;
        public Point LighterPosition;
        public bool Transparency;
        public bool BackRibbers;
        /// <summary>Непрозрачность модели в процентах (0..100). 0 делает грани невидимыми.</summary>
        public int TransparencyValue = 100;
        public ViewProjection Projection = ViewProjection.Perspective;
        public bool DisplayBasis = true;
        public bool DisplayCompass = true;
        public bool IsCutting = false;
        public float RotationAngle = 2.5f;
        public ViewAxis RotationAxis = ViewAxis.XYZ;
        public float AngleOfProjection = 2.5f;

        //result settings
        public bool ShowResultsField;
        public bool ShowResultsScale;
        public bool ShowNodeResultsValue;
        public bool ShowElementsResultsValue;
        public bool MergeResultsValue;
        public bool IsScaleMaxMinManual;
        public int Scale_X_Coord;
        public int Scale_Y_Coord;
        public float Scale_MaxValue;
        public float Scale_MinValue;
        public int Scale_Precision = 2;
        public int Scale_Intervals = 10;
        public int Scale_scale = 1;
        internal bool ShowNodesOnCurves;

        public bool ShowAllMeshWhenGeneration;

        //cond settings
        public bool CheckCondValue;

        public string Language = "en";

        /// <summary>Восстанавливает допустимое число интервалов для старых конфигураций.</summary>
        public void NormalizeResultScaleSettings()
        {
            Scale_Intervals = Scale_Intervals == 0 ? 10 : Math.Clamp(Scale_Intervals, 2, 10);
        }

        /// <summary>
        /// Приводит TransparencyValue к процентам: до перехода на ModelView значение хранилось в 0..255.
        /// Явно сохранённый 0 сохраняется; отсутствующее в файле поле получает 100 из инициализатора.
        /// </summary>
        public void NormalizeTransparencyValue()
        {
            if (TransparencyValue > 100)
                TransparencyValue = Math.Min(TransparencyValue, 255) * 100 / 255;
            if (TransparencyValue < 0)
                TransparencyValue = 0;
        }
    }
}
