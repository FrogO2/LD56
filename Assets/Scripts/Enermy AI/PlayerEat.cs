using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEat : MonoBehaviour
{
    public EnermyDead EnermyDead;
    public int k;
    public float force=1;
    public float scale=1;
    private Rigidbody2D rb;
    public GameObject Player;
    public CameraController CameraController;
    public EnermyCreate EnermyCreate;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


    }
private void OnTriggerEnter2D(Collider2D collision){
switch (collision.tag){
case "Square":
Destroy(collision.gameObject);
EnermyCreate.de();
break;
case "Ai":
Destroy(collision.gameObject);
EnermyCreate.de();
break;
default:break;
}
}
}
