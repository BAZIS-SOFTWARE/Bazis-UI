using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Model.Interfaces.MeshObjects;
using Model.MeshObjects;
using System.Collections.Generic;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum PointPropertyKeys { Number, ElementSize }
        enum ElementPropertyKeys { Number, ElementsLevel, IncludedNodes }
        enum NodePropertyKeys { Number, CordX, CordY, CordZ, LinkedElements }

        private List<RowProperty> GetPointProperty(int number)
        {
            var meshSize = project.GetMeshPointSize(number);

            var rows = new List<RowProperty>
            {
                new RowProperty(PointPropertyKeys.Number.ToString(),
                Resources.Header_point_number,
                number),

                new RowProperty(PointPropertyKeys.ElementSize.ToString(),
                Resources.Header_point_elementsSize,
                meshSize)
                // - TO DO снять все ограничения (кнопка)
            };

            return rows;
        }

        private List<RowProperty> GetElementProperty(IElement element)
        {
            var nodes = string.Join(";", element.GetVertexes().Select(node => node.Number).ToList());
            var levels = new List<string>() { "1", "2" };
            var rows = new List<RowProperty>
            {
                new RowProperty(ElementPropertyKeys.Number.ToString(),
                Resources.Header_element_number,
                element.Number,
                true),

                new RowProperty(ElementPropertyKeys.ElementsLevel.ToString(),
                Resources.Header_element_elementLevel,
                new DropDownPropertyValue(element.Level, levels)),

                new RowProperty(ElementPropertyKeys.IncludedNodes.ToString(),
                Resources.Header_element_includedNodes,
                nodes,
                true)
            };

            return rows;
        }

        private List<RowProperty> GetNodeProperty(Node node)
        {
            var coord = node.GetCoordinates();
            var listNumbers = string.Join(";", node.GetElements().Select(element => element.Number).ToList());

            var rows = new List<RowProperty>
            {
                new RowProperty(NodePropertyKeys.Number.ToString(),
                Resources.Header_node_number,
                node.Number,
                true),

                new RowProperty(NodePropertyKeys.CordX.ToString(),
                Resources.Header_node_cordX
                , node.Position._x),

                new RowProperty(NodePropertyKeys.CordY.ToString(),
                Resources.Header_node_cordY,
                node.Position._y),

                new RowProperty(NodePropertyKeys.CordZ.ToString(),
                Resources.Header_node_cordZ,
                node.Position._z),

                new RowProperty(NodePropertyKeys.LinkedElements.ToString(),
                Resources.Header_node_linkedElements,
                listNumbers,
                true)
            };

            return rows;
        }
    }
}
