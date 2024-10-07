using Cell;
using QFramework;
using System;
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
        private Stack<Transform> dirtydata;
        public MonoCell cell;
        private void Awake()
        {
            cell = GetComponent<MonoCell>();
            Debug.Log(cell);
            ComponentList = new List<BaseComponent>{null, null, null, null, null, null};
            componentPairs = new Dictionary<System.Type, BaseComponent>(baseComponents.Length);
            dirtydata = new Stack<Transform>();
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
            string str = "";
            for (int i = 0; i < 6; i++)
            {
                switch (list[i])
                {
                    case ComponentType.Devour:
                        ComponentList[i] = componentPairs[typeof(DevourComponent)];
                        str += "DE ";
                        break;
                    case ComponentType.Produce:
                        ComponentList[i] = componentPairs[typeof(ProduceComponent)];
                        str += "PU ";
                        break;
                    case ComponentType.Exhaust:
                        ComponentList[i] = componentPairs[typeof(ExhaustComponent)];
                        str += "EX ";
                        break;
                    case ComponentType.None:
                        ComponentList[i] = null;
                        str += "None ";
                        break;
                    default:
                        Debug.LogError("Unexpected Type: " + list[i]);
                        break;
                }
            }
            Debug.Log(str);
        }

        private void AttachComponentToObj(int index)
        {
            if (ComponentList[index] == null) return;
            BaseComponent obj = GameObject.Instantiate(ComponentList[index], posList[index].position, new Quaternion(), posList[index]);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -60 * index));
        }

        private void MarkComponentFromObj(int index)
        {
            if (posList[index].childCount == 0) return;
            for (int i = 0; i < posList[index].childCount; i++)
            {
                dirtydata.Push(posList[index].GetChild(i));
            }
        }

        private void RemoveComponents()
        {
            while (!dirtydata.IsNullOrEmpty())
            {
                Destroy(dirtydata.Pop().gameObject);
            }
        }

        public void ReplaceComponent(int index)
        {
            MarkComponentFromObj(index);
            AttachComponentToObj(index);
            RemoveComponents();
        }

        public void UpdateAllComponents()
        {
            for (int i = 0; i < 6; i++)
            {
                MarkComponentFromObj(i);
                AttachComponentToObj(i);
            }
            RemoveComponents();
        }

        public void DeleteAllComponents()
        {
            for (int i = 0; i < 6; i++)
            {
                MarkComponentFromObj(i);
            }
            RemoveComponents();
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
                    Debug.LogError("Unexpected Name");
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

        }

        private void OnMouseDown()
        {
            ComponentType[] temp = new ComponentType[6];
            for (int i = 0; i < 6; i++)
            {
                if (i < 5) temp[i + 1] = cell.m_components.allcomponents[i];
                else temp[0] = cell.m_components.allcomponents[i];
            }
            for (int i = 0;i < 6;i++) cell.m_components.allcomponents[i] = temp[i];
            RefreshComponents();
            //DeleteAllComponents();
            UpdateAllComponents();
        }
    }
}
