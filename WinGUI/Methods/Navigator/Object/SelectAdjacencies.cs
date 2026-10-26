using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BazisGUI
{
    public partial class BaseForm
    {
        private List<(ISetInfo, List<int> Numbers)> SelectAdj(int dim, int number)
        {
            var (upperLevel, lowerLevel) = GmshController.Gmsh.Model.GetAdjacencies(dim, number);
            var dimUp = dim + 1;
            var dimLow = dim - 1;
            List<(ISetInfo, List<int> Numbers)> sets = new List<(ISetInfo, List<int> Numbers)>();

            var up = GetFilteredSet(dimUp, upperLevel);
            if (up.Set != null)
                sets.Add((up.Set, up.Numbers));

            var low = GetFilteredSet(dimLow, lowerLevel);
            if (low.Set != null)
                sets.Add((low.Set, low.Numbers));

            return sets;
        }


        private (ISetInfo Set, List<int> Numbers) GetFilteredSet(int dim, int[] adjNumbers)
        {
            var setInfo = project.GetModelSetInfo((ObjType)dim, ((ObjType)dim).ToString());
            if (setInfo == null) return (null, new List<int>());

            var adjSet = new HashSet<int>(adjNumbers);
            var numbers = setInfo.GetNumbers().Where(n => adjSet.Contains(n)).ToList();

            return (setInfo, numbers);
        }
    }
}
