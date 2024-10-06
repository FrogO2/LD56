using System.Collections.Generic;
using Unity.VisualScripting;

namespace Cell
{
    public class BFS
    {
        MonoCellManager cellManager;
        private Dictionary<int, CellView> cells;
        
        private void Awake()
        {
            cellManager = MonoCellManager.Instance;
            cells = cellManager._cells;

        }

        void Start()
        {
            // cellManager.UpCell()
        }
    }
}