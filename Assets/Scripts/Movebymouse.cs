using Cell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movebymouse : MonoBehaviour
{
    public float speed = 0.5f;
    public float countplusa = 10.0f;
    public float smooth = 1.0f;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        //Vector3 mouse = Input.mousePosition;
        //mouse.z=Camera.main.nearClipPlane;
        //Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouse);

        //mouseWorldPos.z = 0;
        //Vector3 dir= mouseWorldPos - transform.position;
        //

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), smooth);
        //direction = Vector3.Normalize(dir);
        Vector3 mouse = Input.mousePosition;
        //获取物体坐标，物体坐标是世界坐标，将其转换成屏幕坐标，和鼠标一直 
        Vector3 obj = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = mouse - obj;
        dir.z = 0f;
        dir= dir.normalized;
        float angle = Vector3.Angle(dir,Vector3.up);
        float count = MonoCellManager.Instance.MonoCellList.Count;
        float x = (count + countplusa) / count;
        if (mouse.x > obj.x) angle = -angle;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * smooth * x);
        //transform.up= dir;
    }
    private void LateUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        float count = MonoCellManager.Instance.MonoCellList.Count;
        float x = (count + countplusa) / count;
        Vector3 endPos = transform.position + transform.right * speed * moveX /x+ transform.up * speed * moveY / x;
        transform.position = Vector3.SmoothDamp(transform.position, endPos, ref velocity, Time.deltaTime * smooth);
    }
}
