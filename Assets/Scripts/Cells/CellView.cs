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
                m_cell_obj = GameObject.Instantiate<GameObject>(target, pos, Quaternion.identity);
            }
            else
            {
                target = GameObject.Instantiate<GameObject>(target_cell, pos, Quaternion.identity, target_cell.transform.parent);
                m_cell_obj = target;
            }

            m_cell = m_cell_obj.GetComponent<MonoCell>();
            m_cell.SetCellData(cellData);
            MonoCellManager.Instance.AddMonoCellToList(this);

            // 添加 SpringJoint2D 并连接到父对象
            Rigidbody2D parentRb = target.transform.parent?.GetComponent<Rigidbody2D>(); // 获取父对象的 Rigidbody2D
            if (parentRb != null)
            {
                SpringJoint2D springJoint = target.AddComponent<SpringJoint2D>(); // 添加 SpringJoint2D 组件
                springJoint.connectedBody = parentRb; // 将 SpringJoint2D 的连接体设置为父对象的 Rigidbody2D

                // 调整 SpringJoint2D 的参数（可根据实际需求调整）
                springJoint.autoConfigureDistance = false; // 禁用自动距离配置
                springJoint.distance = 0; // 弹簧原始距离为 0，固定在原位
                springJoint.dampingRatio = 0.5f; // 阻尼比，可以调整弹簧的阻尼效果
                springJoint.frequency = 10.0f; // 弹簧频率，控制弹簧的回弹速度
                Debug.LogWarning("Working");
            }
            else
            {
                Debug.LogWarning("Parent object does not have a Rigidbody2D component!");
            }

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
                m_cell_obj = GameObject.Instantiate<GameObject>(target, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("CellGroup").transform);
            }
            else
            {
                new_id = MonoCellManager.Instance.FindClosedAvailableID(target_cell.GetComponent<MonoCell>().id);
                Vector3 pos = MonoCellManager.Instance.ChoosePos(new_id);
                target = GameObject.Instantiate<GameObject>(target_cell, pos, Quaternion.identity, target_cell.transform.parent);
                target.transform.localPosition = pos;
                m_cell_obj = target;
            }

            m_cell = m_cell_obj.GetComponent<MonoCell>();
            m_cell.SetCellData(cellData);
            MonoCellManager.Instance.AddMonoCellToList(this, new_id);

            // 添加 SpringJoint2D 并连接到父对象
            Rigidbody2D parentRb = target.transform.parent?.GetComponent<Rigidbody2D>(); // 获取父对象的 Rigidbody2D
            if (parentRb != null)
            {
                SpringJoint2D springJoint = target.AddComponent<SpringJoint2D>(); // 添加 SpringJoint2D 组件
                springJoint.connectedBody = parentRb; // 将 SpringJoint2D 的连接体设置为父对象的 Rigidbody2D

                // 调整 SpringJoint2D 的参数（可根据实际需求调整）
                springJoint.autoConfigureDistance = false; // 禁用自动距离配置
                springJoint.distance = 0; // 弹簧原始距离为 0，固定在原位
                springJoint.dampingRatio = 0.5f; // 阻尼比，可以调整弹簧的阻尼效果
                springJoint.frequency = 10.0f; // 弹簧频率，控制弹簧的回弹速度
                Debug.LogWarning("Working");
            }
            else
            {
                Debug.LogWarning("Parent object does not have a Rigidbody2D component!");
            }

            return target;
        }


        public GameObject CreateWithParent(CellData cellData, Transform transform)
        {
            GameObject target = null;

            if (target_cell == null)
            {
                Debug.Log("no target cell");
                target = Resources.Load<GameObject>(util.Pathes.BaseCellPath);
                m_cell_obj = GameObject.Instantiate<GameObject>(target, new Vector3(0, 0, 0), new Quaternion(), transform);
            }

            m_cell = m_cell_obj.GetComponent<MonoCell>();
            m_cell.SetCellData(cellData);
            MonoCellManager.Instance.AddMonoCellToList(this);
            return target;
        }

        public GameObject CreateWithParentWithPos(CellData cellData, Transform transform, int index)
        {
            GameObject target = null;

            if (target_cell == null)
            {
                Debug.Log("no target cell");
                target = Resources.Load<GameObject>(util.Pathes.BaseCellPath);
                m_cell_obj = GameObject.Instantiate<GameObject>(target, MonoCellManager.Instance.ChoosePos(index), new Quaternion(), transform);
            }

            m_cell = m_cell_obj.GetComponent<MonoCell>();
            m_cell.SetCellData(cellData);
            MonoCellManager.Instance.AddMonoCellToListWithID(this, index);
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
    public struct OnDevour { }
}