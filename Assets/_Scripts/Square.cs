using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    [SerializeField]
    private float speed;
    public bool IsSpawn = false;
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

    public bool IsColliding { get; set; }

    private Transform column;

    public bool Touched = false;
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


        if (!Touched)
            gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.spawns[int.Parse(gameObject.transform.parent.name)].transform.GetChild(gameObject.transform.GetSiblingIndex()).position, speed * Time.deltaTime);
        else
            gameObject.transform.position = Vector2.MoveTowards(transform.position, centerPrefab.position, speed * Time.deltaTime);

        //// Boundary
        //if (Mathf.Abs(transform.position.y) > 100 || Mathf.Abs(transform.position.x) > 100)
        //{
        //    Destroy(gameObject);
        //}



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


        if (other.CompareTag("square") && this.score == other.GetComponent<Square>().Score

                        && transform.parent != null)
        {
                GameManager.Instance.Merge(gameObject);
                Destroy(other.gameObject);
        }
        else if (other.CompareTag("square") &&  this.score != other.GetComponent<Square>().Score)
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






