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
        private void Awake()
        {
            cellManager = MonoCellManager.Instance;
            
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("send msg");
                cellData.resource = 1000;
                cellData.efficiency = 10;
                cellData.span = 5;
                CellView test_cell = new CellView(GameObject.FindGameObjectWithTag("Cell"), cellData);
                TypeEventSystem.Global.Send ( new OnRegisterMonoCellCreating { cellView = test_cell, data = cellData });
                TypeEventSystem.Global.Send<OnCreateCell>();
            }
        }
    }
}