using System.Collections.Generic;
using System.Linq;

namespace SceneCore.Layers
{
    public class SceneLayerCollection
    {
        private readonly List<ISceneLayer> layers = new List<ISceneLayer>();

        public void Add(ISceneLayer layer) => layers.Add(layer);

        public bool Remove(string name)
        {
            var layer = layers.FirstOrDefault(l => l.Name == name);
            return layer != null && layers.Remove(layer);
        }

        public ISceneLayer Find(string name) => layers.FirstOrDefault(l => l.Name == name);

        public IEnumerable<ISceneLayer> GetOrdered() => layers.OrderBy(l => l.Order);
    }
}
