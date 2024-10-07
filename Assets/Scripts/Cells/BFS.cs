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
        None=-1
    }
    
    public class BFS
    {
        MonoCellManager cellManager;
        
        public BFS()
        {
            cellManager = MonoCellManager.Instance;
        }
        
        /// <summary>
        /// 使用BFS搜索相连的管道
        /// </summary>
        /// <param name="startId">起始棋子的ID</param>
        /// <param name="type">组件类型</param>
        /// <returns>相连管道的ID列表</returns>
        public List<int> GetConnectedPipes(int startId, ComponentType type)
        {
            Queue<(int, Direction)> queue = new Queue<(int, Direction)>(); // BFS队列，包含当前cell的ID和它的方向
            HashSet<int> visited = new HashSet<int>(); // 已访问的cell
            List<int> connectedCells = new List<int>(); // 存储连通的cell

            // 初始起点入队，方向为Direction.None
            queue.Enqueue((startId, Direction.None));
            visited.Add(startId);
            connectedCells.Add(startId);

            // 开始BFS
            while (queue.Count > 0)
            {
                var (currentId, lastDirection) = queue.Dequeue(); // 取出当前节点

                // 获取与当前节点相连的节点
                int[] connectedIds = GetConnectedCells(currentId, type, lastDirection);

                // 遍历所有相连的节点
                for (int i = 0; i < connectedIds.Length; i++)
                {
                    int neighborId = connectedIds[i];

                    // 如果相连且未访问过
                    if (neighborId != -1 && !visited.Contains(neighborId))
                    {
                        queue.Enqueue((neighborId, (Direction)i)); // 将相邻节点入队，并记录方向
                        visited.Add(neighborId); // 标记为已访问
                        connectedCells.Add(neighborId); // 添加到结果列表
                    }
                }
            }

            return connectedCells;
        }


        /// <summary>
        /// 获取指定细胞器类型在指定方向上的相邻细胞的ID。
        /// </summary>
        /// <param name="id">要检查的细胞的ID。</param>
        /// <param name="type">要检查的细胞器类型。</param>
        /// <param name="lastDirection">上一个相邻细胞的方向。</param>
        /// <returns>按顺序返回所有符合细胞器类型连接的相邻细胞ID，不符合的为-1。</returns>
        private int[] GetConnectedCells(int id, ComponentType type, Direction lastDirection)
        {
            // 获取相邻六个方向的棋子的索引
            int[] surroundingCells = {
                cellManager.UpCell(id),
                cellManager.RightUpCell(id),
                cellManager.RightDownCell(id),
                cellManager.DownCell(id),
                cellManager.LeftDownCell(id),
                cellManager.LeftUpCell(id)
            };
            
            int[] index = {-1, -1, -1, -1, -1, -1};

            foreach (int i in GetConnectedComponents(id, type, lastDirection))
            {
                if (type == cellManager._cells[surroundingCells[i]].m_cell.m_components.allcomponents[(i + 3) % 6])
                {
                    index[i] = surroundingCells[i];
                }
            }

            return index;
        }
        
        /// <summary>
        /// 获取指定细胞器类型在指定方向上的连续情况。
        /// </summary>
        /// <param name="id">要检查的细胞的ID。</param>
        /// <param name="type">要检查的细胞器类型。</param>
        /// <param name="lastDirection">上一个相邻细胞的方向。</param>
        /// <returns>返回一个列表，包含所有从相邻细胞进入当前细胞的连续细胞器下标。</returns>
        private List<int> GetConnectedComponents(int id, ComponentType type, Direction lastDirection)
        {
            List<int> index = new List<int>();

            var organelles = cellManager._cells[id].m_cell.m_components.allcomponents;

            int from = ((int)lastDirection + 3) % 6;

            int thres = 6;
            // 遍历顺时针的相邻细胞器
            for (int i = from; i < from + 6; i++)
            {
                if (type == organelles[i % 6])
                {
                    thres--;
                    index.Add(i % 6);
                }
                else
                {
                    break;
                }
            }
            // 逆时针
            for (int i = from; i > from - thres; i--)
            {
                if (type == organelles[i % 6])
                {
                    index.Add(i % 6);
                }
                else
                {
                    break;
                }
            }
            return index;
        }
        
        
        /// <summary>
        /// Checks if the specified cell has an Exhaust Component with no obstacles and returns the direction of the exit.
        /// </summary>
        /// <param name="id">The ID of the cell to check.</param>
        /// <returns>The direction of the exit if found, otherwise Direction.None.</returns>
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
        
        /// <summary>
        /// Checks if the specified cell has an Devour Component with no obstacles and returns the direction of the entry.
        /// </summary>
        /// <param name="id">The ID of the cell to check.</param>
        /// <returns>The direction of the entry if found, otherwise Direction.None.</returns>
        private Direction IsEntry(int id)
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
                if (index[i] != -1 && components[id] == ComponentType.Devour)
                {
                    return (Direction)i;
                }
            }
            return Direction.None;
        }
    }
}