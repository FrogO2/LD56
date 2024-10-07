

using QFramework;
using System;
using Unity.VisualScripting;
using UnityEngine;

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