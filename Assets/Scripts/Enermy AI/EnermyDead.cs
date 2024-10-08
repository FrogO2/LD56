using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyDead : MonoBehaviour
{
    public float k1;
    private Rigidbody2D rb; 
    private Vector2 offset;
    public GameObject square;
    private Vector2 movement;
    private EnermyCreate enermyCreate;
    public float res;
    public Cell.MonoCell Mono;
    public float scale;
    public List<int> t=new List<int>(100);
    private void OnTriggerEnter2D(Collider2D collision){
        switch (collision.tag){
            case "Cell":
                Destroy(gameObject);
                enermyCreate.de();
                break;
            case "Ai":
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
    void Awake(){
        GameObject a = GameObject.Find("EventSystem (1)");
        enermyCreate=a.GetComponent<EnermyCreate>();
    }
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {

    }
    public void run1(float force){
        Vector2  sq= transform.position;
        offset = sq - new Vector2(-1,-1);
        offset =-offset;
        movement= offset.normalized;
        rb.velocity = movement*force*10; 
    }
    public void stop1(){
        movement= new Vector2(0,0);
        rb.velocity = movement*10;
    }
}
