using System;
using System.Collections;
using System.Collections.Generic;

namespace BazisGUI.SceneCore.Text
{
    public class TextLabelCollection : IEnumerable<TextLabel>
    {
        private readonly List<TextLabel> labels = new List<TextLabel>();

        public void Add(TextLabel label) => labels.Add(label);
        public void Clear() => labels.Clear();
        public bool Remove(TextLabel label) => labels.Remove(label);
        public void RemoveWhere(Func<TextLabel, bool> predicate) => labels.RemoveAll(l => predicate(l));

        public IEnumerator<TextLabel> GetEnumerator() => labels.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
