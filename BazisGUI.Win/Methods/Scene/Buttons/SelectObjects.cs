using BazisGUI.Win.Properties;
using BazisGUI.Win.Utilities;
//using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public enum SelectionType { Select, Objects, Figures, Points, Curves, Surfaces, Volumes, Nodes, Elements, Elements1D, Elements2D, Elements3D }
    public partial class BaseForm
    {
        
        Dictionary<SelectionType, Button> objButtons = new();
        public event Action<SelectionType> OnChangeSelectedObjectsEvent;
        /// <summary>
        /// Временный выбранный объект для работы со свойствами через сцену
        /// </summary>
        //public  SelectedType;
        public SelectionType SelectedObjects
        {
            get { return Enum.Parse<SelectionType>(btnSelect.AccessibleName.Split("SelectObjects.btnSelect.")[1]); }
            set 
            {
                btnSelect.AccessibleName = $"SelectObjects.btnSelect.{value.ToString()}";
                btnSelect.Text = Localization.Localization.GetSelectionTypeLocalization(value);
                SetBackColorToAllObjects();
            }
        }

        public void AddObjectsType(SelectionType select)
        {
            var btn = CreateButton(select);

            btn.MouseDown += Btn_MouseDown;

            btn.Visible = false;
            if(objButtons.Count != 0)
            {
                var last = objButtons.Last().Value;
                btn.Location = 
                    new Point(
                        last.Location.X, 
                        last.Location.Y + last.Height - 1);
            }   
            else
            {
                btn.Location =
                new Point(
                    btnSelect.Location.X,
                    btnSelect.Location.Y + btnSelect.Height - 2);
            }
            splitContainer2.Panel1.Controls.Add(btn);
            btn.BringToFront();

            objButtons.Add(select, btn);
        }

        private void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            var btn = sender as Button;
            SelectedObjects = Enum.Parse<SelectionType>(btn.AccessibleName.Split("SelectObjects.btnSelect.")[1]);
            OnChangeSelectedObjectsEvent?.Invoke(SelectedObjects);
            btnSelect.Tag = false;

            foreach (var item in objButtons)
                item.Value.Visible = false;
        }

        private void btnSelect_Leave(object sender, EventArgs e)
        {
            btnSelect.Tag = false;
            btnSelect.Image = Resources.arrow_r;
            foreach (var item in objButtons)
                item.Value.Visible = false;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var flag = bool.Parse(btnSelect.Tag.ToString());
            if (!flag)
            {
                flag = true;
                btnSelect.Image = Resources.arrow_d;
            }
            else
            {
                flag = false;
                btnSelect.Image = Resources.arrow_r;
            }

            btnSelect.Tag = flag;
            foreach (var item in objButtons)
                item.Value.Visible = flag;

        }

        public void PresentModelObjectsForSelection()
        {

            foreach (var item in objButtons)
                splitContainer2.Panel1.Controls.Remove(item.Value);

            objButtons.Clear();

            var objTypes = project.GetAllModelObjects().Select(x => x.ObjType).Distinct();

            if (objTypes.Count() != 0)
            {
                AddObjectsType(SelectionType.Objects);

                foreach (var item in objTypes)
                    AddObjectsType(Converters.ConvertObjTypeToSelectionType(item));
            }
        }

        public Button CreateButton(SelectionType select)
        {
            string localization;

            switch (select)
            {
                case SelectionType.Objects:
                    localization = Resources.btnSelect_Text_Objects;
                    break;
                case SelectionType.Points:
                    localization = Resources.btnSelect_Text_Points;
                    break;
                case SelectionType.Curves:
                    localization = Resources.btnSelect_Text_Curves;
                    break;
                case SelectionType.Surfaces:
                    localization = Resources.btnSelect_Text_Surfaces;
                    break;
                case SelectionType.Nodes:
                    localization = Resources.btnSelect_Text_Nodes;
                    break;
                case SelectionType.Elements1D:
                    localization = Resources.btnSelect_Text_Elements1D;
                    break;
                case SelectionType.Elements2D:
                    localization = Resources.btnSelect_Text_Elements2D;
                    break;
                case SelectionType.Elements3D:
                    localization = Resources.btnSelect_Text_Elements3D;
                    break;
                default:
                    throw new ArgumentException($"{Resources.ConvertFailCaption}: {select.ToString()} -> string");
            }

            var btn = new Button();
            btn.Anchor = btnSelect.Anchor;
            btn.AutoSize = btnSelect.AutoSize;
            btn.Name = select.ToString();
            btn.AccessibleName = $"SelectObjects.btnSelect.{select.ToString()}";
            btn.Size = btnSelect.Size;
            btn.Text = localization;
            btn.AutoSize = btnSelect.AutoSize;
            btn.FlatStyle = btnSelect.FlatStyle;
            btn.Margin = btnSelect.Margin;

            return btn;
        }
    }
}
