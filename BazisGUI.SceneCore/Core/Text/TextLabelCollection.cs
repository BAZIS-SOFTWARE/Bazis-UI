using System;
using System.Collections;
using System.Collections.Generic;

namespace BazisGUI.SceneCore.Text
{
    public class TextLabelCollection<TLabel> : IEnumerable<TLabel> where TLabel : TextLabel
    {
        private readonly List<TLabel> labels = new List<TLabel>();

        public void Add(TLabel label) => labels.Add(label);
        public void Clear() => labels.Clear();
        public bool Remove(TLabel label) => labels.Remove(label);
        public void RemoveWhere(Func<TLabel, bool> predicate) => labels.RemoveAll(l => predicate(l));

        public IEnumerator<TLabel> GetEnumerator() => labels.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
