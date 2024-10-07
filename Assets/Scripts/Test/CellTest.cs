using Cell;
using component;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace test
{
    public class CellTest: MonoBehaviour
    {
        MonoCellManager cellManager;
        CellData cellData = new CellData();
        public Transform initTransform;
        private void Awake()
        {
            cellManager = MonoCellManager.Instance;
            //for (int i = 0; i < 81; i++)
            //{
            //    MonoCellManager.Instance.FindClosedAvailableID(i);
            //}

        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("send msg");
                cellData.resource = 1000;
                cellData.efficiency = 10;
                cellData.span = 60;
                CellView test_cell = new CellView();
                test_cell.CreateWithParent(cellData, initTransform);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log("send msg");
                cellData.resource = 1000;
                cellData.efficiency = 10;
                cellData.span = 1000;
                for (int i = 3; i < 54; i+=9)
                {
                    for (int offset = 0; offset < 5; offset++)
                    {
                        CellView test_cell = new CellView();
                        test_cell.CreateWithParentWithPos(cellData, initTransform, i + offset);
                        for (int j = 0; j < 6; j++)
                        {
                            test_cell.m_cell.SetComponent(j, (ComponentType)Random.Range(0, 5));
                        }
                        ComponentController controller = test_cell.m_cell.GetComponent<ComponentController>();
                        controller.RefreshComponents();
                        controller.UpdateAllComponents();
                    }
                }
            }
        }
    }
}