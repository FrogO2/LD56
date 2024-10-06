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

        public GameObject Create(CellData cellData, Vector3 pos)
        {
            GameObject target = null;

            if (target_cell == null)
            {
                Debug.Log("no target cell");
                target = Resources.Load<GameObject>(util.Pathes.BaseCellPath);
                m_cell_obj = GameObject.Instantiate<GameObject>(target, pos, new Quaternion());
            }
            else
            {
                target = GameObject.Instantiate<GameObject>(target_cell, pos, new Quaternion(), target_cell.transform.parent);
                m_cell_obj = target;
            }
            
            m_cell = m_cell_obj.GetComponent<MonoCell>();
            m_cell.SetCellData(cellData);
            MonoCellManager.Instance.AddMonoCellToList(this);
            return target;
        }

        public GameObject Create(CellData cellData)
        {
            GameObject target = null;
            int new_id = -1;
            if (target_cell == null)
            {
                Debug.Log("no target cell");
                target = Resources.Load<GameObject>(util.Pathes.BaseCellPath);
                m_cell_obj = GameObject.Instantiate<GameObject>(target, new Vector3(0, 0, 0), new Quaternion());
            }
            else
            {
                new_id = MonoCellManager.Instance.FindClosedAvailableID(target_cell.GetComponent<MonoCell>().id);
                Vector3 pos = MonoCellManager.Instance.ChoosePos(new_id);
                target = GameObject.Instantiate<GameObject>(target_cell, pos, new Quaternion(), target_cell.transform.parent);
                m_cell_obj = target;
            }

            m_cell = m_cell_obj.GetComponent<MonoCell>();
            m_cell.SetCellData(cellData);
            MonoCellManager.Instance.AddMonoCellToList(this, new_id);
            return target;
        }

        public GameObject CopyCell()
        {
            if (target_cell == null) return Create(data, MonoCellManager.Instance.ChoosePos(40));
            if (MonoCellManager.Instance.FindClosedAvailableID(target_cell.GetComponent<MonoCell>().id) < 0) return null;
            return Create(data);
        }

        public GameObject RandomCreate(CellData cellData)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(0, 10));
            return Create(cellData, pos);
        }
    }

    public struct OnCreateCell {  }
    public struct OnDestroyCell { public MonoCell monoCell; }
    public struct OnRegisterMonoCellCreating { public CellView cellView; }
}