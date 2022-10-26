using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public GameManager GameManager;
    bool collEd=true;
    public GameObject pickupEffect;
    public Vector3 coinTargetOfset;
    public Vector3 targetCoin;
    public float towards=1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag=="Player" && col.gameObject.tag!="targetCoin" && collEd){
            collEd=false;
            Instantiate(pickupEffect,transform.position,transform.rotation);
            GameManager.bombed();
            Destroy(gameObject);            
        }

    }
}
