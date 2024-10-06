using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private Dictionary<int, CellView> _cells;
        private BitArray CheckMap;
        private int MAXSIZE = 81;
        private int ROWNUM = 9;
        private int CENTER = 40;
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
            MonoCell destroyedCell = _cells[id].m_cell;
            RemoveMonoCellFromListByIndex(id);
            destroyedCell.Death();
        }

        private bool BitArrayIsEmpty()
        {

        }

        private int FindMinAvailableID()
        {
            for (int i = 0; i < CheckMap.Count; i++)
            {
                if (!CheckMap[i]) return i;
            }
            return -1;
        }

        private int FindClosedAvailableID(int id)
        {
            for (int i = 0; i < CheckMap.Count; i++)
            {
                if (!CheckMap[i]) return i;
            }
            return -1;
        }

        public void AddMonoCellToList(CellView cellView)
        {
            if(MonoCellList == null) CellListInit();
            MonoCellList.Add(cellView.m_cell);
            int id = FindMinAvailableID();
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