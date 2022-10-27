using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expand : MonoBehaviour
{
    public GameManager GameManager;
    bool collEd=true;
    public GameObject pickupEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag=="Player" && col.gameObject.tag!="targetCoin" && collEd){
            collEd=false;
            Instantiate(pickupEffect,transform.position,transform.rotation);
            GameManager.expand();
            Destroy(gameObject);            
        }

    }
}
