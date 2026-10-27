using BazisGUI.SceneCore.VBO;
using OperationalController;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        public void SetVBObjectAttribute(IObjsPresenter presenter, string attribName)
        {
            //var objName = objsType.ToString();
            var vboObjs = VBOController.FindVBObj(presenter.Name);

            if (vboObjs != null)
            {
                if (presenter.Count() > 0)
                {
                    if (attribName == "цвет")
                    {
                        var colors = presenter.CreateVertexes(vboObjs.ColorLength, "цвет");
                        vboObjs.PointsColors = colors;
                    }
                    else
                    {
                        var coords = presenter.CreateVertexes(vboObjs.CoordLength, "координаты");
                        vboObjs.PointsCoords = coords;
                    }
                }
            }
        }
    }
}
