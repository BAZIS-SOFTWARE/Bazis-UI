using BazisGUI.Win.Properties;
using BazisGUI.Win.PropertiesPanel;
using BazisGUI.Win.Utilities;
using Model.Interfaces.ObjectsCollections;
using OperationalController;
using System.Collections.Generic;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        public List<RowProperty> GetSetProperty(ISetInfo _objectsSet)
        {
            return new List<RowProperty>
            {
               new RowProperty(SetPropertyKeys.Name.ToString(), Resources.Header_set_name, _objectsSet.Name, isReadOnly: true),

               new RowProperty(SetPropertyKeys.Color.ToString(), Resources.Header_set_color, project.GetColor(_objectsSet)),

               new RowProperty(SetPropertyKeys.View.ToString(), Resources.Header_set_view,
               new DropDownPropertyValue(project.GetViewMode(_objectsSet), Converters.GetEnumNames<ViewMode>()))
            };
        }
    }
}
