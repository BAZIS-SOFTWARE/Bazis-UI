using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using System;
using System.Linq;

namespace BazisGUI
{
    public partial class BaseForm
    {

        //private void ShowVolAdg(int number)
        //{
        //    var vol = project.GetModelVolumes().First(x => x.Number == number);

        //    foreach (var surfaceFigure in vol.GetSurfaceFigures())
        //        ShowAdg(surfaceFigure.Number);      
        //}

        private int[] GetAdg(int dim, int number, int dir)
        {
                if (dir == 1)
                    return GmshController.Gmsh.Model.GetAdjacencies(dim, number).Item2;
                else
                    return GmshController.Gmsh.Model.GetAdjacencies(dim, number).Item1;
        }

        private void ShowAdg(int dim, int number, int dir)
        {
            try
            {
            //var objs = project.GetModelObject(objType,number);
            //objs.ViewState = true;

            int[] adgTags;
            if (dir == 1)
                adgTags = GmshController.Gmsh.Model.GetAdjacencies(dim, number).Item2;
            else
                adgTags = GmshController.Gmsh.Model.GetAdjacencies(dim, number).Item1;

            ObjType adgType;
            if (dir == 1)
                dim--;
            else
                dim++;

            // очень странное преобразование. Врят ли тип связан с размерностью.
            adgType = (ObjType)dim;
            // Выглядит как костыль для объемов. В будущем может это как-то
            // по-другому делать

            if (dim == 3)
            {
                foreach (var item in adgTags)
                {
                    var vol = project.GetModelVolumes().FirstOrDefault(x => x.Number == item);

                    foreach (var surfaceFigure in vol.GetSurfaceFigures())
                        surfaceFigure.ViewState = true;
                }

            }
            else
                foreach (var adgTag in adgTags)
                {
                    var obj = project.GetModelObject(adgType, adgTag);
                    obj.ViewState = true;
                }

            }
            catch (Exception ex)
            {
            }
        }
    }
}
