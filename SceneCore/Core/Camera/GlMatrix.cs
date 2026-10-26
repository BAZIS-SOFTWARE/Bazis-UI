using System;
using MathNet.Numerics.LinearAlgebra;

namespace SceneCore.Camera
{
    /// <summary>
    /// Точечная замена GL.Translate/Rotate/Scale/LoadMatrix, оперирующая матрицей
    /// напрямую вместо стека GL_MODELVIEW (см. scene.avalonia.md, раздел "Что мешает переходу сейчас").
    /// Матрица хранится в виде matrix[row,col], как и BaseForm.ViewMatrix в старом коде.
    /// </summary>
    public static class GlMatrix
    {
        public static Matrix<float> Identity() => Matrix<float>.Build.DenseIdentity(4);

        public static Matrix<float> Translate(Matrix<float> m, double x, double y, double z)
        {
            var t = Identity();
            t[0, 3] = (float)x;
            t[1, 3] = (float)y;
            t[2, 3] = (float)z;
            return m * t;
        }

        public static Matrix<float> Scale(Matrix<float> m, float x, float y, float z)
        {
            var s = Identity();
            s[0, 0] = x;
            s[1, 1] = y;
            s[2, 2] = z;
            return m * s;
        }

        public static Matrix<float> Rotate(Matrix<float> m, float angleDeg, double x, double y, double z)
        {
            return m * RotationMatrix(angleDeg, (float)x, (float)y, (float)z);
        }

        public static Matrix<float> RotationMatrix(float angleDeg, float x, float y, float z)
        {
            var len = (float)Math.Sqrt(x * x + y * y + z * z);
            if (len < 1e-8f)
                return Identity();

            x /= len; y /= len; z /= len;

            var rad = angleDeg * Math.PI / 180.0;
            var c = (float)Math.Cos(rad);
            var s = (float)Math.Sin(rad);
            var t = 1 - c;

            var r = Identity();
            r[0, 0] = t * x * x + c;
            r[0, 1] = t * x * y - s * z;
            r[0, 2] = t * x * z + s * y;

            r[1, 0] = t * x * y + s * z;
            r[1, 1] = t * y * y + c;
            r[1, 2] = t * y * z - s * x;

            r[2, 0] = t * x * z - s * y;
            r[2, 1] = t * y * z + s * x;
            r[2, 2] = t * z * z + c;

            return r;
        }
    }
}
