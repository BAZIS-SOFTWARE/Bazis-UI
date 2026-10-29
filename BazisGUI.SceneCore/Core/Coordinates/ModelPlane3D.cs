using System;

namespace BazisGUI.SceneCore.Coordinates
{
    /// <summary>Плоскость в координатах модели: Normal · point + D = 0.</summary>
    public sealed class ModelPlane3D
    {
        public ModelVector3D Normal { get; }
        public double D { get; }

        /// <summary>Создаёт плоскость по нормали и свободному члену уравнения.</summary>
        public ModelPlane3D(ModelVector3D normal, double d)
        {
            var length = Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);
            if (length < 1e-12)
                throw new ArgumentException("Plane normal magnitude must be at least 1e-12.", nameof(normal));

            Normal = normal;
            D = d;
        }
    }
}
