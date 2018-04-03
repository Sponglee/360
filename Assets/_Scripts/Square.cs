using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    [SerializeField]
    private float speed;
 
    private int row;
    public int Row
    { get { return row; } set { row = value; } }
    [SerializeField]
    private int score;
    public int Score
    { get { return score; } set { score = value; } }

    [SerializeField]
    private Color32 color;

    public Transform centerPrefab;
    //for storing data
    public Transform Column
    {get{return column;}set{column = value;}}



    private Transform column;

    public bool Scaling = false;
    public bool Merged = false;
    public bool Touched = false;
    public bool IsSpawn = false;
    public bool ExpandSpawn { get; set; }


    [SerializeField]
    private Text SquareText;
    [SerializeField]
    private SpriteRenderer SquareColor;


    Vector3 destination;

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
            //gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();

            /******* METHOD PART***/

         
            ApplyStyle(this.score);

        }
        else
        {
            //gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();

            /******* METHOD PART***/

            gameObject.transform.SetParent(GameManager.Instance.currentSpot.transform);
            gameObject.name = gameObject.transform.GetSiblingIndex().ToString();
            ApplyStyle(this.score);
        }



        //Destinations
        centerPrefab = GameObject.Find("Wheel").transform;

        //destination = GameManager.Instance.spawns[int.Parse(gameObject.transform.parent.name)].transform.GetChild(gameObject.transform.GetSiblingIndex()).position;
    }
	
	// Update is called once per frame
	void Update () {

        //if Touched - stops 
       if(!this.Touched)
        { 
            
            gameObject.transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("Wheel").transform.position, speed * Time.deltaTime);
            
            
        }
        else
        {
            // if first square - move up by square length/2 (maybe variable this 0.55?)
            if (gameObject.transform.GetSiblingIndex() == 1)
            {
                gameObject.transform.localPosition = new Vector3(0.55f, 0, 0);
            }
        }
        
        // If there's no parent - fall
        if(this.gameObject.transform.parent == null)
        {
            this.Touched = false;
        }

        // Boundary
        if (Mathf.Abs(transform.position.y) > 100 || Mathf.Abs(transform.position.x) > 100)
        {
            Destroy(gameObject);
        }



    }


    public void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("spot"))
        {
            //Make it fall down

            this.Touched = true;


            if (this.gameObject.transform.parent != null)
            {
                GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score);
            }



            if (!Touched)
                gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.spawns[int.Parse(gameObject.transform.parent.name)].transform.GetChild(gameObject.transform.GetSiblingIndex()).position, speed * Time.deltaTime);
            else if (Touched && gameObject.transform.parent == null && !Scaling)
                gameObject.transform.position = Vector2.MoveTowards(transform.position, centerPrefab.position, speed * Time.deltaTime);

            //// Boundary
            //if (Mathf.Abs(transform.position.y) > 100 || Mathf.Abs(transform.position.x) > 100)
            //{
            //    Destroy(gameObject);
            //}


        }
    }



    public void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy on contact with center
        if (other.CompareTag("center") && gameObject.CompareTag("square"))
        {
            //Debug.Log("destroy this");
            Destroy(gameObject);
        }

        if (other.CompareTag("spot"))
        {

            if (transform.parent != null)
            {
                GameManager.Instance.CheckRow(int.Parse(transform.parent.name), transform.GetSiblingIndex(), score);
            }
        }


        if (other.CompareTag("square") && this.score == other.GetComponent<Square>().Score && transform.parent != null && transform.GetSiblingIndex() > other.transform.GetSiblingIndex())
        {
            Destroy(other.gameObject);
            this.Merged = true;
            GameManager.Instance.Merge(gameObject);
                
        }
        else if (other.CompareTag("square") &&  this.score != other.GetComponent<Square>().Score && transform.parent != null && transform.GetSiblingIndex() > other.transform.GetSiblingIndex())
        {
            
            ////Make it fall down
            //this.Touched = true;
            //Debug.Log("SQUARE COLLISION");

            GameManager.Instance.CheckRow(int.Parse(transform.parent.name), transform.GetSiblingIndex(), score);
          
           
            //Check GameOver
            GameManager.Instance.GameOver();


        }

    }


}






