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
        }
        public float GetCurrentSpan() => _current_span;
        private void RefreshCurrentSpan() => _current_span = span;

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
        }
    }
}