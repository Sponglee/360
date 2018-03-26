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

    [SerializeField]
    private Color32 color;
    
    //for storing data
    public Transform Column
    {get{return column;}set{column = value;}}

    public bool IsColliding { get; set; }

    private Transform column;

    private bool expandSpawn=false;
    public bool ExpandSpawn { get; set; }


    [SerializeField]
    private Text SquareText;
    [SerializeField]
    private SpriteRenderer SquareColor;

    private void Awake()
    {
       

    }

    // Helps ApplyStyle to grab numbers/color
    void ApplyStyleFromHolder(int index)
    {
        SquareText.text = SquareStyleHolder.Instance.SquareStyles[index].Number.ToString();
        SquareText.color = SquareStyleHolder.Instance.SquareStyles[index].TextColor;
        SquareColor.color = SquareStyleHolder.Instance.SquareStyles[index].SquareColor;
    }
    //Gets Values from style script for each square
    public void ApplyStyle(int num)
    {
        switch(num)
        {
            case 2:
                ApplyStyleFromHolder(0);
                break;
            case 4:
                ApplyStyleFromHolder(1);
                break;
            case 8:
                ApplyStyleFromHolder(2);
                break;
            case 16:
                ApplyStyleFromHolder(3);
                break;
            case 32:
                ApplyStyleFromHolder(4);
                break;
            case 64:
                ApplyStyleFromHolder(5);
                break;
            case 128:
                ApplyStyleFromHolder(6);
                break;
            case 256:
                ApplyStyleFromHolder(7);
                break;
            default:
                Debug.LogError("Check the number that u pass to ApplyStyle");
                break;
        }
    }

    // Use this for initialization
    void Start () {
        if (ExpandSpawn)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();

            /******* METHOD PART***/

         
            ApplyStyle(this.score);
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();

            /******* METHOD PART***/

            gameObject.transform.SetParent(GameManager.Instance.currentSpot.transform);
            gameObject.name = gameObject.transform.GetSiblingIndex().ToString();
            ApplyStyle(this.score);
        }
       
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

               


            }
            gameObject.name = gameObject.transform.GetSiblingIndex().ToString();
            //Debug.Log(" -->> " + int.Parse(gameObject.transform.parent.name) + "   :   " + gameObject.transform.GetSiblingIndex() + "  :  " + score);
            //Check for boops
            
        }

        //Make it green again
        if (gameObject.transform.parent != null && gameObject.CompareTag("square"))
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



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("center") && gameObject.CompareTag("square"))
        {
            Debug.Log("destroy this");
            Destroy(gameObject);
        }

    }
    
}






