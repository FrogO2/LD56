
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
        CellData cell_data { get; }
        float current_span { get; }
        protected void Generate();
        protected void OnBirth();
        protected void OnDeath();
    }
}