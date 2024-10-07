using Cell;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace component
{
    public class ComponentController : MonoBehaviour
    {
        public List<Transform> posList;
        public BaseComponent[] baseComponents;
        public List<BaseComponent> ComponentList;
        private Dictionary<System.Type, BaseComponent> componentPairs;
        public MonoCell cell;
        private void Awake()
        {
            cell = GetComponent<MonoCell>();
            ComponentList = new List<BaseComponent>{null, null, null, null, null, null};
            componentPairs = new Dictionary<System.Type, BaseComponent>(baseComponents.Length);
            InitDic();

            Transform t = transform.Find("Components");
            Debug.Log(t);
            posList = new List<Transform>() { t.Find("u"), t.Find("ru"), t.Find("rd"), t.Find("d"), t.Find("ld"), t.Find("lu") };
        }

        private void InitDic()
        {
            foreach (BaseComponent component in baseComponents)
            {
                componentPairs.Add(component.GetType(), component);
            }
        }
        public void RefreshComponents()
        {
            List<ComponentType> list = cell.m_components.allcomponents;
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i])
                {
                    case ComponentType.Devour:
                        ComponentList[i] = componentPairs[typeof(DevourComponent)];
                        break;
                    case ComponentType.Produce:
                        ComponentList[i] = componentPairs[typeof(ProduceComponent)];
                        break;
                    case ComponentType.Exhaust:
                        ComponentList[i] = componentPairs[typeof(ExhaustComponent)];
                        break;
                    case ComponentType.None:
                        ComponentList[i] = null;
                        break;
                    default:
                        break;
                }
            }
        }

        private void AttachComponentToObj(int index)
        {
            if (ComponentList[index] == null) return;
            GameObject.Instantiate(ComponentList[index], posList[index].position, Quaternion.Euler(0, 0, -index * 60), posList[index]);
        }

        private void RemoveComponentFromObj(int index)
        {
            if (posList[index].childCount == 0) return;
            Transform component = posList[index].GetChild(0);
            Destroy(component.gameObject);
        }

        public void ReplaceComponent(int index)
        {
            RemoveComponentFromObj(index);
            AttachComponentToObj(index);
        }

        public void UpdateAllComponents()
        {
            for (int i = 0; i < 6; i++)
            {
                ReplaceComponent(i);
            }
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                GetComponent<MonoCell>().m_components.up = ComponentType.Devour;
                GetComponent<MonoCell>().m_components.right_up = ComponentType.Produce;
                RefreshComponents();
                UpdateAllComponents();
            }
        }
    }
}
