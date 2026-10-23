using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WinGUI.Methods.PlatformSpecific
{
    static class PlatformSpecific
    {
        #region opengl32
        [DllImport("opengl32.dll", CharSet = CharSet.Auto, EntryPoint = "wglUseFontBitmapsW")]
        [SuppressUnmanagedCodeSecurity]
        internal static extern bool UseFontBitmapsW(IntPtr hDC, int first, int count, int listBase);

        [DllImport("opengl32.dll", EntryPoint = "wglGetCurrentDC", ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr GetCurrentDC();
        #endregion

        #region gdi32

        [DllImport("gdi32.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        internal static extern bool DeleteObject(IntPtr objectHandle);

        [DllImport("gdi32.dll")]
        [SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr SelectObject(IntPtr deviceContext, IntPtr objectHandle);
        #endregion

        #region glu32
        [DllImport("glu32.dll")]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr gluNewQuadric();

        [DllImport("glu32.dll")]
        [SuppressUnmanagedCodeSecurity]
        public static extern void gluDeleteQuadric([In] IntPtr quad);

        [DllImport("glu32.dll")]
        [SuppressUnmanagedCodeSecurity]
        public static extern void gluCylinder([In] IntPtr quad, double baseRadius, double topRadius, double height, int slices, int stacks);

        [DllImport("glu32.dll")]
        [SuppressUnmanagedCodeSecurity]
        public static extern void gluSphere([In] IntPtr quad, double radius, int slices, int stacks);

        [DllImport("glu32.dll")]
        [SuppressUnmanagedCodeSecurity]
        public static extern void gluPerspective(double fovY, double aspectRatio, double zNear, double zFar);
        #endregion
    }
}
