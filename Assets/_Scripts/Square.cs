using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    [SerializeField]
    public bool bottom = false;
    private int row;
    public int Row
    { get { return row; } set { row = value; } }
    [SerializeField]
    private int score;
    public int Score
    { get { return score; } set { score = value; } }


  
    //for storing data
    public Transform Column
    {get{return column;}set{column = value;}}

    public bool IsColliding { get; set; }

    private Transform column;


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
       
        if (other.gameObject.CompareTag("spot"))
        {
           

            //gameObject.GetComponent<SpriteRenderer>().color = new Color32(200, 200, 200, 255);
            //gameObject.transform.position = other.gameObject.transform.position;

            //name of square's position
            //gameObject.name = gameObject.transform.GetSiblingIndex().ToString();

            //Debug.Log(" -->> " + int.Parse(gameObject.transform.parent.name) + "   :   " + gameObject.transform.GetSiblingIndex() + "  :  " + score);
           
            GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score);

            this.column = other.gameObject.transform;
            
            

        }

        if (other.gameObject.CompareTag("square") && gameObject.transform.GetSiblingIndex()>other.gameObject.transform.GetSiblingIndex())
        {
            
            if (this.score == other.gameObject.GetComponent<Square>().Score )
            {
                IsColliding = true;
                //Merge squares
                GameManager.Instance.Merge(gameObject);
                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();
                Destroy(other.gameObject);
            }
            else if (this.score != other.gameObject.GetComponent<Square>().Score)
            {
                
                //gameObject.transform.SetParent(other.gameObject.transform.parent);                                     //for II VARIANT COMMENT THIS
                //gameObject.GetComponent<SpriteRenderer>().color = new Color32(200, 200, 200, 255);

                //Check Row
               
                GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score);
                //Check GameOver
                GameManager.Instance.GameOver();

                //Make it green again
                if (gameObject.transform.parent !=null)
                {
                    if (gameObject.transform.parent.childCount < 6)
                    {
                        if (gameObject.transform.parent.GetComponent<Spot>().Blocked == false)
                        {
                            Debug.Log("u can ");
                            gameObject.transform.parent.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
                        }
                           
                        else
                            Debug.Log("u can't drop it");
                    }
                }
               

            }
            gameObject.name = gameObject.transform.GetSiblingIndex().ToString();
            //Debug.Log(" -->> " + int.Parse(gameObject.transform.parent.name) + "   :   " + gameObject.transform.GetSiblingIndex() + "  :  " + score);
            //Check for boops
            
        }
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("center") && gameObject.CompareTag("square"))
        {
            Debug.Log("destroy this");
            Destroy(gameObject);
        }

    }
    
}






