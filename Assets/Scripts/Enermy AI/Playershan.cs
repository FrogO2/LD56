using UnityEngine;
 
public class Playershan : MonoBehaviour
{
    public EnermyDead EnermyDead;
    public float radius = 5;
    public LayerMask collisionMask;
    public Vector3 k;
    public GameObject  core;
    public GameObject  devour;
    public float force=1;
    void Update()
    {
        k=-core.transform.position +devour.transform.position;
        k=k.normalized *3;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position +k, radius, collisionMask);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.name=="Square(Clone)"){
            Rigidbody2D rb=hitCollider.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity=force*(-hitCollider.gameObject.transform.position+new Vector3(transform.position[0],transform.position[1],0));
            }

        }
    }
}