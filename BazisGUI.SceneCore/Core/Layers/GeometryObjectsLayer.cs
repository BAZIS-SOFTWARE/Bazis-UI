using System;
using System.Collections.Generic;
using System.Linq;
using BazisGUI.SceneCore.Rendering;
using BazisGUI.SceneCore.VBO;

namespace BazisGUI.SceneCore.Layers
{
    /// <summary>
    /// Заменяет DisplayGeometryObjectEvent. В старом коде рисование добавлялось и снималось
    /// подпиской на делегат, а скрытие искало обработчик по имени метода
    /// (BaseForm.HideGeometryObj/FindGeometryObj, GetInvocationList().Contains(searchMethod)).
    /// Здесь то же самое — явный ключ вместо поиска по имени метода делегата.
    /// </summary>
    /// <remarks>
    /// Объект может владеть GL-буферами (меши конуса/сферы/маркера): они освобождаются при снятии
    /// объекта (Remove/RemoveWhere/Clear). Все эти методы вызываются только при текущем GL-контексте.
    /// </remarks>
    public class GeometryObjectsLayer : ISceneLayer
    {
        public string Name => "GeometryObjects";
        public int Order { get; set; } = 20;
        public bool IsVisible { get; set; } = true;

        private readonly Dictionary<string, Action<IRenderContext>> items = new Dictionary<string, Action<IRenderContext>>();
        private readonly Dictionary<string, VBObject[]> ownedBuffers = new Dictionary<string, VBObject[]>();

        /// <summary>
        /// Добавляет объект рисования. <paramref name="buffers"/> — GL-буферы, которыми владеет объект:
        /// они удаляются вместе с ним.
        /// </summary>
        public void Add(string key, Action<IRenderContext> draw, params VBObject[] buffers)
        {
            ReleaseBuffers(key);
            items[key] = draw;
            if (buffers != null && buffers.Length > 0)
                ownedBuffers[key] = buffers;
        }

        public bool Remove(string key)
        {
            ReleaseBuffers(key);
            return items.Remove(key);
        }

        public bool Contains(string key) => items.ContainsKey(key);

        public void RemoveWhere(Func<string, bool> predicate)
        {
            foreach (var key in items.Keys.Where(predicate).ToList())
                Remove(key);
        }

        public bool Any(Func<string, bool> predicate) => items.Keys.Any(predicate);

        public void Clear()
        {
            foreach (var key in ownedBuffers.Keys.ToList())
                ReleaseBuffers(key);
            items.Clear();
        }

        public void Draw(IRenderContext context)
        {
            foreach (var draw in items.Values)
                draw(context);
        }

        /// <summary>Удаляет GL-буферы, принадлежащие объекту с ключом <paramref name="key"/>.</summary>
        private void ReleaseBuffers(string key)
        {
            if (!ownedBuffers.TryGetValue(key, out var buffers))
                return;

            foreach (var buffer in buffers)
                BazisGUI.SceneCore.VBO.VBO.DeleteAllBuffers(buffer);
            ownedBuffers.Remove(key);
        }
    }
}
