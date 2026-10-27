using BazisGUI.MasterInterface;
using BazisGUI.MasterInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        public void HandleGroupsMaster(IGroupHandling gh)
        {
            var dict = new Dictionary<GroupType, Dictionary<int, string>>
                {
                    { GroupType.Узел, new Dictionary<int, string>() },
                    { GroupType.Элемент1D, new Dictionary<int, string>() },
                    { GroupType.Элемент2D, new Dictionary<int, string>() },
                    { GroupType.Элемент3D, new Dictionary<int, string>() }
                };
            foreach (var group in project.GetAllModelGroups())
                dict[Converter.GetGroupTypeFromString(group.ObjType.ToString())][group.Number] = group.Name;

            gh.InitialGroupFilling(dict);
            OnGroupCreated += (type, num, name) => gh.AddGroup(Converter.GetGroupTypeFromString(type.ToString()), num, name);
            OnGroupRenamed += (type, num, name) => gh.RenameGroup(Converter.GetGroupTypeFromString(type.ToString()), num, name);
            OnGroupDeleted += (type, num) => gh.DeleteGroup(Converter.GetGroupTypeFromString(type.ToString()), num);
            navigator.DelAllMeshEvent += () => gh.DeleteAllGroups();
            navigator.DelAllGroupsEvent += () => gh.DeleteAllGroups();
            OnProjectLoaded += () =>
            {
                var tempdict = new Dictionary<GroupType, Dictionary<int, string>>
                    {
                        { GroupType.Узел, new Dictionary<int, string>() },
                        { GroupType.Элемент1D, new Dictionary<int, string>() },
                        { GroupType.Элемент2D, new Dictionary<int, string>() },
                        { GroupType.Элемент3D, new Dictionary<int, string>() }
                    };
                foreach (var group in project.GetAllModelGroups())
                    tempdict[Converter.GetGroupTypeFromString(group.ObjType.ToString())][group.Number] = group.Name;

                gh.InitialGroupFilling(tempdict);
            };
        }
    }
}
