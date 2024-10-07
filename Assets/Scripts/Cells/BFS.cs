using System;
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

    public struct Organelle
    {
        public ComponentType type;
        public Direction direction;
        public int id;
    }
    
    
    
    public class BFS
    {
        MonoCellManager cellManager;

        public Organelle NewOrganelle(ComponentType type, Direction direction, int id)
        {
            var o = new Organelle();
            o.type = type;
            o.direction = direction;
            o.id = id;
            return o;
        }
        
        public BFS()
        {
            cellManager = MonoCellManager.Instance;
        }
        

        
        /// <summary>
        /// 使用BFS搜索相连的管道
        /// </summary>
        /// <param name="startId">起始棋子的ID</param>
        /// <param name="type">组件类型</param>
        /// <param name="direction">初始方向</param>
        /// <returns>相连管道的ID列表</returns>
        public Tuple<List<int>, List<Organelle>> GetConnectedPipes(int startId, ComponentType type, Direction direction)
        {
            Queue<(int, Direction)> queue = new Queue<(int, Direction)>(); // BFS队列，包含当前cell的ID和它的方向
            HashSet<int> visited = new HashSet<int>(); // 已访问的cell
            List<int> connectedCells = new List<int>(); // 存储连通的cell
            List<Organelle> connectedOrganelles = GetConnectedComponents(startId, type, direction); // 存储连通的organelles
            
            queue.Enqueue((startId, direction));
            visited.Add(startId);
            connectedCells.Add(startId);

            
            // 开始BFS
            while (queue.Count > 0)
            {
                var (currentId, lastDirection) = queue.Dequeue(); // 取出当前节点

                // 获取与当前节点相连的节点
                var connectedIds = GetConnectedCells(currentId, type, lastDirection);

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
                        foreach (var organelle in GetConnectedComponents(neighborId, type, (Direction)i))
                        {
                            connectedOrganelles.Add(organelle);
                        }
                    }
                }
            }

            return new Tuple<List<int>, List<Organelle>>(connectedCells, connectedOrganelles);
        }

        private List<Organelle> GetConnectedProduce(Organelle organelle)
        {
            List<Organelle> connected = new List<Organelle>();
            var components = cellManager._cells[organelle.id].m_cell.m_components.allcomponents;

            if (components[((int)organelle.direction + 1) % 6]==ComponentType.Produce)
            {
                connected.Add(NewOrganelle(ComponentType.Produce, (Direction)(((int)organelle.direction + 1) % 6), organelle.id));
            }
            
            if (components[((int)organelle.direction - 1) % 6]==ComponentType.Produce)
            {
                connected.Add(NewOrganelle(ComponentType.Produce, (Direction)(((int)organelle.direction - 1) % 6), organelle.id));
            }

            return connected;

        }

        public Tuple<List<Organelle>, List<List<int>>, List<List<Organelle>>, List<List<int>>, List<List<Organelle>>> GetAll()
        {
            // List<int> availableIds = new List<int>();
            // 找到所有可用的入口
            List<Organelle> availableEntrances = new List<Organelle>();
            for (int i = 0; i < cellManager.CheckMap.Length; i++)
            {
                if (cellManager.CheckMap[i])
                {
                    var entrances = IsEntrance(i);
                    foreach (var entrance in entrances)
                    {
                        availableEntrances.Add(entrance);
                    }
                }
            }
            
            var devours = new List<Organelle>();
            var producePipes = new List<List<int>>();
            var exhaustPipes = new List<List<int>>();
            var producePipesOrganelles = new List<List<Organelle>>();
            var exhaustPipesOrganelles = new List<List<Organelle>>();
            
            // 找到每个入口相连的能量细胞器
            foreach (var entrance in availableEntrances)
            {
                var produces = GetConnectedProduce(entrance);

                foreach (var produce in produces)
                {
                    (var producePipe, var produceOrganelles) = GetConnectedPipes(produce.id, ComponentType.Produce, produce.direction);
                    
                    var exhausts = new HashSet<Organelle>();
                    // 找到能量相连的排泄
                    
                    foreach (var produceOrganelle in produceOrganelles)
                    {
                        foreach (var exhaust in GetConnectedExhaust(produceOrganelle))
                        {
                            exhausts.Add(exhaust);
                        }
                    }

                    foreach (var exhaust in exhausts)
                    {
                        (var exhaustPipe, var exhaustOrganelles) = GetConnectedPipes(exhaust.id, ComponentType.Exhaust, exhaust.direction);

                        var flag = false;
                        foreach (var ex in exhaustOrganelles)
                        {
                            if (IsExit(ex))
                            {
                                flag = true;
                                break; 
                            }
                        }

                        if (flag)
                        {
                            devours.Add(entrance);
                            producePipes.Add(producePipe);
                            producePipesOrganelles.Add(produceOrganelles);
                            exhaustPipes.Add(exhaustPipe);
                            exhaustPipesOrganelles.Add(exhaustOrganelles);
                        }
                    }
                    tempDevours.Add(entrance);
                    //tempProducePipes.Add();
                }
                
                
            }

            return new Tuple<List<Organelle>, List<List<int>>, List<List<Organelle>>, List<List<int>>, List<List<Organelle>>>(devours, producePipes, producePipesOrganelles, exhaustPipes, exhaustPipesOrganelles);
        }

        // 获取对面或身边的排泄模块[ID, 方向]
        private List<Organelle> GetConnectedExhaust(Organelle organelle)
        {
            List<Organelle> connected = new List<Organelle>();
            int[] surroundingCells = {
                cellManager.UpCell(organelle.id),
                cellManager.RightUpCell(organelle.id),
                cellManager.RightDownCell(organelle.id),
                cellManager.DownCell(organelle.id),
                cellManager.LeftDownCell(organelle.id),
                cellManager.LeftUpCell(organelle.id)
            };

            if (ComponentType.Exhaust == cellManager._cells[surroundingCells[(int)organelle.direction]].m_cell.m_components.allcomponents[((int)organelle.direction + 3) % 6])
            {
                connected.Add(NewOrganelle(ComponentType.Exhaust, (Direction)(((int)organelle.direction + 3) % 6), surroundingCells[(int)organelle.direction]));
            }
            
            if (cellManager._cells[organelle.id].m_cell.m_components.allcomponents[((int)organelle.direction + 1) % 6]==ComponentType.Exhaust)
            {
                connected.Add(NewOrganelle(ComponentType.Exhaust, (Direction)(((int)organelle.direction + 1) % 6), organelle.id));
            }
        
            if (cellManager._cells[organelle.id].m_cell.m_components.allcomponents[((int)organelle.direction - 1) % 6]==ComponentType.Exhaust)
            {
                connected.Add(NewOrganelle(ComponentType.Exhaust, (Direction)(((int)organelle.direction - 1) % 6), organelle.id));
            }

            return connected;
        }
        


        /// <summary>
        /// 获取指定细胞器类型在指定方向上的相邻细胞的ID。
        /// </summary>
        /// <param name="id">要检查的细胞的ID。</param>
        /// <param name="type">要检查的细胞器类型。</param>
        /// <param name="lastDirection">上一个相邻细胞的方向。</param>
        /// <returns>按顺序返回所有符合细胞器类型连接的相邻细胞ID，不符合的为-1，同时返回所有相连的细胞器。</returns>
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
            
            List<Organelle> organelles = GetConnectedComponents(id, type, lastDirection);

            foreach (Organelle i in organelles)
            {
                if (type == cellManager._cells[surroundingCells[(int)i.direction]].m_cell.m_components.allcomponents[((int)i.direction + 3) % 6])
                {
                    index[(int)i.direction] = surroundingCells[(int)i.direction];

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
        /// <returns>返回一个列表，包含所有从相邻细胞进入当前细胞的连续细胞器。</returns>
        private List<Organelle> GetConnectedComponents(int id, ComponentType type, Direction lastDirection)
        {
            List<Organelle> organelles = new List<Organelle>();

            int from = ((int)lastDirection + 3) % 6;

            int thres = 6;
            // 遍历顺时针的相邻细胞器
            for (int i = from; i < from + 6; i++)
            {
                if (type == cellManager._cells[id].m_cell.m_components.allcomponents[i % 6])
                {
                    thres--;
                    organelles.Add(NewOrganelle(type, (Direction)(i % 6), id));
                }
                else
                {
                    break;
                }
            }
            // 逆时针
            for (int i = from; i > from - thres; i--)
            {
                if (type == cellManager._cells[id].m_cell.m_components.allcomponents[i % 6])
                {
                    organelles.Add(NewOrganelle(type, (Direction)(i % 6), id));
                }
                else
                {
                    break;
                }
            }
            return organelles;
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
        
        private bool IsExit(Organelle organelle)
        {
            int[] index = {
                cellManager.UpCell(organelle.id),
                cellManager.RightUpCell(organelle.id),
                cellManager.RightDownCell(organelle.id),
                cellManager.DownCell(organelle.id),
                cellManager.LeftDownCell(organelle.id),
                cellManager.LeftUpCell(organelle.id)
            };
            
            if (index[(int)organelle.direction] == -1)
            {
                return true;
            }
            return false;
        }
        
        private List<Organelle> IsEntrance(int id)
        {
            List<Organelle> organelles = new List<Organelle>();
            
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
                    organelles.Add(NewOrganelle(ComponentType.Devour, (Direction)i, id));
                }
            }

            return organelles;
        }
    }
}