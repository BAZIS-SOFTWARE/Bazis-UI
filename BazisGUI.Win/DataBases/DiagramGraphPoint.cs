
using UserControlsEx.Graph;

namespace BazisGUI.Win.DataBases
{
    public class DiagramGraphPoint : GraphPoint
    {
        public DiagramGraphPoint(float x, float y, float phase) : base(x, y)
        {
            Phase = phase;            
        }

        public float Phase { get; private set; }

        public override string ToString()
        {
            var p = Phase.ToString("0.00");

            return $"{p}";
        }
    }
}
