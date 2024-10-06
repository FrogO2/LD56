
using JetBrains.Annotations;

namespace Cell
{
    public enum ComponentType
    {
        Devour,
        Produce,
        Exhaust,
        None
    }
    public struct Component
    {
        public ComponentType type;
        public int id;
    }
    public class CellData
    {
        public float resource;
        public float efficiency;
        public float span;
    }

    public interface BaseMonoCell
    {
        int id {  get; }
        CellData cell_data { get; }
        void Birth();
        void Death();
    }
}