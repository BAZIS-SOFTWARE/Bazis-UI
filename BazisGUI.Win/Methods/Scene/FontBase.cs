using BazisGUI.SceneCore.Interfaces;
using System;
using Geometry;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        //идентификатор нужен для корректного отображения шрифтов
        public int FontBase { get; set; }
    }
}
