using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;

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
        
        public Direction GetRelativeDirection(int id1, int id2)
        {
            // 获取 id1 相邻六个方向的 ID
            int leftUpId = cellManager.LeftUpCell(id1);
            int rightUpId = cellManager.RightUpCell(id1);
            int leftDownId = cellManager.LeftDownCell(id1);
            int rightDownId = cellManager.RightDownCell(id1);
            int upId = cellManager.UpCell(id1);
            int downId = cellManager.DownCell(id1);

            // 判断 id2 是否与 id1 相邻
            if (id2 == leftUpId) return Direction.LeftUp;
            if (id2 == rightUpId) return Direction.RightUp;
            if (id2 == leftDownId) return Direction.LeftDown;
            if (id2 == rightDownId) return Direction.RightDown;
            if (id2 == upId) return Direction.Up;
            if (id2 == downId) return Direction.Down;

            // 如果 id2 不在 id1 的相邻方向，返回 None
            return Direction.None;
        }
        
        /// <summary>
        /// 使用BFS搜索相连的管道
        /// </summary>
        /// <param name="startId">起始棋子的ID</param>
        /// <param name="type">组件类型</param>
        /// <param name="direction">初始方向</param>
        /// <returns>相连管道的ID列表</returns>
        public List<int> GetConnectedPipes(int startId, ComponentType type, Direction direction)
        {
            Queue<(int, Direction)> queue = new Queue<(int, Direction)>(); // BFS队列，包含当前cell的ID和它的方向
            HashSet<int> visited = new HashSet<int>(); // 已访问的cell
            List<int> connectedCells = new List<int>(); // 存储连通的cell

            // 初始起点入队，方向为Direction.None
            queue.Enqueue((startId, direction));
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

        private List<int> GetConnectedProduce(int id, Direction lastDirection)
        {
            List<int> connected = new List<int>();
            ComponentType type = ComponentType.Produce;
            var components = cellManager._cells[id].m_cell.m_components.allcomponents;

            if (components[((int)lastDirection + 2) % 6]==type)
            {
                connected.Add(((int)lastDirection + 2) % 6);
            }
            
            if (components[((int)lastDirection + 4) % 6]==type)
            {
                connected.Add(((int)lastDirection + 4) % 6);
            }

            return connected;

        }

        public List<List<int>> GetAll()
        {
            // List<int> availableIds = new List<int>();
            // 找到所有可用的入口
            List<int[]> availableEntrances = new List<int[]>();
            for (int i = 0; i < cellManager.CheckMap.Length; i++)
            {
                if (cellManager.CheckMap[i])
                {
                    var entry = IsEntrance(i);
                    if (entry != Direction.None)
                    {
                        int[] j = { i, (int)entry };
                        availableEntrances.Add(j);
                    }
                }
            }


            var devours = new List<int[]>();
            var producePipes = new List<List<int>>();
            var exhaustPipes = new List<List<int>>();
            
            // 找到每个入口相连的能量细胞器
            foreach (var entrance in availableEntrances)
            {
                var produces = GetConnectedProduce(entrance[0], (Direction)entrance[1]);
                var tempDevours = new List<int[]>();
                var tempProducePipes = new List<List<int>>();

                foreach (var produce in produces)
                {
                    var producePipe = GetConnectedPipes(entrance[0], ComponentType.Produce, (Direction)produce);
                    Direction lastDirection = (Direction)produce;
                    
                    var exhausts = new List<int[]>();
                    // 找到能量相连的排泄
                    for (int i = 0; i < producePipe.Count - 1; i++)
                    {
                        var ex = GetConnectedDevour(producePipe[i], lastDirection);

                        for (int j = 0; j < ex.Length; j++)
                        {
                            if (ex[i] != -1)
                            {
                                exhausts.Add(new int[] {ex[j], j});
                            }
                        }
                        
                        lastDirection = GetRelativeDirection(producePipe[i], producePipe[i + 1]);
                    }

                    foreach (var exhaust in exhausts)
                    {
                        GetConnectedPipes(exhaust[0], ComponentType.Exhaust, (Direction)exhaust[1]);
                    }
                    tempDevours.Add(entrance);
                    //tempProducePipes.Add();
                }
                
                
            }

            return new List<List<int>>();
        }

        private int[] GetConnectedDevour(int id, Direction direction)
        {
            int[] connected = {-1, -1, -1, -1, -1, -1};
            int[] surroundingCells = {
                cellManager.UpCell(id),
                cellManager.RightUpCell(id),
                cellManager.RightDownCell(id),
                cellManager.DownCell(id),
                cellManager.LeftDownCell(id),
                cellManager.LeftUpCell(id)
            };
            
            var connectedComponents = GetConnectedComponents(id, ComponentType.Produce, direction);
            for (int i = 0; i < connectedComponents.Count; i++)
            {
                if (ComponentType.Produce == cellManager._cells[surroundingCells[i]].m_cell.m_components.allcomponents[(i + 3) % 6])
                {
                    connected[i] = surroundingCells[i];
                }
            }

            return connected;
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
                if (index[i] == -1 && components[i] == ComponentType.Exhaust)
                {
                    return (Direction)i;
                }
            }
            return Direction.None;
        }
        
        private Direction IsExit(int id, Direction direction)
        {
            int[] index = {
                cellManager.UpCell(id),
                cellManager.RightUpCell(id),
                cellManager.RightDownCell(id),
                cellManager.DownCell(id),
                cellManager.LeftDownCell(id),
                cellManager.LeftUpCell(id)
            };
            
            var components = GetConnectedComponents(id, ComponentType.Exhaust, direction);
            
            for (int i = 0; i < index.Length; i++)
            {
                if (index[i] == -1 && components.Contains(i))
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
        private Direction IsEntrance(int id)
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
                if (index[i] == -1 && components[i] == ComponentType.Devour)
                {
                    return (Direction)i;
                }
            }
            return Direction.None;
        }
    }
}