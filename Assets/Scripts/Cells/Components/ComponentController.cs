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
            Debug.Log(cell);
            ComponentList = new List<BaseComponent>{null, null, null, null, null, null};
            componentPairs = new Dictionary<System.Type, BaseComponent>(baseComponents.Length);
            InitDic();

            Transform t = transform.Find("Components");
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
            ComponentType[] list = cell.m_components.allcomponents;
            for (int i = 0; i < 6; i++)
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
            BaseComponent obj = GameObject.Instantiate(ComponentList[index], posList[index].position, new Quaternion(), posList[index]);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -60 * index));
        }

        private void RemoveComponentFromObj(int index)
        {
            if (posList[index].childCount == 0) return;
            Transform component = posList[index].GetChild(0);
            Debug.Log(component);
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

        public void ContactComponent(string name, ComponentType type)
        {
            int index = -1;
            switch (name)
            {
                case "u":
                    index = 0; break;
                case "ru": 
                    index = 1; break;
                case "rd":
                    index = 2; break;
                case "d":
                    index = 3; break;
                case "ld":
                    index = 4; break;
                case "lu":
                    index = 5; break;
                default:
                    break;
            }
            cell.SetComponent(index, type);
            RefreshComponents();
            ReplaceComponent(index);
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                cell.SetComponent(0, ComponentType.Devour);
                cell.SetComponent(1, ComponentType.Exhaust);
                RefreshComponents();
                UpdateAllComponents();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                cell.SetComponent(0, ComponentType.Produce);
                cell.SetComponent(1, ComponentType.Devour);
                cell.SetComponent(2, ComponentType.Exhaust);
                RefreshComponents();
                UpdateAllComponents();
            }
        }
    }
}
