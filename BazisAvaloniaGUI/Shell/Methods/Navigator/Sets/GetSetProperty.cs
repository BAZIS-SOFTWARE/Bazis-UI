using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using BazisAvaloniaGUI.Utilities;
using Model.Interfaces.ObjectsCollections;
using OperationalController;
using System.Collections.Generic;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        public List<RowProperty> GetSetProperty(ISetInfo _objectsSet)
        {
            var modelView = project;
            return new List<RowProperty>
            {
               new RowProperty(SetPropertyKeys.Name.ToString(), Resources.Header_set_name, _objectsSet.Name, isReadOnly: true),

               new RowProperty(SetPropertyKeys.Color.ToString(), Resources.Header_set_color, modelView.GetColor(_objectsSet)),

               new RowProperty(SetPropertyKeys.View.ToString(), Resources.Header_set_view,
               new DropDownPropertyValue(modelView.GetViewMode(_objectsSet), Converters.GetEnumNames<ViewMode>()))
            };
        }
    }
}
