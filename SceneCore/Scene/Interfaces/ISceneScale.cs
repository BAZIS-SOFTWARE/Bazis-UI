using System.Collections.Generic;
using System.Drawing;

namespace SceneCore.Interfaces
{
    /// <summary>
    /// ISceneScale
    /// </summary>
    public interface ISceneScale
    {
        /// <summary>
        /// Title
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// Info
        /// </summary>
        string Info { get; set; }
        /// <summary>
        /// Display
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="items"></param>
        /// <param name="g"></param>
        /// <param name="font"></param>
        //void Display(int width, int height, SceneScaleRange[] items,Graphics g, Font font);
        /// <summary>
        /// X coord
        /// </summary>
        //int Coord_X { get; set; }
        /// <summary>
        /// Y coord
        /// </summary>
        //int Coord_Y { get; set; }
    }
}
