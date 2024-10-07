using Cell;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

namespace Cell
{ 

    public struct Components
    {
        public ComponentType right_up;
        public bool is_outer_ru;
        public ComponentType left_up;
        public bool is_outer_lu;
        public ComponentType right_down;
        public bool is_outer_rd;
        public ComponentType left_down;
        public bool is_outer_ld;
        public ComponentType up;
        public bool is_outer_u;
        public ComponentType down;
        public bool is_outer_d;
        public ComponentType[] allcomponents;
        public bool[] allouters;
    }
    public class MonoCell : MonoBehaviour, BaseMonoCell
    {
        private CellData _cell_data;
        private int _cell_index;
        private float _current_span;
        private float _current_resource;
        private CellView m_cellView;
        private MonoCellManager m_monoCellManager = MonoCellManager.Instance;

        public float resource;
        public float efficiency;
        public float span;

        public Components m_components;
        private bool isDevourActive = false;
        private bool isProduceActive = false;
        private bool isExhaustActive = false;

        public bool isOuter => m_components.is_outer_d || m_components.is_outer_ru || m_components.is_outer_lu ||
                                m_components.is_outer_u || m_components.is_outer_ld || m_components.is_outer_rd;

        public bool NoSpace => m_monoCellManager.FindClosedAvailableID(id) < 0;   

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

        public void Birth()
        {
            Initialize();
        }

        public void Death()
        {
            //AudioKit.PlaySound("Cell Died");
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
            //m_spriteRenderer = GetComponent<SpriteRenderer>();
            CellDataInit(this.resource, this.efficiency, this.span);
            _current_span = span;
            _current_resource = 0;
            m_components = new Components();
            m_components.allcomponents = new ComponentType[6] {ComponentType.None, ComponentType.None, ComponentType.None,
                                                                ComponentType.None,ComponentType.None,ComponentType.None,};
            m_components.allouters = new bool[6];
            OuterUpdate();
            MsgInit();
            
        }

        public ComponentType id2component(int id)
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
                    return ComponentType.None;
            }
        }

        public int Id2pairid(int _id)
        {
            if (_id >= 3) return _id - 3;
            else return 3 - _id;
        }

        public List<int> Component2paircomponent(int _id)
        {
            List<int> components_id = new List<int>(2);
            components_id[0] = (id + 1) % 6;
            components_id[1] = (id - 1) % 6;
            return components_id;
        }
        
        public void SetComponent(int index, ComponentType type)
        {
            m_components.allcomponents[index] = type;
            switch (id)
            {
                case 0:
                    m_components.up = type;
                    break;
                case 1:
                    m_components.right_up = type;
                    break;
                case 2:
                    m_components.right_down = type;
                    break;
                case 3:
                    m_components.down = type;
                    break;
                case 4:
                    m_components.left_down = type;
                    break;
                case 5:
                    m_components.left_up = type;
                    break;
                default:
                    break;
            }
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
        private void RefreshCurrentResource() => _current_resource = 0;

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
                if (id - MonoCellManager.ROWNUM - 1 >= 0)
                    if (!MonoCellManager.Instance.CheckMap[id - MonoCellManager.ROWNUM -1])
                        m_components.is_outer_lu = true;
                if (id - MonoCellManager.ROWNUM >= 0)
                    if (!MonoCellManager.Instance.CheckMap[id - MonoCellManager.ROWNUM])
                        m_components.is_outer_ru = true;
                if (id + MonoCellManager.ROWNUM - 1 < MonoCellManager.MAXSIZE)
                    if (!MonoCellManager.Instance.CheckMap[id + MonoCellManager.ROWNUM - 1])
                        m_components.is_outer_ld = true;
                if (id + MonoCellManager.ROWNUM < MonoCellManager.MAXSIZE)
                    if (!MonoCellManager.Instance.CheckMap[id + MonoCellManager.ROWNUM])
                        m_components.is_outer_rd = true;
                if (id + 2 * MonoCellManager.ROWNUM < MonoCellManager.MAXSIZE)
                    if (!MonoCellManager.Instance.CheckMap[id + (2 * MonoCellManager.ROWNUM)])
                        m_components.is_outer_d = true;
            }
            if (m_components.is_outer_u) m_components.allouters[0] = true;
            if (m_components.is_outer_ru) m_components.allouters[1] = true;
            if (m_components.is_outer_rd) m_components.allouters[2] = true;
            if (m_components.is_outer_d) m_components.allouters[3] = true;
            if (m_components.is_outer_ld) m_components.allouters[4] = true;
            if (m_components.is_outer_lu) m_components.allouters[5] = true;
        }

        public void AddResource(int num)
        {
            _current_resource += num;
        }

        private void Awake()
        {
            Initialize();
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

            if (_current_resource < resource) _current_resource += 10*Time.deltaTime;

            if (_current_resource >= resource && enabled && !NoSpace) 
            {
                CellData new_data = new CellData();
                new_data.resource = resource;
                new_data.efficiency = efficiency;
                new_data.span = span;
                CellView new_cellView = new CellView(this.gameObject, new_data);
                new_cellView.target_cell = this.gameObject;
                TypeEventSystem.Global.Send(new OnRegisterMonoCellCreating { cellView = new_cellView });
                TypeEventSystem.Global.Send<OnCreateCell>();
                _current_resource -= resource;
            }
        }
        private float parameter = 0f;
        private IEnumerator Dying(float s)
        {
            yield return new WaitForSeconds(s);
            Color color = new Color(255, 255, 255);
            while (parameter < 1)
            {
                parameter += Time.deltaTime;
                var tg = GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer t in tg)
                {
                    t.color = Color.Lerp(color, new Color(255, 255, 255, 0f), parameter);
                }
                GetComponentInChildren<SpriteShapeRenderer>().color = Color.Lerp(color, new Color(255, 255, 255, 0f), parameter);
                yield return null;
            }
            Death();
            yield return null;
        }

        public void StartDying()
        {
            var tg = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D t in tg)
            {
                t.enabled = false;
            }
            StartCoroutine(Dying(0));
        }

        public void StartDyingWaitForSeconds(float s)
        {
            var tg = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D t in tg)
            {
                t.enabled = false;
            }
            StartCoroutine(Dying(s));
        }
    }
}
