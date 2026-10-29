namespace BazisGUI.SceneCore.Coordinates
{
    /// <summary>Локальная система координат в координатах модели.</summary>
    public sealed class ModelFrame3D
    {
        public ModelPoint3D Centre { get; }
        public ModelVector3D DirectionX { get; }
        public ModelVector3D DirectionY { get; }
        public ModelVector3D DirectionZ { get; }

        /// <summary>Создаёт локальную систему координат по центру и направлениям осей.</summary>
        public ModelFrame3D(ModelPoint3D centre, ModelVector3D directionX, ModelVector3D directionY, ModelVector3D directionZ)
        {
            Centre = centre;
            DirectionX = directionX;
            DirectionY = directionY;
            DirectionZ = directionZ;
        }
    }
}
