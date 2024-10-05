using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerMove : MonoBehaviour
{
public  TMP_Text text;
 // Rigidbody of the player.
 private Rigidbody2D rb; 

 // Movement along X and Y axes.
 private float movementX;
 private float movementY;
 public float playerSpeed;
 // Speed at which the player moves.
 public float speed = 0; 
public Vector3 playerVector;
public Transform playerPosition;
 // Start is called before the first frame update.
 void Start()
    {

 // Get and store the Rigidbody component attached to the player.
      rb = GetComponent<Rigidbody2D>();
    }
 
 // This function is called when a move input is detected.
 /*void OnMove(InputValue movementValue)
    {
 // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

 // Store the X and Y components of the movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }*/
 // FixedUpdate is called once per fixed frame-rate frame.
 private void FixedUpdate() 
    {
 // Create a 3D movement vector using the X and Y inputs.
        float moveH=Input.GetAxis("Horizontal");
        float moveV=Input.GetAxis("Vertical");
        Vector2 movement=new Vector2(moveH,moveV);

 // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed); 
        }
   public void SetSpeend(int i){
      playerSpeed=i;
      playerVector=new Vector3(0,0,0);
      playerPosition.position += playerVector * playerSpeed;
   }
    }