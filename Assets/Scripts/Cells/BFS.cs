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
            var ids = cells.Keys;
            // cellManager.UpCell()
        }

        private List<int> GetConnectedCells(int id, ComponentType type)
        {
            List<int> ids = new List<int>();

            int[] index = {cellManager.UpCell(id), cellManager.RightUpCell(id), cellManager.RightDownCell(id), cellManager.DownCell(id), cellManager.LeftDownCell(id), cellManager.LeftUpCell(id)};

            for(int i = 0; i < index.Length; i++)
            {
                if (index[i] != -1)
                {
                    ids.Add(index[i]);
                }
            }
            
            return ids;
        }
        

        private bool IsExit(int id)
        {
            // cellManager._cells[id]
            return false;
        }
    }
}