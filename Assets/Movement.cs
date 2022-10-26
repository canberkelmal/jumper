using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Movement : MonoBehaviour
{
    
    Rigidbody2D rb;
    public GameObject dropEffect;
    
    public GameObject jumpEffect;
    public float JumpForce=10;
    public float SideForce=10;
    bool onFloor=true;
    bool second=true;
    
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }
    
    

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !onFloor && second){
            rb.AddForce(Vector2.up*JumpForce);
            Instantiate(jumpEffect,transform.position-new Vector3(0,0.8f,1),jumpEffect.transform.rotation);
            second=false;
            Debug.Log(onFloor);
        }
        if(Input.GetKeyDown(KeyCode.Space) && onFloor){
            rb.AddForce(Vector2.up*JumpForce);
            Instantiate(jumpEffect,transform.position-new Vector3(0,0.8f,1),jumpEffect.transform.rotation);
            onFloor=false;
            Debug.Log(onFloor);
        }
        if(Input.GetKey(KeyCode.D)){
            rb.AddForce(Vector2.right*SideForce);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        }
        if(Input.GetKey(KeyCode.A)){
            rb.AddForce(Vector2.left*SideForce);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        Instantiate(dropEffect,transform.position-new Vector3(0,0.8f,1),dropEffect.transform.rotation);
        onFloor=true;
        second=true;
        Debug.Log(onFloor);
    }    
}
