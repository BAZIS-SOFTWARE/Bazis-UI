using BazisGUI.Win.Properties;
using BazisGUI.Win.PropertiesPanel;
using BazisGUI.Win.PropertiesPanel.DataGridViewNumericUpDown;
using Project.Interfaces.Tasks;
using Project.Tasks;
using ResultDB.IO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        enum ResultPropertyKeys { Result, ShowFields, ShowNodesValues, ShowElementsValues, MergeResultsValues, ShowScale, ResultScale, ClarifyValues, MaxScaleValue, MinScaleValue, ScalePrecision, ScaleIntervals, ScaleXPos, ScaleYPos }
        private void navigator_SelectResultsEvent()
        {
            /*
             * TO DO

             * Показывать шкалу
             * Масштаб (TextBox)
             * Уточнить значения (checkBox)
             * Макс. значение (TextBox)
             * Мин. значение (TextBox)
             * Точность (NumericUpDown) тут нужно попробовать создать собственный cell
             *      на базе dataGridViewCell из коробки
             * Интервалы (NumericUpDown) как создать смотри выше
             * Положение шкалы по Х (NumericUpDown) как создать смотри выше
             * Положение шкалы по Y (NumericUpDown) как создать смотри выше
             */

            var rows = GetResultsProperties();
            propertiesPanel.DrawTable(rows);
        }

        public List<RowProperty> GetResultsProperties()
        {
            List<RowProperty> rows = new List<RowProperty>
            {
                new RowProperty(ResultPropertyKeys.ShowFields.ToString(), Resources.Header_result_showFields, settingsConfig.ShowResultsField),
                new RowProperty(ResultPropertyKeys.ShowNodesValues.ToString(), Resources.Header_result_showNodesValues, settingsConfig.ShowNodeResultsValue),
                new RowProperty(ResultPropertyKeys.ShowElementsValues.ToString(), Resources.Header_result_showElementsValues, settingsConfig.ShowElementsResultsValue),
                new RowProperty(ResultPropertyKeys.MergeResultsValues.ToString(), Resources.Header_result_averageValues, settingsConfig.MergeResultsValue),
                new RowProperty(ResultPropertyKeys.ShowScale.ToString(), Resources.Header_result_showScale, settingsConfig.ShowResultsScale)
            };

            if (settingsConfig.ShowResultsScale)
            {
                rows.Add(new RowProperty(ResultPropertyKeys.ResultScale.ToString(), Resources.Header_result_resultScale, settingsConfig.Scale_scale));
                rows.Add(new RowProperty(ResultPropertyKeys.ClarifyValues.ToString(), Resources.Header_result_clarifyValues, settingsConfig.IsScaleMaxMinManual));

                if (settingsConfig.IsScaleMaxMinManual)
                {
                    rows.Add(new RowProperty(ResultPropertyKeys.MaxScaleValue.ToString(), Resources.Header_result_maxScaleValue, settingsConfig.Scale_MaxValue));
                    rows.Add(new RowProperty(ResultPropertyKeys.MinScaleValue.ToString(), Resources.Header_result_minScaleValue, settingsConfig.Scale_MinValue));
                }

                rows.Add(new RowProperty(ResultPropertyKeys.ScalePrecision.ToString(), Resources.Header_result_precision, 
                    new NumericUpDownValue(settingsConfig.Scale_Precision, 0, 15, 0, 1)));

                rows.Add(new RowProperty(ResultPropertyKeys.ScaleIntervals.ToString(),Resources.Header_result_intervals, 
                    new NumericUpDownValue(settingsConfig.Scale_Intervals, 2, 10, 0, 1)));

                rows.Add(new RowProperty(ResultPropertyKeys.ScaleXPos.ToString(), Resources.Header_result_xPos,
                    new NumericUpDownValue(settingsConfig.Scale_X_Coord, 0, 2000, 0, 1)));
                rows.Add(new RowProperty(ResultPropertyKeys.ScaleYPos.ToString(), Resources.Header_result_yPos,
                    new NumericUpDownValue(settingsConfig.Scale_Y_Coord, 0, 2000, 0, 1)));
            }

            return rows;
        }
    }
}
