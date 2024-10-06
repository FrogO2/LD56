using Cell;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Cell
{ 
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
            CellDataInit(this.resource, this.efficiency, this.span);
            _current_span = span;
            _current_resource = resource;
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