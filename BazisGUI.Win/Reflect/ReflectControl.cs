using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using UserControlsEx;

namespace BazisGUI.Win.Reflect
{
    public partial class ReflectControl : UserControl
    {
        private CultureInfo culture;

        private float[] Plane { get; set; }
        private Point MouseLastPos { get; set; }

        private bool PreventRedraw { get; set; }

        private Pen Pen { get; set; }

        /// <summary>
        /// Получить или обновить матрицу
        /// </summary>
        public event Action<string, MatrixEvent> MatrixEvent;
        /// <summary>
        /// Обновляет плоскость отражения на сцене
        /// </summary>

        public event Action<string, float[]> UpdateReflectPlane;
        public event Action<string, float[]> CreateReflectObj;
        public event Action<string> ShowObjs;

        public ReflectControl()
        {
            InitializeComponent();
        }

        private void OnChangeNormal(object sender, EventArgs e)
        {
            var tb = sender as ColorSlider;
            Plane[tb.TabIndex] = tb.Value * 0.01f - 1;
            var label = tableLayoutPanel1.Controls.OfType<Label>()
                                                  .Where(c => c.TabIndex == tb.TabIndex)
                                                  .First();
            var text = label.Text.Split(' ');
            label.Text = text[0] + " " + Plane[tb.TabIndex].ToString("0.##");
            if (!PreventRedraw && comboBox1.SelectedItem != null)
            {
                UpdateReflectPlane?.Invoke(comboBox1.SelectedItem.ToString(), Plane);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && comboBox1.SelectedItem != null)
            {
                var sign = Math.Sign(e.X - MouseLastPos.X);
                var delta = float.Parse(domainUpDown1.Text, NumberStyles.Any, culture);
                Plane[3] += sign * delta;
                textBox1.Text = Plane[3].ToString("0.##");
                UpdateReflectPlane?.Invoke(comboBox1.SelectedItem.ToString(), Plane);
            }
            MouseLastPos = e.Location;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var rBtn = sender as RadioButton;
            var rect = e.ClipRectangle;
            var borderSize = rBtn.FlatAppearance.BorderSize;
            var leftColor = rBtn.Checked ? SystemColors.ControlText : SystemColors.ControlLightLight;
            var rightColor = rBtn.Checked ? SystemColors.ControlLightLight : SystemColors.ControlText;
            var leftBorder = new PointF[] { new PointF(rect.Left, rect.Bottom), new PointF(rect.Left, rect.Top),
                                            new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Top)};
            var rightBorder = new PointF[] { new PointF(rect.Left, rect.Bottom - borderSize), new PointF(rect.Right, rect.Bottom - borderSize),
                                             new PointF(rect.Right - borderSize, rect.Bottom), new PointF(rect.Right - borderSize, rect.Top)};
            Pen.Color = leftColor;
            Pen.Width = borderSize;
            e.Graphics.DrawLines(Pen, leftBorder);
            Pen.Color = rightColor;
            e.Graphics.DrawLines(Pen, rightBorder);
        }

        private void OnChoicePlane(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                var rBtn = sender as RadioButton;
                var strVec = rBtn.Tag.ToString().Split(' ');
                var vec = strVec.Select(x => float.Parse(x)).ToArray();

                var vector = Vector<float>.Build.Dense(vec);
                //var vector = TransformVector(vec);
                UpdateControlNormal(vector);
                UpdateReflectPlane?.Invoke(comboBox1.SelectedItem.ToString(), Plane);
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.CurrencyDecimalSeparator = ".";
            Pen = new Pen(SystemColors.Control);
            Plane = new float[4] { 1, 0, 0, 0 };
            domainUpDown1.SelectedItem = 2;
        }

        public void SetGlObjs(IEnumerable<string> objsName)
        {
            foreach (var item in objsName)
                if (!comboBox1.Items.Contains(item))
                    comboBox1.Items.Add(item);
        }

        public IEnumerable<string> GetAllSrcObjs()
        {
            foreach (var item in comboBox1.Items)
                yield return (string)item;
        }

        private void OnResetShifting(object sender, EventArgs e)
        {
            Plane[3] = 0;
            textBox1.Text = "0";
            if (!PreventRedraw && comboBox1.SelectedItem != null)
            {
                UpdateReflectPlane?.Invoke(comboBox1.SelectedItem.ToString(), Plane);
            }
        }

        private void OnSetCopyName(object sender, EventArgs e)
        {
            CreateReflectObj?.Invoke(comboBox1.SelectedItem.ToString(), Plane);
        }


        private void UpdateControlNormal(Vector<float> vector)
        {
            PreventRedraw = true;
            trackBar1.Value = (int)(vector[0] * 100 + 100);
            trackBar2.Value = (int)(vector[1] * 100 + 100);
            trackBar3.Value = (int)(vector[2] * 100 + 100);
            OnResetShifting(this, null);
            PreventRedraw = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCreateCopy.Enabled = true;
    
            ShowObjs?.Invoke(comboBox1.SelectedItem.ToString());
        }
    }

    public class MatrixEvent : EventArgs
    {
        public float[] Matrix { get; set; }
    }
}