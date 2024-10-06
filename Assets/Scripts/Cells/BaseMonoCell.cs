
using JetBrains.Annotations;

namespace Cell
{
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