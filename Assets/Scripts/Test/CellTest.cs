using Cell;
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
                cellData.span = 5;
                CellView test_cell = new CellView();
                test_cell.CreateWithParent(cellData, initTransform);
            }
        }
    }
}