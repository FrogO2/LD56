using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyWith : MonoBehaviour
{
    private Vector2 movement;
    public EnermyMove EnermyMove;
    private Rigidbody2D rb; 
    private Vector2 offset;
    public GameObject Player;
    public GameObject AI;
    public float t;
    //public Move PlayerMove;
    public float timer = 0;
    public int cl=0;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log(Player);
    }

    // Update is called once per frame
    
    void Update()
    {
    timer += Time.deltaTime;
        if (timer >= 1)
        {
            ran();
            timer = 0;
    }
    }
    public void run(){
    if(cl==0){
        cl=1;
    }
    else{
        cl=0;
    }
    if(cl==0){
    offset = transform.position - Player.transform.position;
    offset =offset;
    movement= offset.normalized;
    rb.velocity = movement*10; 
    }
    if(cl==1){
    movement= new Vector2(0,0);
    rb.velocity = movement*10;
    }
    }
    public void Wonder(){
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
    void ran(){
    offset = transform.position - Player.transform.position;
    float a1=offset[0];
    float a2=offset[1];
    Vector2 k=AI.transform.position; 
    t=(a1-k[0])*(a1-k[0])+(a2-k[1])*(a2-k[1]);

    if(t<=500){
        run();
    }
    else if(t>500){
        Wonder();
    }
    if(cl==1){
    Vector2 movement= new Vector2(0,0);
    rb.velocity = movement*10;
    }
}
}

