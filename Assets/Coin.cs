using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameManager GameManager;
    bool collEd=true;
    public GameObject pickupEffect;
    public Rigidbody2D rb;
    public Vector3 coinTargetOfset;
    public Vector3 targetCoin;
    public float towards=1;
    // Start is called before the first frame update
    void Start()
    {
        rb=this.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetCoin=GameManager.cam.transform.GetChild(0).GetChild(1).transform.position;

        if(!collEd){
            coinTargetOfset = Vector3.MoveTowards(transform.position, targetCoin, towards);
            //targetPos = Vector3.MoveTowards(transform.GetChild(i).position, transform.GetChild(i-1).position, (swing/Mathf.Pow(i,pow))*Time.deltaTime);
            transform.position=coinTargetOfset;
            if(transform.position.y>targetCoin.y-0.001f){
                Instantiate(pickupEffect,transform.position,transform.rotation);
                GameManager.getCoin();
                Destroy(gameObject);
            }
        }

        /* if(transform.position.y<CoinOut.position.y){
            transform.position=new Vector2(Random.Range(-3,3),CoinIn.position.y);
            GetComponent<Rigidbody2D>().velocity=Vector2.zero;
        } */
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag=="Player" && col.gameObject.tag!="targetCoin" && collEd){
            collEd=false;
            Instantiate(pickupEffect,transform.position,transform.rotation);
            rb.simulated=false;
            rb.gravityScale=0;
            rb.velocity=new Vector2(0,0);
            
        }

    }
}
