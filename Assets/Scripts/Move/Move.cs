using Cell;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Move : MonoBehaviour
{
    public float speed=0.5f,aglspeed=1.0f;
    public float smooth = 1.0f;
    public float countplusa = 10.0f;
    private float tiltAngle = 0f;
    private Vector3 velocity = Vector3.zero;
    
    private void Update()
    {
        float count=math.max(MonoCellManager.Instance.MonoCellList.Count,1);
        float x = (count + countplusa) / count;
        float moveX = 0;
        if(Input.GetKey(KeyCode.Q)) moveX = -1;
        if (Input.GetKey(KeyCode.E)) moveX = 1;
        tiltAngle += -aglspeed * moveX;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0, 0, tiltAngle), Time.deltaTime*smooth* x);
    }
    private void LateUpdate()
    {
        float count = math.max(MonoCellManager.Instance.MonoCellList.Count, 1);
        float x = (count + countplusa) / count;
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        //Vector3 dir= new(math.sin(-tiltAngle*math.PI/180), math.cos(-tiltAngle * math.PI / 180), 0);
        Vector3 endPos = transform.position + transform.right * speed * moveX / x + transform.up * speed * moveY / x;
        transform.position = Vector3.SmoothDamp(transform.position, endPos, ref velocity, Time.deltaTime * smooth);
    }
}
