using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTester : MonoBehaviour
{
    public Animator animator1;  // Animator组件的引用
    public Animator animator2;  // Animator组件的引用
    



    void Update()
    {
        // 检测空格键的输入
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 设置Animator的Trigger来播放动画
            animator1.SetTrigger("PlayMitosis");
            animator2.SetTrigger("PlayMitosis");
        }
        

        

    }
}

