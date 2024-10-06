using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyMove : MonoBehaviour
{
    private Rigidbody2D rb; 
    private Vector2 offset;
    public GameObject player;
    public Move PlayerMove;
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
        if (timer >= 1)
        {
            run();
            timer = 0;
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
    Vector2 movement= offset.normalized;
    rb.velocity = movement*10; 
    }
    if(cl==1){
    Vector2 movement= new Vector2(0,0);
    rb.velocity = movement*10;
    }
}
}

