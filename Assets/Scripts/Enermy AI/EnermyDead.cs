using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyDead : MonoBehaviour
{
    public EnermyCreate EnermyCreate;
    private void OnTriggerEnter2D(Collider2D other){
Destroy(gameObject);
EnermyCreate.de();
    }
}
