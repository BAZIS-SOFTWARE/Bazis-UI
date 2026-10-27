
using System.Numerics;

namespace BazisGUI.Win.CrossSection
{
    public class CreatePlaneFromTextArgs
    {
        public Vector3 point1 { get; }
        public Vector3 point2 { get; }
        public Vector3 point3 { get; }

        public CreatePlaneFromTextArgs(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.point3 = point3;
        }
    }
}
