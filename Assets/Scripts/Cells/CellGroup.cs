
using System.Collections;
using QFramework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using component;

namespace Cell
{
    public class CellGroup: MonoBehaviour
    {
        private MonoCellManager cellManager;
        private CustomObjectFactory<GameObject> CellFactory;
        private void Awake()
        {
            cellManager = MonoCellManager.Instance;
            Initialize();
        }
        private void Update()
        {
            
        }

        public void Initialize()
        {
            MsgInit();
        }

        public void MsgInit()
        {
            TypeEventSystem.Global.Register<OnCreateCell>(e =>
            {
                GameObject obj = CellFactory.Create();
                MonoCell cell = obj.GetComponent<MonoCell>();
                for (int j = 0; j < 6; j++)
                {
                    cell.SetComponent(j, (ComponentType)UnityEngine.Random.Range(0, 4));
                }
                ComponentController controller = cell.GetComponent<ComponentController>();
                controller.RefreshComponents();
                controller.UpdateAllComponents();
                BFS bfs = new BFS();
                var a = bfs.GetAll();
                string name = "";
                List<int> s = a.Item2[0];
                foreach (var VARIABLE in s)
                {
                    name += VARIABLE + " ";
                }
                Debug.LogWarning(name);
            }).UnRegisterWhenGameObjectDestroyed(this);
            TypeEventSystem.Global.Register<OnRegisterMonoCellCreating>(e =>
            {
                RegisterMonoCellCreating(e.cellView);
            }).UnRegisterWhenGameObjectDestroyed(this);
            TypeEventSystem.Global.Register<OnDestroyCell>(e =>
            {
                cellManager.DestroyMonoCellDying(e.monoCell.id);
            }).UnRegisterWhenGameObjectDestroyed(this);
        }

        public void RegisterMonoCellCreating(CellView cellView)
        {
            CellFactory = new CustomObjectFactory<GameObject>(() => cellView.CopyCell());
        }
    }

    public class CustomObjectFactory<T> : IObjectFactory<T>
    {
        public CustomObjectFactory(Func<T> factoryMethod)
        {
            mFactoryMethod = factoryMethod;
        }

        protected Func<T> mFactoryMethod;

        public T Create()
        {
            return mFactoryMethod();
        }
    }
}