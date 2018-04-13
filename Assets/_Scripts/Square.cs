﻿using System.Collections;
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

    //pop moving point
    public Transform centerPrefab;

    private bool IsTop = false;
 
    private Transform squareTmpSquare = null;
    public Transform SquareTmpSquare
    {
        get
        {
            return squareTmpSquare;
        }

        set
        {
            squareTmpSquare = value;
            
        }
    }

    public bool IsColliding { get; set; }

    private Transform column;
    [SerializeField]
    private bool touched = false;
   

    public bool Touched
    {
        get
        {
            return touched;
        }

        set
        {
            touched = value;
        }
    }



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
    void Update() {
        //if Touched - stops 
        //if(!this.Touched)
        // {
        // If it's first and not touched - fall
        if (this.gameObject.transform.parent != null)
        {
            if (gameObject.transform.GetSiblingIndex() == 5 && gameObject.transform.position != GameManager.Instance.spawns[int.Parse(gameObject.transform.parent.name)].transform.GetChild(5).position)
            
                gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.spawns[int.Parse(gameObject.transform.parent.name)].transform.GetChild(5).position, speed * Time.deltaTime);
            else
                gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.spawns[int.Parse(gameObject.transform.parent.name)].transform.GetChild(gameObject.transform.GetSiblingIndex()).position, speed * Time.deltaTime);

        }
        else if (this.IsTop == true)
        {
            gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.wheel.transform.position, speed * Time.deltaTime);
        }
        else
        {
            gameObject.transform.position = Vector2.MoveTowards(transform.position, squareTmpSquare.position, speed * Time.deltaTime);
        }

        //}
        //else
        //{
          
        ////}
        //if (this.gameObject.transform.parent == null)
        //{
            

        //}


        // Boundary
        if (Mathf.Abs(transform.position.y) > 100 || Mathf.Abs(transform.position.x) > 100)
        {
            Destroy(gameObject);
        }
        else if (squareTmpSquare != null && transform.position == squareTmpSquare.position)
        {
            Destroy(gameObject);
        }



    }

    // DOUBT IF NEEDED SEE FIXED UPDATE
    public void OnCollisionEnter2D(Collision2D other)
    {
       

        if (other.gameObject.CompareTag("spot"))
        {
           // Debug.Log(" SQUARE " + this.Score + " " + gameObject.transform.parent.name + ":" + gameObject.transform.GetSiblingIndex());

            GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score, this.gameObject);

           
        }
        //other square
        if (other.gameObject.CompareTag("square") && gameObject.CompareTag("square") && !this.touched /*&& gameObject.transform.GetSiblingIndex() > other.gameObject.transform.GetSiblingIndex()*/)
        {
            //make sure checks only one of 2 collisions (one that is not touched
            other.gameObject.GetComponent<Square>().touched = true;
           
            if (this.score == other.gameObject.GetComponent<Square>().Score)
            {
               // Debug.Log("SCORE : " + gameObject.GetComponent<Square>().Score + " to " + other.gameObject.GetComponent<Square>().Score);
                //if spawned by player and pops - no moves 
                if (this.IsSpawn)
                {
                    this.IsSpawn = false;
                }

                
                
                GameManager.Instance.Merge(gameObject, other.gameObject);
                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();
               

            }
            else if (this.score != other.gameObject.GetComponent<Square>().Score)
            {
               // Debug.Log(" SQUARE " + this.Score + " " + gameObject.transform.parent.name + ":" + gameObject.transform.GetSiblingIndex() );


                GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score, this.gameObject);


                //reset Touched bool 
                StartCoroutine(StopTouch(other.gameObject));
                
                //Check GameOver
                GameManager.Instance.GameOver();
              



                //Debug.Log("!!SCORE : " + gameObject.GetComponent<Square>().Score + " to " + other.gameObject.GetComponent<Square>().Score);

            }

            gameObject.name = gameObject.transform.GetSiblingIndex().ToString();
            //other.gameObject.GetComponent<Square>().touched = false;
           
            //Check for boops

        }

        //Make it green again
        if (gameObject.transform.parent != null && gameObject.CompareTag("square"))
        {

            if (gameObject.transform.parent.childCount < 5)
            {
                if (gameObject.transform.parent.GetComponent<Spot>().Blocked == false)
                {
                    //Debug.Log("u can ");
                    gameObject.transform.parent.GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
                }
            }
        }


        if (this.score >= 256)
        {
            this.IsTop = true;
            this.gameObject.transform.parent = null;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }


    private IEnumerator StopTouch(GameObject first)
    {
        yield return new WaitForSeconds(0.15f);
        first.GetComponent<Square>().touched = false;
    }




    public void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy on contact with center
        if (other.CompareTag("center") && gameObject.CompareTag("square"))
        {
            GameManager.Instance.scores += this.score;
            GameManager.Instance.ScoreText.text = GameManager.Instance.scores.ToString();
            //Debug.Log("destroy this");
            Destroy(gameObject);
        }
        //if (other.gameObject.CompareTag("square") && this.score != other.gameObject.GetComponent<Square>().Score

        //                && gameObject.transform.parent != null)
        //{
        //    //this.Touched = true;
        //    Debug.Log("SQUARE ENTER");
        //}


    }

    //public void OnTriggerExit2D(Collider2D other)
    //{
    //    //square and other is lower than this
    //    if (other.CompareTag("square") && gameObject.CompareTag("square") && gameObject.transform.GetSiblingIndex() > other.gameObject.transform.GetSiblingIndex())
    //    {
    //        //Debug.Log("SQUARE EXIT");
    //        gameObject.GetComponent<Square>().Touched = false;
    //    }

    //}


    //public void OnTriggerStay2D(Collider2D other)
    //{

    //    if (other.CompareTag("square")&& gameObject.transform.parent !=null && other.gameObject.transform.parent !=null)
    //    {
    //        Debug.Log("Stay");
    //        this.Touched = true;
    //    }
    //}


}






