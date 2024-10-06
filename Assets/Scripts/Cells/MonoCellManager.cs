using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Cell
{
    public class MonoCellManager
    {
        private static MonoCellManager _instance;
        public static MonoCellManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MonoCellManager();
                    _instance.Initialize();
                }
                return _instance;
            }
            private set => _instance = value;
        }

        public HashSet<MonoCell> MonoCellList;
        public Dictionary<int, CellView> _cells;
        public BitArray CheckMap;
        private int MAXSIZE = 81;
        private int ROWNUM = 9;
        private int CENTER = 40;
        private Vector3 ZeroPoint = new Vector3(-1.52f * 4, 0.88f * 2, 0);
        //private delegate GameObject CellFactoryMethod(CellData cellData);

        public void BitInit() => CheckMap = new BitArray(81, false);

        public void CellListInit()
        {
            if (MonoCellList != null && _cells != null) return;
            if (MonoCellList == null) MonoCellList = new HashSet<MonoCell>();
            if (_cells == null) _cells = new Dictionary<int, CellView>();
        }

        public void Initialize()
        {
            BitInit();
            CellListInit();
        }

        public void DestroyMonoCell(int id)
        {
            CellView destroyedCellView = _cells[id];
            MonoCell destroyedCell = _cells[id].m_cell;
            RemoveMonoCellFromListByIndex(id);
            destroyedCell.Death();
        }

        private bool CheckMapIsEmpty()
        {
            foreach (bool item in CheckMap)
            {
                if (item) return false;
            }
            return true;
        }

        private int FindMinAvailableID()
        {
            for (int i = 0; i < CheckMap.Count; i++)
            {
                if (!CheckMap[i]) return i;
            }
            return -1;
        }

        public int FindClosedAvailableID(int id)
        {
            Queue<int> ints = new Queue<int>();
            int num = 0;
            for (int i = 0; i < 6; i++) 
            {
                if (id < ROWNUM && (i == 0 || i == 2)) continue;
                if (id < 2*ROWNUM && i == 1) continue;
                if (id >= MAXSIZE - ROWNUM && (i == 3 || i == 5)) continue;
                if (id >= MAXSIZE - 2*ROWNUM && (i == 4)) continue;
                if ((id % (2 * ROWNUM) == 0) && (i == 3 || i == 0)) continue;
                if ((id % (2 * ROWNUM) == 2*ROWNUM - 1) && (i == 2 || i == 5)) continue;
                if ((id/ROWNUM)%2 == 1) {
                    if (id - 2 * ROWNUM >= 0) if (!CheckMap[id - (2 * ROWNUM)] && i == 1) {
                            ints.Enqueue(id - (2 * ROWNUM));
                            num++;
                        }
                    if (id - ROWNUM >= 0) if (!CheckMap[id - ROWNUM] && i == 0) {
                            ints.Enqueue(id - ROWNUM);
                            num++;
                        }
                    if (id - ROWNUM + 1 >= 0) if (!CheckMap[id - ROWNUM + 1] && i == 2) {
                            ints.Enqueue(id - ROWNUM + 1);
                            num++;
                        }
                    if (id + ROWNUM < MAXSIZE) if (!CheckMap[id + ROWNUM] && i == 3) {
                            ints.Enqueue(id + ROWNUM);
                            num++;
                        }
                    if (id + ROWNUM + 1 < MAXSIZE) if (!CheckMap[id + ROWNUM + 1] && i == 5) {
                            ints.Enqueue(id + ROWNUM + 1);
                            num++;
                        }
                    if (id + 2 * ROWNUM < MAXSIZE) if (!CheckMap[id + (2 * ROWNUM)] && i == 4) {
                            ints.Enqueue(id + (2 * ROWNUM));
                            num++;
                        }
                }
                else
                {
                    if (id - 2 * ROWNUM >= 0) if (!CheckMap[id - (2 * ROWNUM)] && i == 1)
                        {
                            ints.Enqueue(id - (2 * ROWNUM));
                            num++;
                        }
                    if (id - ROWNUM - 1 >= 0) if (!CheckMap[id - ROWNUM - 1] && i == 0)
                        {
                            ints.Enqueue(id - ROWNUM - 1);
                            num++;
                        }
                    if (id - ROWNUM>= 0) if (!CheckMap[id - ROWNUM] && i == 2)
                        {
                            ints.Enqueue(id - ROWNUM);
                            num++;
                        }
                    if (id + ROWNUM - 1 < MAXSIZE) if (!CheckMap[id + ROWNUM - 1] && i == 3)
                        {
                            ints.Enqueue(id + ROWNUM - 1);
                            num++;
                        }
                    if (id + ROWNUM < MAXSIZE) if (!CheckMap[id + ROWNUM] && i == 5)
                        {
                            ints.Enqueue(id + ROWNUM);
                            num++;
                        }
                    if (id + 2 * ROWNUM < MAXSIZE) if (!CheckMap[id + (2 * ROWNUM)] && i == 4)
                        {
                            ints.Enqueue(id + (2 * ROWNUM));
                            num++;
                        }
                }
            }
            if (ints.Count == 0) return -1;
            int[] arr = new int[num];
            //Debug.Log(num);
            //string str = "";
            for (int i = 0; i < num; i++)
            {
                arr[i] = ints.Dequeue();
                //str += arr[i] + " ";
            }
            int choose = UnityEngine.Random.Range(0, num);
            //Debug.Log("id: " + id + "|" + str);
            return arr[choose];
        }

        public int LeftUpCell(int id)
        {
            if (id % (2 * ROWNUM) != 0 || id >= ROWNUM) 
            {
                if ((id / ROWNUM) % 2 == 1 && CheckMap[id - ROWNUM]) return id - ROWNUM;
                else if ((id / ROWNUM) % 2 == 0 && CheckMap[id - ROWNUM - 1]) return id - ROWNUM - 1;
            }
            return -1;
        }

        public int RightUpCell(int id)
        {
            if (id % (2 * ROWNUM) != 2 * ROWNUM - 1 || id >= ROWNUM)
            {
                if ((id / ROWNUM) % 2 == 1 && CheckMap[id - ROWNUM + 1]) return id - ROWNUM + 1;
                else if ((id / ROWNUM) % 2 == 0 && CheckMap[id - ROWNUM]) return id - ROWNUM;
            }
            return -1;
        }

        public int LeftDownCell(int id)
        {
            if (id % (2 * ROWNUM) != 0 || id < MAXSIZE - ROWNUM)
            {
                if ((id / ROWNUM) % 2 == 1 && CheckMap[id + ROWNUM]) return id + ROWNUM;
                else if ((id / ROWNUM) % 2 == 0 && CheckMap[id + ROWNUM - 1]) return id + ROWNUM - 1;
            }
            return -1;
        }

        public int RightDownCell(int id)
        {
            if (id % (2 * ROWNUM) != 2 * ROWNUM - 1 || id < MAXSIZE - ROWNUM)
            {
                if ((id / ROWNUM) % 2 == 1 && CheckMap[id + ROWNUM + 1]) return id - ROWNUM + 1;
                else if ((id / ROWNUM) % 2 == 0 && CheckMap[id - ROWNUM]) return id - ROWNUM;
            }
            return -1;
        }

        public int UpCell(int id)
        {
            if ( id >= 2*ROWNUM)
            {
                return id - (2 * ROWNUM);
            }
            return -1;
        }

        public int DownCell(int id)
        {
            if (id < MAXSIZE - 2 * ROWNUM)
            {
                return id + (2 * ROWNUM);
            }
            return -1;
        }

        public Vector3 ChoosePos(int id)
        {
            Vector3 pos = ZeroPoint;
            float d_x = id % ROWNUM * 1.52f + (id / ROWNUM)%2 * 0.76f;
            float d_y = id / ROWNUM * 0.44f;
            pos.x += d_x;
            pos.y -= d_y;
            return pos;
        }

        public void AddMonoCellToList(CellView cellView)
        {
            int id;
            if(MonoCellList == null) CellListInit();
            MonoCellList.Add(cellView.m_cell);
            if (CheckMapIsEmpty()) id = CENTER;
            else id = FindClosedAvailableID(cellView.target_cell.GetComponent<MonoCell>().id);
            Debug.Log(id);
            if (id < 0) return;
            else 
            {
                cellView.m_cell.SetId(id);
                _cells.Add(id, cellView);
                CheckMap[id] = true;
            }
        }

        public void AddMonoCellToList(CellView cellView, int id)
        {
            if (MonoCellList == null) CellListInit();
            MonoCellList.Add(cellView.m_cell);
            if (id < 0) return;
            else
            {
                cellView.m_cell.SetId(id);
                _cells.Add(id, cellView);
                CheckMap[id] = true;
            }
        }

        public void AddMonoCellToListByIndex(int id)
        {
            if (!_cells.ContainsKey(id))
            {
                Debug.LogWarning("Cell " + id + " wasn't registered correctly");
                return;
            }
            CellView cell = _cells[id];
            if (cell != null)
            {
                AddMonoCellToList(cell);
            }
        }

        public void RemoveMonoCellFromList(CellView cellView)
        {
            if (!MonoCellList.Contains(cellView.m_cell)) return;
            else MonoCellList.Remove(cellView.m_cell);
            _cells.Remove(cellView.m_cell.id);
        }

        public void RemoveMonoCellFromListByIndex(int id)
        {
            if (!_cells.ContainsKey(id))
            {
                Debug.LogWarning("Cell " + id + " wasn't registered correctly");
                return;
            }
            CellView cell = _cells[id];
            if (cell != null)
            {
                RemoveMonoCellFromList(cell);
            }
            _cells.Remove(id);
            CheckMap[id] = false;
        }
    }
}