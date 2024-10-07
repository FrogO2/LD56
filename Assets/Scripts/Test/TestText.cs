using Cell;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace test
{
    public class TestText : MonoBehaviour
    {
        private void Awake()
        {
            for (int i = 0; i < 81; i++)
            {
                MonoCellManager.Instance.ChoosePos(i);
                //TextMeshPro.Instantiate<TextMeshPro>( )
            }
        }
        private void Update()
        {
            //text.text = cell.id.ToString();
        }
    }
}