using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cell
{
    public class CellView
    {
        public GameObject target_cell;
        public GameObject m_cell_obj;
        public MonoCell m_cell;
        public CellData data;

        public CellView(GameObject cell, CellData cellData)
        {
            target_cell = cell;
            data = cellData;
        }

        public CellView(GameObject cell)
        {
            target_cell = cell;
        }

        public CellView()
        {
            target_cell = null;
        }

        public GameObject Create(CellData cellData)
        {
            GameObject target = null;

            if (target_cell == null)
            {
                Debug.Log("no target cell");
                target = Resources.Load<GameObject>(util.Pathes.BaseCellPath);
                m_cell_obj = target.Instantiate<GameObject>();
            }
            else
            {
                target = target_cell.Instantiate<GameObject>();
                m_cell_obj = target;
            }
            
            m_cell = m_cell_obj.GetComponent<MonoCell>();
            m_cell.SetCellData(cellData);
            return target;
        }

        public GameObject RandomCreate(CellData cellData)
        {
            GameObject target = null;
            Vector3 pos = new Vector3(UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(0, 10));
            if (target_cell == null)
            {
                Debug.Log("no target cell");
                target = Resources.Load<GameObject>(util.Pathes.BaseCellPath);
                m_cell_obj = target.Instantiate<GameObject>(pos, new Quaternion());
            }
            else
            {
                target = target_cell.Instantiate<GameObject>(pos, new Quaternion());
                m_cell_obj = target;
            }

            m_cell = m_cell_obj.GetComponent<MonoCell>();
            m_cell.SetCellData(cellData);
            return target;
        }
    }

    public struct OnCreateCell {  }
    public struct OnDestroyCell { }
    public struct OnRegisterMonoCellCreating { public CellView cellView; public CellData data; }
}