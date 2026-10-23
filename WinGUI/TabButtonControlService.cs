using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinGUI
{
    public class TabButtonControlService
    {
        private Panel container;
        private Dictionary<string, Button> buttons;
        private Dictionary<string, Control> controls;

        public TabButtonControlService(Panel container) 
        { 
            this.container = container;
            buttons = new();
            controls = new();
        }

        public Button GetButton(string name) => buttons[name];
        public Control GetControl(string name) => controls[name];
        public IEnumerable<string> GetNames() => buttons.Keys;

        public void AddControl(string name, Control control)
        {
            name = name.Replace("cntr", "").Replace("btnTab", "");
            if (buttons.ContainsKey(name))
                return;

            var btn = CreateTabButton(name);

            control.Margin = new();
            control.Location = new Point(btn.Width + btn.Margin.Left + btn.Margin.Right, 0);
            control.Name = $"cntr{name}";
            control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            SetControlSize(control);

            container.Controls.Add(btn);
            container.Controls.Add(control);
            buttons[name] = btn;
            controls[name] = control;

            control.BringToFront();
        }

        public void RemoveControl(string name)
        {
            if (!buttons.ContainsKey(name))
                return;

            container.Controls.Remove(GetButton(name));
            container.Controls.Remove(GetControl(name));

            buttons.Remove(name);
            controls.Remove(name);

            RedrawButtons();
            var nav = controls.Values.FirstOrDefault();
            if (nav != null)
            {
                nav.Visible = true;
                nav.BringToFront();
            }
        }

        public Button CreateTabButton(string name)
        {
            var btn = new Button();
            if (buttons.Count > 0)
            {
                var first = buttons.First().Value;

                btn.Anchor = first.Anchor;
                btn.AutoSize = first.AutoSize;

                var g = first.CreateGraphics();
                var textSize = g.MeasureString(name, first.Font);
                var visibleControls = buttons.Values.Where(x => x.Visible);
                btn.Location = new Point(0, visibleControls.Sum(x => x.Height + x.Margin.Top));
                btn.Size = new Size((int)textSize.Height, (int)textSize.Width);

                btn.Margin = first.Margin;
                btn.FlatStyle = first.FlatStyle;
            }

            else
            {
                btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                btn.AutoSize = false;

                var g = btn.CreateGraphics();
                var textSize = g.MeasureString(name, btn.Font);
                btn.Location = new Point(0, 0);
                btn.Size = new Size((int)textSize.Height, (int)textSize.Width);

                btn.FlatStyle = default;
            }

            btn.MinimumSize = new Size(27, 130);
            btn.Tag = true;
            btn.Name = $"btnTab{name}";

            btn.MouseDown += button_MouseDown;
            btn.Paint += buttonTab_Paint;

            return btn;
        }

        private void RedrawButtons()
        {
            container.SuspendLayout();

            var visible = buttons.Values.Where(x => x.Visible).ToArray();
            for (var i = 0; i < visible.Count(); i++)
                visible[i].Location = new Point(0, i * (visible[i].Height + visible[i].Margin.Top));

            container.ResumeLayout();
        }

        private void SetControlSize(Control control)
        {
            var btn = buttons.Values.FirstOrDefault();
            var maxWidth = btn == null ? 33 : btn.Margin.Left + btn.Margin.Right + btn.Width;

            var width = container.Width - container.Padding.Right - container.Padding.Left - maxWidth;
            var height = container.Height - container.Padding.Top - container.Padding.Bottom;

            control.Size = new Size(width, height);
        }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            var btn = sender as Button;
            btn.Tag = true;

            for (int i = 0; i < container.Controls.Count; i++)
            {
                var cntr = container.Controls[i];
                if (cntr.Name.Contains("btnTab") & cntr.Visible)
                {
                    var tabPage = GetControl(cntr.Name.Replace("cntr", "").Replace("btnTab", ""));
                    SetControlSize(tabPage);

                    if (cntr.Name != btn.Name)
                    {
                        cntr.Tag = false;
                        tabPage.Visible = false;

                    }
                    else
                    {
                        tabPage.Visible = true;
                        tabPage.BringToFront();
                    }
                }
            }
        }

        private void buttonTab_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            var btn = sender as Button;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (bool.Parse(btn.Tag.ToString()))
            {
                using var pen = new Pen(Color.Black, 1.5f);
                g.DrawRectangle(pen, 1, 1, btn.Width, btn.Height);
            }

            var text = btn.Name.Replace("btnTab", "");
            var size = g.MeasureString(text, btn.Font);

            var state = g.Save();

            g.TranslateTransform(btn.Width / 2f, btn.Height / 2f);
            g.RotateTransform(-90);

            g.DrawString(text, btn.Font, Brushes.Black, -size.Width / 2f, -size.Height / 2f);
            g.Restore(state);
        }
    }
}
