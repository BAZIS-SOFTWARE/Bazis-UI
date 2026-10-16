namespace BazisAvaloniaGUI.Databases.MetallurgyGUI
{
    internal sealed class DiagramGraphPoint : GraphPoint
    {
        public DiagramGraphPoint(float x, float y, float phase) : base(x, y)
        {
            Phase = phase;
        }

        public float Phase { get; }

        public override string ToString()
        {
            return Phase.ToString("0.00");
        }
    }
}
