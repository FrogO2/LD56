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
            //for (int i = 0; i < 81; i++) 
            //{
            //    GameObject obj = Resources.Load<GameObject>(util.Pathes.BaseCellPath);
            //    GameObject.Instantiate(obj, MonoCellManager.Instance.ChoosePos(i), new Quaternion(), GameObject.Find("CellGroup").transform);
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
                CellView test_cell = new CellView(GameObject.FindGameObjectWithTag("Cell"), cellData);
                TypeEventSystem.Global.Send ( new OnRegisterMonoCellCreating { cellView = test_cell });
                TypeEventSystem.Global.Send<OnCreateCell>();
            }
        }
    }
}