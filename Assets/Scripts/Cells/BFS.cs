using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace Cell
{

    public enum Direction
    {
        Up,
        RightUp,
        RightDown,
        Down,
        LeftDown,
        LeftUp,
        None
    }
    
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

        private int[] GetConnectedCells(int id, ComponentType type, Direction lastDirection)
        {

            // 获取相邻六个方向的棋子的索引
            int[] index = {
                cellManager.UpCell(id),
                cellManager.RightUpCell(id),
                cellManager.RightDownCell(id),
                cellManager.DownCell(id),
                cellManager.LeftDownCell(id),
                cellManager.LeftUpCell(id)
            };

            // 遍历每个方向的相邻棋子
            for (int i = 0; i < index.Length; i++)
            {
                if (index[i] != -1) // 如果该方向有相邻棋子
                {
                    CellView cellView = cellManager._cells[index[i]]; // 获取相邻棋子的CellView

                    // 根据方向i，判断当前棋子的边是否与相邻棋子的对应边颜色一致
                    switch (i)
                    {
                        case 0: // 上方向
                            if (!(cellView.m_cell.m_components.down == type && lastDirection != Direction.Down)) // 当前棋子的上边与相邻棋子的下边对比
                            {
                                index[i] = -1;
                            }
                            break;
                            
                        case 1: // 右上方向
                            if (!(cellView.m_cell.m_components.left_down == type && lastDirection != Direction.LeftDown)) // 当前棋子的右上边与相邻棋子的左下边对比
                            {
                                index[i] = -1;
                            }
                            break;

                        case 2: // 右下方向
                            if (!(cellView.m_cell.m_components.left_up == type && lastDirection != Direction.LeftUp)) // 当前棋子的右下边与相邻棋子的左上边对比
                            {
                                index[i] = -1;
                            }
                            break;

                        case 3: // 下方向
                            if (!(cellView.m_cell.m_components.up == type && lastDirection != Direction.Up)) // 当前棋子的下边与相邻棋子的上边对比
                            {
                                index[i] = -1;
                            }
                            break;

                        case 4: // 左下方向
                            if (!(cellView.m_cell.m_components.right_up == type && lastDirection != Direction.RightUp)) // 当前棋子的左下边与相邻棋子的右上边对比
                            {
                                index[i] = -1;
                            }
                            break;

                        case 5: // 左上方向
                            if (!(cellView.m_cell.m_components.right_down == type && lastDirection != Direction.RightDown)) // 当前棋子的左上边与相邻棋子的右下边对比
                            {
                                index[i] = -1;
                            }
                            break;
                    }
                }
            }
            return index; // 按顺序返回所有棋子，不符合的为-1
        }
            

        private Direction IsExit(int id)
        {
            int[] index = {
                cellManager.UpCell(id),
                cellManager.RightUpCell(id),
                cellManager.RightDownCell(id),
                cellManager.DownCell(id),
                cellManager.LeftDownCell(id),
                cellManager.LeftUpCell(id)
            };
            
            CellView cellView = cellManager._cells[id];
            
            var components = cellView.m_cell.m_components.allcomponents;
            
            for (int i = 0; i < index.Length; i++)
            {
                if (index[i] != -1 && components[id] == ComponentType.Exhaust)
                {
                    return (Direction)i;
                }
            }
            return Direction.None;
        }
    }
}