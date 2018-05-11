﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    [SerializeField]
    private float speed = 10f;
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }
    public bool IsSpawn = false;
    private int row;
    public int Row
    { get { return row; } set { row = value; } }
    [SerializeField]
    private int score;
    public int Score
    { get { return score; } set { score = value; } }


    GameObject desto;
    public GameObject Desto
    {
        get
        {
            return desto;
        }

        set
        {
            desto = value;
        }
    }




    // For sounds 
    [SerializeField]
    private bool further = false;
    public bool Further
    {
        get
        {
            return further;
        }

        set
        {
            further = value;
        }
    }


    [SerializeField]
    private Color32 color;

    //pop moving point
    public Transform centerPrefab;

    private bool IsTop = false;

    public bool ColumnPew = false;


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
    // toggle for further pop first
    [SerializeField]
    private bool checkPriority = false;
    public bool CheckPriority
    {
        get
        {
            return checkPriority;
        }

        set
        {
            checkPriority = value;
        }
    }
 

    public bool IsColliding { get; set; }
    private Transform column;

    // for stopping squares that move
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
    //For column pops 
    [SerializeField]
    private bool checkAround = true;
    public bool CheckAround
    {
        get
        {
            return checkAround;
        }

        set
        {
            checkAround = value;
        }
    }

    private int checkGrid;

    public bool ExpandSpawn { get; set; }

    //for checkRow disable once merged(let it fall first)
    [SerializeField]
    private bool isMerging = false;
    public bool IsMerging
    {
        get
        {
            return isMerging;
        }

        set
        {
            isMerging = value;
        }
    }
    private bool isChecking = false;
    public bool IsChecking
    {
        get
        {
            return isChecking;
        }

        set
        {
            isChecking = value;
        }
    }


    //[SerializeField]
    //private bool IsMoving = false;
    [SerializeField]
    private Text SquareText;
    [SerializeField]
    private SpriteRenderer SquareColor;
    [SerializeField]
    private bool mergeCheck=false;
    public bool MergeCheck
    {
        get
        {
            return mergeCheck;
        }

        set
        {
            mergeCheck = value;
        }
    }

   
    Vector2 curPos;
    Vector2 lastPos;




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
            //case 512:
            //    ApplyStyleFromHolder(8);
            //    break;
            default:
                Debug.LogError("Check the number that u pass to ApplyStyle");
                break;
        }
    }

    // Use this for initialization
    void Start () {

        
    
        if (ExpandSpawn)
        {
           // gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();

         
            ApplyStyle(this.score);

        }
        else
        {
            //gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();

            gameObject.transform.SetParent(GameManager.Instance.currentSpot.transform);
            gameObject.name = gameObject.transform.GetSiblingIndex().ToString();
            ApplyStyle(this.score);
        }



        checkGrid = transform.GetSiblingIndex();
       
    }


    // Update is called once per frame
    void FixedUpdate() {

        //Check if something is moving
        curPos = gameObject.transform.localPosition;
        if (curPos == lastPos)
        {
            //IsMoving = false;
            GameManager.Instance.SomethingIsMoving = false;
        }
        else
        {
            //IsMoving = true;
            GameManager.Instance.SomethingIsMoving = true;
        }
        lastPos = curPos;

        //for expandMoves
        if (this.checkPriority)
        {
            this.IsSpawn = false;
        }

            
        if (!this.gameObject.transform.parent.CompareTag("outer"))
        {
            //If siblingindex changed => check around
            if (gameObject.transform.GetSiblingIndex() != checkGrid)
            {
                CheckAround = true;
                checkGrid = gameObject.transform.GetSiblingIndex();
            }
           

            //Move to needed grid spot
            if (gameObject.transform.GetSiblingIndex() == 5 
                && gameObject.transform.position != GameManager.Instance.spawns[int.Parse(gameObject.transform.parent.name)].transform.GetChild(5).position)
            {
                //GameManager.Instance.SomethingIsMoving = true;
                gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.spawns[int.Parse(gameObject.transform.parent.name)].transform.GetChild(5).position, Speed * Time.deltaTime);
            }
            else
            {
                //GameManager.Instance.SomethingIsMoving = true;
                gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.spawns[int.Parse(gameObject.transform.parent.name)].transform.GetChild(gameObject.transform.GetSiblingIndex()).position, Speed * Time.deltaTime);

            }

        }
        //256 square to center
        else if (this.IsTop == true)
        {
            IsTop = false;
            AudioManager.Instance.PlaySound("256");
            //Debug.Log("YOSH");
            gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.wheel.transform.position, Speed * Time.deltaTime);
        }
        else
        {
            //Debug.Log("NOT YOSH");
            //GameManager.Instance.SomethingIsMoving = true;
            if (SquareTmpSquare != null)
                gameObject.transform.position = Vector2.MoveTowards(transform.position, squareTmpSquare.position, Speed * Time.deltaTime);
            else
                gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.wheel.transform.position, Speed * Time.deltaTime);
        }



        // Boundary
        if (Mathf.Abs(transform.position.y) > 100 || Mathf.Abs(transform.position.x) > 100)
        {
            Destroy(gameObject);
        }
        //reached tmpSquare
        else if (squareTmpSquare != null && transform.position == squareTmpSquare.position)
        {
            if(GameManager.Instance.checkObjs.Contains(gameObject))
            {
                Debug.Log("DADADADA ETO KAVKAZ");
            }
                Destroy(gameObject);
        }



    }



    //private IEnumerator StopCheckAround()
    //{
    //    Debug.Log("STOP");
    //    yield return new WaitForSeconds(0.2f);
      


    //}

    //    //Make it green again and drop 256
    //private void MakeItGreen()
    //{
       
    //    if (!gameObject.transform.parent.CompareTag("outer") && gameObject.CompareTag("square"))
    //    {

    //        if (gameObject.transform.parent.childCount < 5)
    //        {
    //            if (gameObject.transform.parent.GetComponent<Spot>().Blocked == false)
    //            {
    //                //Debug.Log("u can ");
    //                gameObject.transform.parent.GetComponent<SpriteRenderer>().color = leGreen;
    //            }
    //        }
    //    }


    //    if (this.score >= 256)
    //    {
    //        this.IsTop = true;
            
    //        this.gameObject.transform.parent = gameObject.transform.parent.parent.parent.GetChild(3);
    //        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    //    }
    //}


    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //   
    //}
    // DOUBT IF NEEDED SEE FIXED UPDATE
    public void OnCollisionEnter2D(Collision2D other)
    {

        //if (other.gameObject.CompareTag("spot") || (other.gameObject.CompareTag("square") && !gameObject.CompareTag("square")))
        //          AudioManager.Instance.PlaySound("bump");


            if (other.gameObject.CompareTag("spot"))
            {

                //if (this.IsSpawn)
                //{
                //    this.IsSpawn = false;
                //}

                // Debug.Log(" SQUARE " + this.Score + " " + gameObject.transform.parent.name + ":" + gameObject.transform.GetSiblingIndex());

                //reset speed back
                this.speed = 10f;

                //for column checkrow
                GameManager.Instance.checkObjs.Enqueue(gameObject);

                //GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score, this.gameObject);
               // this.NotTouched = true;


            }
        //other square
        if (other.gameObject.CompareTag("square") && gameObject.CompareTag("square") && !this.touched /*&& gameObject.transform.GetSiblingIndex() > other.gameObject.transform.GetSiblingIndex()*/)
        {
            //make sure checks only one of 2 collisions (one that is not touched
            other.gameObject.GetComponent<Square>().touched = true;


            if (this.score == other.gameObject.GetComponent<Square>().Score)
            {
              
                //if spawned by player and pops - no moves 
                if (this.IsSpawn)
                {
                    this.IsSpawn = false;
                }

                //GameManager.Instance.scores += score;
               
                GameManager.Instance.Merge(gameObject, other.gameObject);
                
               
            }
            else if (this.score != other.gameObject.GetComponent<Square>().Score)
            {
                
                //reset speed back
                this.speed = 10f;

                //for column checkrow
                GameManager.Instance.checkObjs.Enqueue(gameObject);

                //reset Touched bool 
                StartCoroutine(StopTouch(other.gameObject));

                //Check GameOver
                GameManager.Instance.GameOver(gameObject.transform.parent.gameObject);
            }
            gameObject.name = gameObject.transform.GetSiblingIndex().ToString();

        }

        //Make it green again
        if (!gameObject.transform.parent.CompareTag("outer") && gameObject.CompareTag("square"))
        {

            if (gameObject.transform.parent.childCount < 4)
            {
                if (gameObject.transform.parent.GetComponent<Spot>().Blocked == false)
                {
                    //Debug.Log("u can ");
                    gameObject.transform.parent.GetComponent<SpriteRenderer>().color = GameManager.Instance.leGreen;
                }
            }
            else if (gameObject.transform.parent.childCount == 4)
            {
                if (gameObject.transform.parent.GetComponent<Spot>().Blocked == false)
                {
                    //Debug.Log("u can ");
                    gameObject.transform.parent.GetComponent<SpriteRenderer>().color = GameManager.Instance.leYellow;
                }
            }
        }


        if (this.score >= 256)
        {
            this.IsTop = true;

          
        }
    }


    private IEnumerator StopTouch(GameObject first)
    {
        yield return new WaitForSeconds(0.15f);
        if (first != null)
            first.GetComponent<Square>().touched = false;
    }




    public void OnTriggerEnter2D(Collider2D other)
    {
        //Destroy on contact with center
        if (other.CompareTag("center") && gameObject.CompareTag("square"))
        {
           GameManager.Instance.Tops++;
           
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






