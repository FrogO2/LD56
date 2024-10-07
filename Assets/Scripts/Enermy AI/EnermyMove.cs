using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyMove : MonoBehaviour
{
    private Rigidbody2D rb; 
    private Vector2 offset;
    public GameObject player;
    private Vector2 movement;
    //public Move PlayerMove;
    public float timer = 0;
    public int cl=0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    
    void Update()
    {
    timer += Time.deltaTime;
    int k = Random.Range(0,3);
        if (timer >= 2)
        {if(k==0){
            Wonder();
            timer = 0;}
        else{
            Wonder();
            timer = 0;
        }
    }
    }
    void run(){
    if(cl==0){
        cl=1;
    }
    else{
        cl=0;
    }
    if(cl==0){
    offset = transform.position - player.transform.position;
    offset =-offset;
    movement= offset.normalized;
    rb.velocity = movement*10; 
    }
    if(cl==1){
    movement= new Vector2(0,0);
    rb.velocity = movement*10;
    }
    }
    void Wonder(){
    if(cl==0){
        cl=1;
    }
    else{
        cl=0;
    }
    if(cl==0){
    int a1=Random.Range(-100,100);
    int a2=Random.Range(-100,100);
    movement= new Vector2(a1,a2);
    movement= movement.normalized;
    rb.velocity = movement*10; 
    }
    if(cl==1){
    movement= new Vector2(0,0);
    rb.velocity = movement*10;
    }
    }
}

