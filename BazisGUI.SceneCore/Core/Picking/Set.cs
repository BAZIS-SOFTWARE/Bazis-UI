using System.Collections.Generic;
using BazisGUI.SceneCore.Coordinates;

namespace BazisGUI.SceneCore.Picking
{
    public enum ScenePickKind
    {
        /// <summary>Отдельная точка модели или узел сетки.</summary>
        Other,
        /// <summary>Упорядоченные вершины ломаной; соседние пары образуют сегменты.</summary>
        Curve,
        /// <summary>Последовательность троек вершин независимых треугольников.</summary>
        Surface
    }

    public sealed class Set
    {
        /// <summary>Создаёт набор объектов для геометрического выбора на сцене.</summary>
        public Set(string name, ScenePickKind kind, IEnumerable<ScenePickObject> objects)
        {
            Name = name;
            Kind = kind;
            Objects = objects;
        }

        public string Name { get; }
        public ScenePickKind Kind { get; }
        public IEnumerable<ScenePickObject> Objects { get; }
    }

    public sealed class ScenePickObject
    {
        /// <summary>Создаёт объект с номером и координатами примитивов в порядке, заданном видом набора.</summary>
        public ScenePickObject(int number, IEnumerable<ModelPoint3D> points)
        {
            Number = number;
            Points = points;
        }

        public int Number { get; }
        public IEnumerable<ModelPoint3D> Points { get; }
    }
}
