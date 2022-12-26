using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool rainOn=true;
    public bool rainOnMe=false;
    public int time=30;
    public float DeltaStarDrop=1f;
    bool rained=false;
    Vector3 target;
    public int starCounter=0;
    bool alive=true;
    public Color floorFirstColor;
    public Color floorLastColor;
    public float floorFadeIn=0.08f;
    float fadeTime= 3;
    float fadeStart= 0;
    public GameObject bg;
    public GameObject floors;
    public GameObject coins;
    public GameObject cam;
    public GameObject player;
    public GameObject LoseScreen;
    public Transform CoinIn;
    public Transform CoinOut;
    public Text height;
    public Text stars;
    public Text clock;
    int item;
                    //\\        
                   ///\\\
                  ///--\\\
                 ///----\\\
                ///------\\\
               ///////\\\\\\\
              ///----------\\\
             ///------------\\\
            ///--------------\\\
    //---------------------------------\\
    void Start()
    {
        
        player.transform.GetChild(0).gameObject.SetActive(true);
        alive=true;
    }
    
    void FixedUpdate(){
        floors.transform.GetChild(floors.transform.childCount-1).transform.localScale=
            Vector3.MoveTowards(floors.transform.GetChild(floors.transform.childCount-1).transform.localScale,
            new Vector3(floors.transform.GetChild(floors.transform.childCount-1).transform.localScale.x, 0.45f, floors.transform.GetChild(floors.transform.childCount-1).transform.localScale.z),
            floorFadeIn);
        
        ColorChanging(floors.transform.GetChild(floors.transform.childCount-1).gameObject);

    }

    void Update(){

        

        if(Input.GetMouseButtonDown(0)){
            target=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(target.y<player.transform.position.y-1.13f){
                Instantiate(floors.transform.GetChild(0), new Vector3(target.x,target.y,player.gameObject.transform.position.z), Quaternion.identity, floors.transform);
                floors.transform.GetChild(floors.transform.childCount-1).transform.localScale=new Vector3(floors.transform.GetChild(floors.transform.childCount-1).transform.localScale.x, 0f, floors.transform.GetChild(floors.transform.childCount-1).transform.localScale.z);
                fadeStart = 0;
            }
        }
        //ColorChanging(floors.transform.GetChild(floors.transform.childCount-1).gameObject);
        //floors.transform.GetChild(floors.transform.childCount-1).GetComponent<SpriteRenderer>().color = Color.Lerp(floorFirstColor, floorLastColor, Mathf.PingPong(Time.time * colorDuration, 1.0f));
        

        for(int i=0;i<coins.transform.childCount;i++){
            if(coins.transform.GetChild(i).position.y<CoinOut.transform.position.y){
                Destroy(coins.transform.GetChild(i).gameObject);
            }
        }

        for(int i=1;i<floors.transform.childCount;i++){
            if(floors.transform.GetChild(i).position.y<CoinOut.transform.position.y-15){
                Destroy(floors.transform.GetChild(i).gameObject);
            }
        }

        height.text=((int)player.transform.position.y) + " : h";
        stars.text= starCounter.ToString();

        if(player.transform.position.x>=0){
            bg.GetComponent<SpriteRenderer>().size=new Vector2(200+player.transform.position.y*2, 200+player.transform.position.x*2);
            bg.GetComponent<SpriteRenderer>().flipY=true;
        }   
        if(player.transform.position.x<0){
            bg.GetComponent<SpriteRenderer>().size=new Vector2(200+player.transform.position.y*2, 200-player.transform.position.x*2);
            bg.GetComponent<SpriteRenderer>().flipY=false;
        }

        if(rained==false){
            Debug.Log("rainer IF triggered");
            StartCoroutine(coinRain());
            StartCoroutine(Timer());
        }

        // Leave camera when player y lower than 0;
        if(player.transform.position.y<0 && alive){
            alive=false;
            leaveCamera();
        }

    }
    
    void ColorChanging(GameObject a)
    {
        fadeTime=floorFadeIn*37.5f;
        if (fadeStart < fadeTime)
        {
            fadeStart += Time.deltaTime * fadeTime;
 
            a.GetComponent<SpriteRenderer>().color= Color.Lerp(floorFirstColor, floorLastColor, fadeStart);
        }
    }
    
    public void restart(){
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);        
    }

    public void getCoin(){
        starCounter++;
    }

    public void bombed(){
        alive=false;
        leaveCamera();
        player.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void expand(){
        floors.transform.GetChild(0).GetComponent<BoxCollider2D>().size +=new Vector2(0.2f, 0);
        floors.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().size+=new Vector2(1.46f, 0);
    }

    void leaveCamera(){
        if(alive==false){
            cam.transform.parent=null;
            rainOn=false;

            player.transform.GetComponent<Rigidbody2D>().simulated=false;
            LoseScreen.SetActive(true);
            LoseScreen.transform.GetChild(1).GetComponent<Text>().text="Height: " + 0;
            LoseScreen.transform.GetChild(2).GetComponent<Text>().text="Coins: " + starCounter;
            LoseScreen.transform.GetChild(3).GetComponent<Text>().text="Score: " + 0;
        }
        else{
            
            rainOn=false;
            player.transform.GetComponent<Rigidbody2D>().simulated=false;
            LoseScreen.SetActive(true);
            LoseScreen.transform.GetChild(1).GetComponent<Text>().text="Height: " + player.transform.position.y;
            LoseScreen.transform.GetChild(2).GetComponent<Text>().text="Coins: " + starCounter;
            LoseScreen.transform.GetChild(3).GetComponent<Text>().text="Score: " + (int)player.transform.position.y*starCounter;
        }
    }

    public GameObject ItemCreator(){
        item=Random.Range(1,11);
        Debug.Log(item.ToString());

        
        //return cam.transform.GetChild(4).gameObject;
        
        if(item>9){
            return cam.transform.GetChild(3).gameObject;
            //Debug.Log(item.ToString() + "bomb");
        }
        else if(item<8){
            return cam.transform.GetChild(0).gameObject;
            //Debug.Log(item.ToString() + " coin");
        }
        else{
            return cam.transform.GetChild(4).gameObject;
            //Debug.Log(item.ToString() + "expand");
        }
    }

    IEnumerator coinRain(){

        if(rainOn && alive){    
            rained=true;

            if(!rainOnMe){
                Instantiate(ItemCreator(), CoinIn.transform.position+ new Vector3(Random.Range(-3,3), 0, 0), Quaternion.identity, coins.transform);
            }

            if(rainOnMe){
                Instantiate(ItemCreator(), CoinIn.transform.position, Quaternion.identity, coins.transform);
            }

            for(int i=0;i<coins.transform.childCount;i++){
                coins.transform.GetChild(i).GetComponent<Rigidbody2D>().simulated=true;

                if(coins.transform.GetChild(i).position.y<CoinOut.transform.position.y){
                    Destroy(coins.transform.GetChild(i).gameObject);
                }
            }
        }
        yield return new WaitForSeconds(DeltaStarDrop);
        StartCoroutine(coinRain());
    }

    IEnumerator Timer(){
        
        for(int i=time;i>=0;i--){

            if(i>99){
                clock.transform.GetChild(0).gameObject.SetActive(false);
            }
            else if(i<=99){
                clock.transform.GetChild(0).gameObject.SetActive(true);
            }
            
            clock.text=i.ToString();
            yield return new WaitForSeconds(1f);            
        }
        
        leaveCamera();
    }
}
