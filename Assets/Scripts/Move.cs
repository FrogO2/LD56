using Cell;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed=0.5f,aglspeed=1.0f;
    public float smooth = 1.0f;
    public float countplusa = 10.0f;
    private float tiltAngle = 0f;
    private Vector3 velocity = Vector3.zero;
    
    private void Update()
    {
        float count=MonoCellManager.Instance.MonoCellList.Count;
        float x = (count + countplusa) / count;
        float moveX = Input.GetAxis("Horizontal");
        tiltAngle += -aglspeed * moveX;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0, 0, tiltAngle), Time.deltaTime*smooth* x);
    }
    private void LateUpdate()
    {
        float count = MonoCellManager.Instance.MonoCellList.Count;
        float x = (count + countplusa) / count;
        float moveY = Input.GetAxis("Vertical");
        Vector3 dir= new(math.sin(-tiltAngle*math.PI/180), math.cos(-tiltAngle * math.PI / 180), 0);
        Vector3 endPos = transform.position + dir * speed * moveY/x;
        transform.position = Vector3.SmoothDamp(transform.position, endPos, ref velocity, Time.deltaTime * smooth);
    }
}
