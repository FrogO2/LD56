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
        //private delegate GameObject CellFactoryMethod(CellData cellData);

        private CustomObjectFactory<GameObject> CellFactory;

        public void BitInit() => CheckMap = new BitArray(64, false);

        public void CellListInit()
        {
            if (MonoCellList != null) return;
            MonoCellList = new HashSet<MonoCell>();
        }

        public void MsgInit() 
        {
            TypeEventSystem.Global.Register<OnCreateCell>(e =>
            {
                CellFactory.Create();
            });
            TypeEventSystem.Global.Register<OnRegisterMonoCellCreating>(e =>
            {
                RegisterMonoCellCreating(e.cellView, e.data);
            });
        }

        public void Initialize()
        {
            BitInit();
            CellListInit();
            MsgInit();
        }

        public void RegisterMonoCellCreating(CellView cellView, CellData cellData)
        {
            CellFactory = new CustomObjectFactory<GameObject>(() => cellView.RandomCreate(cellData));
        }

        public void DestroyMonoCell()
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
        }
    }

    public class CustomObjectFactory<T> : IObjectFactory<T>
    {
        public CustomObjectFactory(Func<T> factoryMethod)
        {
            mFactoryMethod = factoryMethod;
        }

        protected Func<T> mFactoryMethod;

        public T Create()
        {
            return mFactoryMethod();
        }
    }
}