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
        TextMeshPro text;
        MonoCell cell;
        private void Awake()
        {
            text = GetComponentInChildren<TextMeshPro>();
            cell = GetComponent<MonoCell>();
        }
        private void Update()
        {
            text.text = cell.GetCurrentSpan().ToString("0.0");
        }
    }
}