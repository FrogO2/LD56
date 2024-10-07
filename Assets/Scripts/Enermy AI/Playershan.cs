using UnityEngine;
 
public class Playershan : MonoBehaviour
{
    public EnermyDead EnermyDead;
    public LayerMask collisionMask;
    private Vector2 k;
    public Vector2 size = new Vector2(2,3);
    public GameObject  core;
    public GameObject  devour;
    public float force=1;
    void Update()
    {
        k=-core.transform.position +devour.transform.position;
        k=k.normalized *3;
        float angle = Vector2.Angle(new Vector2(0,0), k);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position,size,angle,collisionMask);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.name=="Square(Clone)"){
            Rigidbody2D rb=hitCollider.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity=force*(-hitCollider.gameObject.transform.position+new Vector3(transform.position[0],transform.position[1],0));
            }

        }
    }
}