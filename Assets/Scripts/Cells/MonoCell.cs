using Cell;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Cell
{ 

    public struct Components
    {
        public Component right_up;
        public bool is_outer_ru;
        public Component left_up;
        public bool is_outer_lu;
        public Component right_down;
        public bool is_outer_rd;
        public Component left_down;
        public bool is_outer_ld;
        public Component up;
        public bool is_outer_u;
        public Component down;
        public bool is_outer_d;
        public List<Component> allcomponents;
        public List<bool> allouters;
    }
    public class MonoCell : MonoBehaviour, BaseMonoCell
    {
        private CellData _cell_data;
        private int _cell_index;
        private float _current_span;
        private float _current_resource;
        private CellView m_cellView;

        public float resource;
        public float efficiency;
        public float span;

        public Components m_components;
        private bool isDevourActive = false;
        private bool isProduceActive = false;
        private bool isExhaustActive = false;

        

        public CellData cell_data
        { 
            get 
            {
                if (_cell_data == null)
                {
                    _cell_data = new CellData();
                }
                return _cell_data;
            }
        }

        public int id
        {
            get
            {
                return _cell_index;
            }
        }

        public virtual void Birth()
        {
            Initialize();
        }

        public virtual void Death()
        {
            Destroy(this.gameObject);
        }

        public void CellDataInit(float resource, float efficiency, float span)
        {
            this.cell_data.resource = resource;
            this.cell_data.efficiency = efficiency;
            this.cell_data.span = span;
        }

        public void Initialize()
        {
            CellDataInit(this.resource, this.efficiency, this.span);
            _current_span = span;
            _current_resource = resource;
            m_components = new Components();
            m_components.up.id = 0;
            m_components.right_up.id = 1;
            m_components.right_down.id = 2;
            m_components.down.id = 3;
            m_components.left_down.id = 4;
            m_components.left_up.id = 5;
            OuterUpdate();
            MsgInit();
            m_components.allcomponents = new List<Component> { m_components.up, m_components.right_up, m_components.right_down,
                                                            m_components.down, m_components.left_down, m_components.left_up};
            m_components.allouters = new List<bool> { m_components.is_outer_u, m_components.is_outer_ru, m_components.is_outer_rd,
                                                    m_components.is_outer_d, m_components.is_outer_ld, m_components.is_outer_lu};
        }

        public Component id2component(int id)
        {
            switch (id)
            {
                case 0:
                    return m_components.up;
                case 1:
                    return m_components.right_up;
                case 2:
                    return m_components.right_down;
                case 3:
                    return m_components.down;
                case 4:
                    return m_components.left_down;
                case 5:
                    return m_components.left_up;
                default:
                    return new Component();
            }
        }

        public int component2pairid(Component component)
        {
            int initid = component.id;
            if (initid >= 3) return id - 3;
            else return 3 - id;
        }

        public List<Component> Component2paircomponent(int id)
        {
            List<Component> components = new List<Component>(2);
            components[0] = id2component((id + 1) % 6);
            components[1] = id2component((id - 1) % 6);
            return components;
        }
        public void MsgInit()
        {
            TypeEventSystem.Global.Register<OnCreateCell>(e =>
            {
                OuterUpdate();
            }).UnRegisterWhenGameObjectDestroyed(this);
            TypeEventSystem.Global.Register<OnDestroyCell>(e =>
            {
                OuterUpdate();
            }).UnRegisterWhenGameObjectDestroyed(this);
        }
        public void SetCellView(CellView cellView) => m_cellView = cellView;

        public void SetId(int id)
        {
            _cell_index = id;
        }

        public void SetCellData(CellData data)
        {
            _cell_data = data;
            resource = data.resource;
            efficiency = data.efficiency;
            span = data.span;
            RefreshCurrentSpan();
            RefreshCurrentResource();
        }
        public float GetCurrentSpan() => _current_span;
        private void RefreshCurrentSpan() => _current_span = span;
        private void RefreshCurrentResource() => _current_resource = resource;

        public void OuterUpdate()
        {
            if (id < MonoCellManager.ROWNUM)
            {
                m_components.is_outer_lu = true;
                m_components.is_outer_ru = true;
            }
            if (id < 2*MonoCellManager.ROWNUM)
            {
                m_components.is_outer_u = true;
            }
            if (id % (2*MonoCellManager.ROWNUM) == 0)
            {
                m_components.is_outer_lu = true;
                m_components.is_outer_ld = true;
            }
            if (id % (2 * MonoCellManager.ROWNUM) == 2 * MonoCellManager.ROWNUM - 1)
            {
                m_components.is_outer_rd = true;
                m_components.is_outer_ru = true;
            }
            if (id >= MonoCellManager.MAXSIZE - 2 * MonoCellManager.ROWNUM)
            {
                m_components.is_outer_d = true;
            }
            if (id >= MonoCellManager.MAXSIZE - MonoCellManager.ROWNUM)
            {
                m_components.is_outer_ld = true;
                m_components.is_outer_rd = true;
            }
            if ((id / MonoCellManager.ROWNUM) % 2 == 1)
            {
                if (id - 2 * MonoCellManager.ROWNUM >= 0) 
                    if (!MonoCellManager.Instance.CheckMap[id - (2 * MonoCellManager.ROWNUM)]) 
                        m_components.is_outer_u = true;
                if (id - MonoCellManager.ROWNUM >= 0) 
                    if (!MonoCellManager.Instance.CheckMap[id - MonoCellManager.ROWNUM]) 
                        m_components.is_outer_lu = true;
                if (id - MonoCellManager.ROWNUM + 1 >= 0) 
                    if (!MonoCellManager.Instance.CheckMap[id - MonoCellManager.ROWNUM + 1]) 
                        m_components.is_outer_ru = true;
                if (id + MonoCellManager.ROWNUM < MonoCellManager.MAXSIZE) 
                    if (!MonoCellManager.Instance.CheckMap[id + MonoCellManager.ROWNUM]) 
                        m_components.is_outer_ld = true;
                if (id + MonoCellManager.ROWNUM + 1 < MonoCellManager.MAXSIZE) 
                    if (!MonoCellManager.Instance.CheckMap[id + MonoCellManager.ROWNUM + 1]) 
                        m_components.is_outer_rd = true;
                if (id + 2 * MonoCellManager.ROWNUM < MonoCellManager.MAXSIZE) 
                    if (!MonoCellManager.Instance.CheckMap[id + (2 * MonoCellManager.ROWNUM)]) 
                        m_components.is_outer_d = true;
            }
            else
            {
                if (id - 2 * MonoCellManager.ROWNUM >= 0)
                    if (!MonoCellManager.Instance.CheckMap[id - (2 * MonoCellManager.ROWNUM)])
                        m_components.is_outer_u = true;
                if (id - MonoCellManager.ROWNUM >= 0)
                    if (!MonoCellManager.Instance.CheckMap[id - MonoCellManager.ROWNUM -1])
                        m_components.is_outer_lu = true;
                if (id - MonoCellManager.ROWNUM + 1 >= 0)
                    if (!MonoCellManager.Instance.CheckMap[id - MonoCellManager.ROWNUM])
                        m_components.is_outer_ru = true;
                if (id + MonoCellManager.ROWNUM < MonoCellManager.MAXSIZE)
                    if (!MonoCellManager.Instance.CheckMap[id + MonoCellManager.ROWNUM - 1])
                        m_components.is_outer_ld = true;
                if (id + MonoCellManager.ROWNUM + 1 < MonoCellManager.MAXSIZE)
                    if (!MonoCellManager.Instance.CheckMap[id + MonoCellManager.ROWNUM])
                        m_components.is_outer_rd = true;
                if (id + 2 * MonoCellManager.ROWNUM < MonoCellManager.MAXSIZE)
                    if (!MonoCellManager.Instance.CheckMap[id + (2 * MonoCellManager.ROWNUM)])
                        m_components.is_outer_d = true;
            }
        }
        
        

        private void Awake()
        {
            Birth();
        }

        void Start()
        {
            
        }

        void Update()
        {
            if (_current_span > 0)
            {
                _current_span -= Time.deltaTime;
            }
            else if (enabled)
            {
                TypeEventSystem.Global.Send(new OnDestroyCell { monoCell = this});
                enabled = false;
            }
            if (_current_resource > 0)
            {
                _current_resource -= Time.deltaTime * 300;
            }
            if (_current_resource < 0.5f * resource) 
            {
                CellData new_data = new CellData();
                new_data.resource = resource;
                new_data.efficiency = efficiency;
                new_data.span = span;
                CellView new_cellView = new CellView(this.gameObject, new_data);
                new_cellView.target_cell = this.gameObject;
                TypeEventSystem.Global.Send(new OnRegisterMonoCellCreating { cellView = new_cellView });
                TypeEventSystem.Global.Send<OnCreateCell>();
                _current_resource += 0.5f*resource;
            }
        }
    }
}