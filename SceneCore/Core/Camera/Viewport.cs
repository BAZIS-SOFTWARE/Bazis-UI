namespace SceneCore.Camera
{
    public struct Viewport
    {
        public int Width;
        public int Height;
        public double Scaling;

        public Viewport(int width, int height, double scaling = 1.0)
        {
            Width = width;
            Height = height;
            Scaling = scaling;
        }

        public float AspectRatio => Height == 0 ? 1f : (float)Width / Height;
    }
}
