
namespace Cell
{
    public struct CellData
    {
        public float resource;
        public float efficiency;
        public float span;
    }

    interface BaseMonoCell
    {
        protected CellData cell_data;
        protected float current_span;
        protected void Generate();
        protected void OnBirth();
        protected void OnDeath();
    }
}