using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    private void Start()
    {
        // 获取所有子对象中的Collider组件
        CircleCollider2D[] colliders = GetComponentsInChildren<CircleCollider2D>();
        
        // 双重循环遍历所有的碰撞体，将它们设置为不发生碰撞
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = i + 1; j < colliders.Length; j++)
            {
                // 忽略这两个碰撞体之间的碰撞
                Physics2D.IgnoreCollision(colliders[i], colliders[j]);
            }
        }
    }
}
