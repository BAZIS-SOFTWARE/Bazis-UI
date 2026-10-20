using BazisGUI.Navigator;
using BazisGUI.PropertiesPanel;
using Project.Interfaces.Tasks;
using Project.TaskParameters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BazisGUI
{
    public partial class BaseForm
    {
        enum SelectCompKeys { ApplyForAll }
        private void Navigator_SelectCompEvent(string arg2)
        {
            try
            {
                var parameters = preProc.ElementAt(navigator.SelectedNode.Index);
                var isExe = parameters.Status == TaskStatus.выполнить;
                List<RowProperty> rows = new List<RowProperty>();
                rows.Add(new RowProperty(CompPropertyKeys.Execute.ToString(), Properties.Resources.Header_comp_Execute, isExe));
                
                if (parameters is ChemicalParameters cmp)
                    rows.AddRange(GetPropertyChemicalTask(cmp));
                else if (parameters is MechanicalParameters mhp)
                    rows.AddRange(GetPropertyMechanicalTask(mhp));
                else if (parameters is TermalParameters tmp)
                    rows.AddRange(GetPropertyTermalTask(tmp));

                rows.AddRange(GetPropertySolverSettings(parameters));
                rows.AddRange(GetPropertyBasic(parameters));
                rows.AddRange(GetPropertyTimeSettings(parameters));
                var availableResults = GetInstructionResultNames(parameters);
                var availableInitialResults = GetInstructionResultNames(parameters, parameters.TaskKind);
                rows.AddRange(GetPropertyInitialState(parameters, availableInitialResults));
                rows.AddRange(GetPropertyInputFields(parameters, availableResults));
                rows.Add(new RowProperty(SelectCompKeys.ApplyForAll.ToString(), Properties.Resources.Header_comp_ApplyForAll, new ButtonPropertyValue(Properties.Resources.OK, () => ApplySettingsToAllInstructions())));
                propertiesPanel.DrawTable(rows);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private string selectInstruction = string.Empty;
        

        private List<RowProperty> GetPropertySolverSettings(GeneralParameters parameters) 
        {
            var solver = EnsureSolverSettings(parameters);

            return new List<RowProperty>
            {
                new RowProperty(CompPropertyKeys.Algorithm.ToString(), Properties.Resources.Header_comp_Algorithm,
                new DropDownPropertyValue(solver.Solver,
                Enum.GetNames<LinearSolverKind>().ToList())),

                new RowProperty(CompPropertyKeys.MatrixStorage.ToString(), Properties.Resources.Header_comp_MatrixStorage,
                new DropDownPropertyValue(solver.MatrixStorage,
                Enum.GetNames<MatrixStorageKind>().ToList())),

                new RowProperty(CompPropertyKeys.SolveIterations.ToString(), Properties.Resources.Header_comp_SolveIterations, solver.MaxIter),
                new RowProperty(CompPropertyKeys.SolveAccuracy.ToString(), Properties.Resources.Header_comp_SolveAccuracy, solver.Precision),
                new RowProperty(CompPropertyKeys.RelaxationCoef.ToString(), Properties.Resources.Header_comp_RelaxationCoef, solver.Relaxation),
                new RowProperty(CompPropertyKeys.MaxRelaxationCoef.ToString(), Properties.Resources.Header_comp_MaxRelaxationCoef, solver.MaxRelaxation),

                new RowProperty(CompPropertyKeys.Priority.ToString(), Properties.Resources.Header_comp_Priority,
                new DropDownPropertyValue(solver.Priority,
                Enum.GetValues<PriorityKeys>().Select(x => x.ToString()).ToList()))
            };
        }

        private List<RowProperty> GetPropertyBasic(GeneralParameters parameters)
        {
            return new List<RowProperty>
            {
                new RowProperty(CompPropertyKeys.IterationOnStep.ToString(), Properties.Resources.Header_comp_IterationsOnStep, parameters.Iterations),
                new RowProperty(CompPropertyKeys.SaveRate.ToString(), Properties.Resources.Header_comp_SaveRate, parameters.SaveRate)
            };
        }

        private List<RowProperty> GetPropertyTimeSettings(GeneralParameters parameters)
        {
            return new List<RowProperty>
            {
                new RowProperty(CompPropertyKeys.StartTime.ToString(), Properties.Resources.Header_comp_StartTime, parameters.TimeSettings.StartTime),
                new RowProperty(CompPropertyKeys.StopTime.ToString(), Properties.Resources.Header_comp_StopTime, parameters.TimeSettings.StopTime),
                new RowProperty(CompPropertyKeys.InitialSolveStep.ToString(), Properties.Resources.Header_comp_InitialSolveStep, parameters.TimeSettings.InitTimeStep),
                new RowProperty(CompPropertyKeys.MinSolveStep.ToString(), Properties.Resources.Header_comp_MinSolveStep, parameters.TimeSettings.MinTimeStep),
                new RowProperty(CompPropertyKeys.MaxSolveStep.ToString(), Properties.Resources.Header_comp_MaxSolveStep, parameters.TimeSettings.MaxTimeStep)
            };
        }

        /// <summary>
        /// Строки родного начального набора полей: имя набора, источник данных
        /// и значения величин по элементным группам.
        /// </summary>
        private List<RowProperty> GetPropertyInitialState(GeneralParameters parameters, List<string> availableResults)
        {
            var fieldSet = GetNativeFieldSet(parameters);
            var field = FindInitialField(parameters, fieldSet);

            var rows = new List<RowProperty>
            {
                // Подпись: показывает, какой набор для задачи родной. Ключ ничего не настраивает.
                new RowProperty(PhysicalSetLabelKey, Properties.Resources.Header_comp_NativeSet,
                    PhysicalFieldSetHeader(fieldSet), true),

                new RowProperty(ComposeFieldKey(CompPropertyKeys.InitialStateSource, fieldSet),
                    Indent(1, Properties.Resources.Header_comp_InputSource),
                    SourceValue(field))
            };

            rows.AddRange(GetPropertySetValues(CompPropertyKeys.InitialStateValue,
                CompPropertyKeys.InitialStateFile, parameters, fieldSet, field, availableResults, 1));

            return rows;
        }

        /// <summary>
        /// Строки внешних наборов полей: флажок наличия, источник данных
        /// и значения величин по элементным группам.
        /// </summary>
        private List<RowProperty> GetPropertyInputFields(GeneralParameters parameters, List<string> availableResults)
        {
            var rows = new List<RowProperty>();
            var fieldSets = InputFieldSetsToShow(parameters);

            if (fieldSets.Count == 0)
                return rows;

            // Подпись, отделяющая неродные наборы от родного.
            rows.Add(new RowProperty(PhysicalSetLabelKey,
                Properties.Resources.Header_comp_ForeignSets, string.Empty, true));

            foreach (var fieldSet in fieldSets)
            {
                var field = FindInputField(parameters, fieldSet);

                rows.Add(new RowProperty(ComposeFieldKey(CompPropertyKeys.InputEnabled, fieldSet),
                    Indent(1, PhysicalFieldSetHeader(fieldSet)), field != null));

                if (field == null)
                    continue;

                rows.Add(new RowProperty(ComposeFieldKey(CompPropertyKeys.InputSource, fieldSet),
                    Indent(2, Properties.Resources.Header_comp_InputSource),
                    SourceValue(field)));

                rows.AddRange(GetPropertySetValues(CompPropertyKeys.InputValue,
                    CompPropertyKeys.InputFile, parameters, fieldSet, field, availableResults, 2));
            }

            return rows;
        }

        /// <summary>
        /// Строки, состав которых зависит от источника набора: имя файла результатов
        /// либо значения величин по элементным группам. Устройство родного и внешнего
        /// наборов одинаково, поэтому эти строки строятся для них одним методом.
        /// </summary>
        private List<RowProperty> GetPropertySetValues(
            CompPropertyKeys valueKey,
            CompPropertyKeys fileKey,
            GeneralParameters parameters,
            PhysicalSetName fieldSet,
            PhysicalSet field,
            List<string> availableResults,
            int level)
        {
            var rows = new List<RowProperty>();

            if ((field?.Source ?? PhysicalSetSource.Values) == PhysicalSetSource.ResultFile)
            {
                rows.Add(new RowProperty(ComposeFieldKey(fileKey, fieldSet),
                    Indent(level, Properties.Resources.Header_comp_FileName),
                    ResultValue(field?.File, availableResults)));

                return rows;
            }

            var quantities = PhysicalQuantitiesToShow(fieldSet, field);

            foreach (var groupName in ElementGroupsToShow(parameters, field))
                foreach (var quantity in quantities)
                    rows.Add(new RowProperty(
                        ComposeQuantityKey(valueKey, fieldSet, quantity, groupName),
                        Indent(level, quantities.Count > 1
                            ? $"{groupName} — {PhysicalQuantityHeader(quantity)}"
                            : groupName),
                        FormatFieldComponents(field, groupName, quantity)));

            return rows;
        }

        /// <summary>
        /// Элементные группы, для которых задаются значения. Значения набора Core хранит
        /// по группам MatData, а их состав для инструкции перечислен в ActiveConditions.
        /// Группа, оставшаяся в значениях от убранного условия, тоже показывается.
        /// </summary>
        private List<string> ElementGroupsToShow(GeneralParameters parameters, PhysicalSet field)
        {
            var groups = new List<string>();

            foreach (var condition in parameters.ActiveConditions ?? Enumerable.Empty<ConditionReference>())
                if (condition.Kind == DataKind.Материал && !groups.Contains(condition.GroupName))
                    groups.Add(condition.GroupName);

            foreach (var groupName in field?.Values?.Keys ?? Enumerable.Empty<string>())
                if (!groups.Contains(groupName))
                    groups.Add(groupName);

            return groups;
        }

        /// <summary>Ячейка выбора источника набора.</summary>
        private DropDownPropertyValue SourceValue(PhysicalSet field)
        {
            var isFile = (field?.Source ?? PhysicalSetSource.Values) == PhysicalSetSource.ResultFile;
            return new DropDownPropertyValue(SourceName(isFile), SourceNames());
        }

        /// <summary>
        /// Наборы, которые задача способна получить от других физических задач.
        /// Уже сохранённые наборы также отображаются, чтобы настройки не терялись.
        /// </summary>
        private List<PhysicalSetName> InputFieldSetsToShow(GeneralParameters parameters)
        {
            var fieldSets = parameters switch
            {
                TermalParameters => new List<PhysicalSetName>
                {
                    PhysicalSetName.Chemical,
                    PhysicalSetName.Hydrodynamic
                },
                MechanicalParameters => new List<PhysicalSetName>
                {
                    PhysicalSetName.Thermal,
                    PhysicalSetName.Chemical,
                    PhysicalSetName.Hydrodynamic
                },
                ChemicalParameters => new List<PhysicalSetName>
                {
                    PhysicalSetName.Thermal
                },
                _ => new List<PhysicalSetName>()
            };

            foreach (var field in parameters.InputSets ?? Enumerable.Empty<PhysicalSet>())
                if (!fieldSets.Contains(field.Name))
                    fieldSets.Add(field.Name);

            return fieldSets;
        }

        /// <summary>Возвращает заголовок набора физических полей.</summary>
        private string PhysicalFieldSetHeader(PhysicalSetName fieldSet)
        {
            return fieldSet switch
            {
                PhysicalSetName.Thermal => Properties.Resources.Header_comp_SetThermal,
                PhysicalSetName.Mechanical => Properties.Resources.Header_comp_SetMechanical,
                PhysicalSetName.Chemical => Properties.Resources.Header_comp_SetChemical,
                PhysicalSetName.Hydrodynamic => Properties.Resources.Header_comp_SetHydrodynamic,
                _ => fieldSet.ToString()
            };
        }

        /// <summary>Возвращает заголовок физической величины.</summary>
        private string PhysicalQuantityHeader(PhysicalFieldName quantity)
        {
            return quantity switch
            {
                PhysicalFieldName.Temperature => Properties.Resources.Header_comp_FieldTemperature,
                PhysicalFieldName.Concentration => Properties.Resources.Header_comp_FieldConcentration,
                PhysicalFieldName.Velocity => Properties.Resources.Header_comp_FieldVelocity,
                PhysicalFieldName.PhaseComposition => Properties.Resources.Header_comp_FieldPhaseComposition,
                PhysicalFieldName.Displacement => Properties.Resources.Header_comp_FieldDisplacement,
                PhysicalFieldName.Pressure => Properties.Resources.Header_comp_FieldPressure,
                PhysicalFieldName.Stress => Properties.Resources.Header_comp_FieldStress,
                PhysicalFieldName.Strain => Properties.Resources.Header_comp_FieldStrain,
                _ => quantity.ToString()
            };
        }

        /// <summary>Возвращает величины, относящиеся к набору полей.</summary>
        private List<PhysicalFieldName> PhysicalQuantitiesToShow(PhysicalSetName fieldSet, PhysicalSet field)
        {
            var quantities = fieldSet switch
            {
                PhysicalSetName.Thermal => new List<PhysicalFieldName>
                {
                    PhysicalFieldName.Temperature,
                    PhysicalFieldName.PhaseComposition
                },
                PhysicalSetName.Mechanical => new List<PhysicalFieldName>
                {
                    PhysicalFieldName.Displacement,
                    PhysicalFieldName.Stress,
                    PhysicalFieldName.Strain,
                    PhysicalFieldName.Temperature,
                    PhysicalFieldName.PhaseComposition
                },
                PhysicalSetName.Chemical => new List<PhysicalFieldName>
                {
                    PhysicalFieldName.Concentration,
                    PhysicalFieldName.Temperature
                },
                PhysicalSetName.Hydrodynamic => new List<PhysicalFieldName>
                {
                    PhysicalFieldName.Velocity,
                    PhysicalFieldName.Pressure,
                    PhysicalFieldName.Temperature
                },
                _ => new List<PhysicalFieldName>()
            };

            if (field?.Values == null)
                return quantities;

            foreach (var values in field.Values.Values)
                foreach (var value in values ?? Enumerable.Empty<PhysicalField>())
                    if (!quantities.Contains(value.Name))
                        quantities.Add(value.Name);

            return quantities;
        }

        /// <summary>
        /// Имена файлов результатов предыдущих инструкций, на которые можно сослаться.
        /// </summary>
        /// <param name="current">Текущая инструкция. В список попадают только предыдущие инструкции.</param>
        /// <param name="taskKind">Если указан, в список попадают только задачи того же типа.</param>
        private List<string> GetInstructionResultNames(GeneralParameters current, TaskKind? taskKind = null)
        {
            var names = new List<string> { string.Empty };
            foreach (var instruction in preProc)
            {
                if (instruction == current)
                    break;
                if (taskKind.HasValue && instruction.TaskKind != taskKind.Value)
                    continue;
                names.Add($"{instruction.FileStem}.db");
            }
            return names;
        }

        /// <summary>
        /// Ячейка выбора результата. Сохранённая ссылка могла остаться от инструкции,
        /// которой больше нет, — тогда она добавляется в список, иначе он её потеряет.
        /// </summary>
        private DropDownPropertyValue ResultValue(string current, List<string> availableResults)
        {
            var values = new List<string>(availableResults);

            if (!string.IsNullOrEmpty(current) && !values.Contains(current))
                values.Add(current);

            return new DropDownPropertyValue(current ?? string.Empty, values);
        }

        /// <summary>Отступ строки настройки, подчинённой строке выше.</summary>
        private string Indent(int level, string header) => new string(' ', 3 * level) + header;

        /// <summary>
        /// Ключ строк-подписей. Такие строки только поясняют структуру: их не разбирает
        /// ни switch по CompPropertyKeys, ни разбор ключей физических наборов.
        /// </summary>
        private const string PhysicalSetLabelKey = "PhysicalSetLabel";

        /// <summary>Локализованные значения выпадающего списка источника данных.</summary>
        private List<string> SourceNames() => new List<string>
        {
            Properties.Resources.Header_comp_SourceConstant,
            Properties.Resources.Header_comp_SourceFile
        };

        private string SourceName(bool isFile) => isFile
            ? Properties.Resources.Header_comp_SourceFile
            : Properties.Resources.Header_comp_SourceConstant;
    }
}
