using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using Project.Tasks.LocalFrames;
using System.Collections.Generic;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum LocalFramePropertyKeys { Trajectory, ReferenceLine, Speed, Plane, ShiftingX, ShiftingY, ShiftingZ, RotX, RotY, RotZ }
        public List<RowProperty> GetLocalFrameProperties(LocalFrame frame, IEnumerable<IGroup> groups)
        {
            var rows = new List<RowProperty>();

            if (frame is MovedFrame mf)
            {
                rows.Add(new RowProperty(LocalFramePropertyKeys.Trajectory.ToString(),
                    Resources.Header_localFrame_trajectory,
                    new DropDownPropertyValue(mf.BaseLine?.Name ?? "*",
                    groups.
                        Where(x => x.ObjType == ObjType.Узел).
                        Select(x => x.Name)
                        .ToList())));
                rows.Add(new RowProperty(LocalFramePropertyKeys.ReferenceLine.ToString(),
                    Resources.Header_localFrame_referenceLine,
                    new DropDownPropertyValue(mf.RefLine?.Name ?? "*",
                    groups.
                        Where(x => x.ObjType == ObjType.Узел).
                        Select(x => x.Name)
                        .ToList())));
                rows.Add(new RowProperty(LocalFramePropertyKeys.Speed.ToString(),
                    Resources.Header_localFrame_speed,
                    mf.Velocity));
            }
            else
            {
                var groupsEx = groups.Select(x => x.Name).ToList();
                groupsEx.Add("*");
                var sf = frame as StaticFrame;
                rows.Add(new RowProperty(LocalFramePropertyKeys.Plane.ToString(),
                    Resources.Header_cond_plane,
                    new DropDownPropertyValue(sf.BaseGroup?.Name ?? "*", groupsEx)));
            }
            rows.Add(new RowProperty(LocalFramePropertyKeys.ShiftingX.ToString(),
                Resources.Header_cond_shiftingX,
                frame.Shifting._x));
            rows.Add(new RowProperty(LocalFramePropertyKeys.ShiftingY.ToString(),
                Resources.Header_cond_shiftingY,
                frame.Shifting._y));
            rows.Add(new RowProperty(LocalFramePropertyKeys.ShiftingZ.ToString(),
                Resources.Header_cond_ShiftingZ,
                frame.Shifting._z));
            rows.Add(new RowProperty(LocalFramePropertyKeys.RotX.ToString(),
                Resources.Header_cond_rotX,
                frame.Rotation_X));
            rows.Add(new RowProperty(LocalFramePropertyKeys.RotY.ToString(),
                Resources.Header_cond_rotY,
                frame.Rotation_Y));
            rows.Add(new RowProperty(LocalFramePropertyKeys.RotZ.ToString(),
                Resources.Header_cond_rotZ,
                frame.Rotation_Z));

            return rows;
        }
    }
}
