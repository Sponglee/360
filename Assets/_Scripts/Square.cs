using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    [SerializeField]
    public bool bottom=false;
    private int row;
    public int Row
    { get { return row; } set { row = value; } }
    [SerializeField]
    private int score;
    public int Score
    { get {return score;} set {score = value;} }

    //for storing data
    public Transform Column
    {get{return column;}set{column = value;}}

    public bool IsColliding { get; set; }

    private Transform column;
    //parent index iterator
    int i = 0;

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();

        /******* METHOD PART***/

        gameObject.transform.SetParent(GameManager.Instance.currentSpot.transform);
        gameObject.name = gameObject.transform.GetSiblingIndex().ToString();

    }
	
	// Update is called once per frame
	void Update () {

        if (Mathf.Abs(transform.position.y) > 100 || Mathf.Abs(transform.position.x) > 100)
        {
            Destroy(gameObject);
        }



    }

    public void OnCollisionEnter2D(Collision2D other)
    {
       
        if (other.gameObject.CompareTag("spot") && !bottom)
        {
            gameObject.transform.SetParent(GameManager.Instance.currentSpot.transform);


            gameObject.transform.position = other.gameObject.transform.position;

            //name of square's position
            gameObject.name = gameObject.transform.GetSiblingIndex().ToString();

            //Debug.Log(" -->> " + int.Parse(gameObject.transform.parent.name) + "   :   " + gameObject.transform.GetSiblingIndex() + "  :  " + score);
            GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score);

            this.column = other.gameObject.transform;
            bottom = true;
            

        }

        if (other.gameObject.CompareTag("square") && gameObject.transform.GetSiblingIndex()>other.gameObject.transform.GetSiblingIndex())
        {
            if (this.score == other.gameObject.GetComponent<Square>().Score /* && !bottom*/)
            {
                IsColliding = true;
               
                if (other.gameObject.GetComponent<Square>().Score == 64)
                {
                    Debug.Log("DESTOYEEERRRRR");                                                //falling down
                    GameManager.Instance.Merge(gameObject);

                    //Check if next spot is green and circle isnt full
                    if (GameManager.Instance.LastSpot != 0 && GameManager.Instance.spots[GameManager.Instance.LastSpot + 1].transform.GetChild(0).GetComponent<SpriteRenderer>().color != new Color32(0, 255, 0, 255))
                    {
                        GameManager.Instance.spots[GameManager.Instance.LastSpot + 1].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
                        GameManager.Instance.LastSpot += 1;
                    }
                    else
                        GameManager.Instance.LastSpot = 0;
                   
                    gameObject.GetComponent<Collider2D>().isTrigger = true;
                    bottom = false;
                }
                else
                {
                     GameManager.Instance.Merge(gameObject);

                }
                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();
                Destroy(other.gameObject);
               
            }
            else if (this.score != other.gameObject.GetComponent<Square>().Score)
            {
                //gameObject.transform.SetParent(other.gameObject.transform.parent);                                     //for II VARIANT COMMENT THIS
               
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(200, 200, 200, 255);
                GameManager.Instance.GameOver();
                //gameObject.isStatic = true;

            }
            gameObject.name = gameObject.transform.GetSiblingIndex().ToString();
            //Debug.Log(" -->> " + int.Parse(gameObject.transform.parent.name) + "   :   " + gameObject.transform.GetSiblingIndex() + "  :  " + score);
            //Check for boops
            GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score);
        }
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("center") && gameObject.CompareTag("square"))
        {
            Debug.Log("destroy this");
            int thisBottom = GameManager.Instance.nBottom;
            Debug.Log(thisBottom);

            //GameManager.Instance.Shrink(GameManager.Instance.wheel.transform.GetChild(0).GetComponent<CircleCollider2D>().radius, thisBottom);
            Destroy(gameObject);
        }

    }
    
}







/***
 * 
 * 
 * 
 * 
 * 
 * public class TouchInput : MonoBehaviour {
private Vector3 screenPoint;
private Vector3 initialPosition;
private Vector3 offset;
public float speed = 20.0f;

Rigidbody2D rb;

void Start(){
    rb = gameObject.GetComponent<Rigidbody2D>();
}

void OnMouseDown(){
    Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y,     screenPoint.z);
    Vector3 initialPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
}

void OnMouseDrag(){
   Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y,     screenPoint.z);
   Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
   Vector3 heading = cursorPosition - initialPosition;
   Vector3 direction = heading / heading.magnitude;     // heading magnitude = distance 
   rb.velocity = new Vector3(150 * Time.deltaTime, 0, 0); 
   //Do what you want.
   //if you want to drag object on only swipe gesture comment below. Otherwise:
   initialPosition = cursorPosition;
}

void OnMouseUp()
{
   rb.velocity = new Vector3(0, 0, 0);
}
}
*///