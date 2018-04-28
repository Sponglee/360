using System.Collections;
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
    // toggle for further pop first
    [SerializeField]
    private bool pewPriority = false;
    public bool PewPriority
    {
        get
        {
            return pewPriority;
        }

        set
        {
            pewPriority = value;
        }
    }
    //for simultanious checks
    [SerializeField]
    private bool stopped = false;

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


    [SerializeField]
    private bool IsMoving = false;
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
            case 512:
                ApplyStyleFromHolder(8);
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

    private void Update()
    {
      
    }

    // Update is called once per frame
    void FixedUpdate() {

        //Check if something is moving
        curPos = gameObject.transform.localPosition;
        if (curPos == lastPos)
        {
            IsMoving = false;
            GameManager.Instance.SomethingIsMoving = false;
        }
        else
        {
            IsMoving = true;
            GameManager.Instance.SomethingIsMoving = true;
        }
        lastPos = curPos;



        //if (GameManager.Instance.SomethingIsMoving)
        //{
        //    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        //}
        //else
        //{
        //    ApplyStyle(this.score);
        //}





        //for expandMoves
        if (this.pewPriority)
        {
            this.IsSpawn = false;
        }

        // if this one stopped popping - try again
        if (stopped)
        {
            stopped = false;
            GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score, this.gameObject);
            return;
        }
        
         
            
        if (this.gameObject.transform.parent != null)
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




            //Call checkRow or Merge
            if (CheckAround && gameObject.transform.position == GameManager.Instance.spawns[int.Parse(gameObject.transform.parent.name)].transform.GetChild(gameObject.transform.GetSiblingIndex()).position)
            {
            
                int mergeIndex = gameObject.transform.GetSiblingIndex();

                //Check for same square above
                if ((mergeIndex + 1) < gameObject.transform.parent.childCount)
                {
                    if (gameObject.transform.parent.GetChild(mergeIndex + 1).gameObject.GetComponent<Square>().Score == gameObject.GetComponent<Square>().Score
                        && !GameManager.Instance.SomethingIsMoving)
                    {
                        //Merge started
                        //Debug.Log("Above");
                        GameManager.Instance.MergeInProgress = true;
                        //for ExtendMoves
                        if (this.IsSpawn)
                        {
                            this.IsSpawn = false;
                        }
                        MergeCheck = true;
                        GameManager.Instance.Merge(gameObject.transform.parent.GetChild(mergeIndex+1).gameObject, gameObject);
                        AudioManager.Instance.PlaySound("stick");
                    }
                }

                // Check for same square below
                if ((mergeIndex - 1) >= 0)
                {
                    if (gameObject.transform.parent.GetChild(mergeIndex - 1).gameObject.GetComponent<Square>().Score == gameObject.GetComponent<Square>().Score 
                        && !GameManager.Instance.SomethingIsMoving)
                    {
                        //Merge started
                        //Debug.Log("DowN");
                        GameManager.Instance.MergeInProgress = true;
                         if (this.IsSpawn)
                        {
                            this.IsSpawn = false;
                        }
                        MergeCheck = true;
                        GameManager.Instance.Merge(gameObject, gameObject.transform.parent.GetChild(mergeIndex - 1).gameObject);
                        AudioManager.Instance.PlaySound("stick");
                    }
                }

                //if something moved - wait with check trigger until it doesnt
                if(!GameManager.Instance.SomethingIsMoving)
                    CheckAround = false;

                //wait for merge to finish - then checkrow
                if (!GameManager.Instance.SomethingIsMoving && !IsMerging && !MergeCheck && !GameManager.Instance.MergeInProgress && !GameManager.Instance.CheckInProgress && !pewPriority)
                {
                   
                        int firstSpot;
                        int nextSpot;


                        //one to the left
                        if (int.Parse(transform.parent.name) - 1 < 0)
                        {
                            firstSpot = GameManager.Instance.nBottom - 1;
                        }
                        else
                            firstSpot = int.Parse(transform.parent.name) - 1;

                        //check next left one after getting index-1
                        if (int.Parse(transform.parent.name) + 1 >= GameManager.Instance.nBottom)
                        {
                            nextSpot = 0;
                        }
                        else
                            nextSpot = int.Parse(transform.parent.name) + 1;

                        //if there's pewPriority near - don't CheckRow
                        if ((GameManager.Instance.spots[nextSpot].transform.childCount > transform.GetSiblingIndex()
                            && GameManager.Instance.spots[nextSpot].transform.GetChild(transform.GetSiblingIndex()).GetComponent<Square>().pewPriority)

                            || (GameManager.Instance.spots[firstSpot].transform.childCount > transform.GetSiblingIndex()
                            && GameManager.Instance.spots[firstSpot].transform.GetChild(transform.GetSiblingIndex()).GetComponent<Square>().pewPriority))
                        {
                            StartCoroutine(StopCheckAround());
                        }
                        else
                        {

                            GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score, this.gameObject);
                            
                        }


                        //Check GameOver
                        GameManager.Instance.GameOver();
                        MakeItGreen();
                    
                   
                }
                //else if(GameManager.Instance.SomethingIsMoving)
                //{
                //    //if something id
                //    Debug.Log("REEEEEEEEEEEEEEEEEEEEEE");
                //    StartCoroutine(StopPew());
                //}

            }



        }
        // 256 square to center
        //else if (this.IsTop == true)
        //{
        //    AudioManager.Instance.PlaySound("256");
        //    gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.wheel.transform.position, Speed * Time.deltaTime);
        //}
        else
        {
            GameManager.Instance.SomethingIsMoving = true;
            if ( SquareTmpSquare != null)
                gameObject.transform.position = Vector2.MoveTowards(transform.position, squareTmpSquare.position, Speed * Time.deltaTime);
            else
                gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.wheel.transform.position, Speed * Time.deltaTime);
        }



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



    private IEnumerator StopCheckAround()
    {
        Debug.Log("STOP");
        yield return new WaitForSeconds(0.2f);
        stopped = true;


    }

    //    //Make it green again and drop 256
    private void MakeItGreen()
    {
       
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
    //// DOUBT IF NEEDED SEE FIXED UPDATE
    //public void OnCollisionEnter2D(Collision2D other)
    //{


    //    if (other.gameObject.CompareTag("spot"))
    //    {
    //        // Debug.Log(" SQUARE " + this.Score + " " + gameObject.transform.parent.name + ":" + gameObject.transform.GetSiblingIndex());

    //        //reset speed back
    //        this.speed = 10f;

    //        //for column checkrow
    //        GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score, this.gameObject);
    //        this.NotTouched = true;


    //    }
    //    //other square
    //    if (other.gameObject.CompareTag("square") && gameObject.CompareTag("square") && !this.touched /*&& gameObject.transform.GetSiblingIndex() > other.gameObject.transform.GetSiblingIndex()*/)
    //    {
    //        //make sure checks only one of 2 collisions (one that is not touched
    //        other.gameObject.GetComponent<Square>().touched = true;

    //        if (this.score == other.gameObject.GetComponent<Square>().Score)
    //        {
    //           // Debug.Log("SCORE : " + gameObject.GetComponent<Square>().Score + " to " + other.gameObject.GetComponent<Square>().Score);
    //            //if spawned by player and pops - no moves 
    //            if (this.IsSpawn)
    //            {
    //                this.IsSpawn = false;
    //            }



    //            GameManager.Instance.Merge(gameObject, other.gameObject);
    //            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();


    //        }
    //        else if (this.score != other.gameObject.GetComponent<Square>().Score)
    //        {
    //            // Debug.Log(" SQUARE " + this.Score + " " + gameObject.transform.parent.name + ":" + gameObject.transform.GetSiblingIndex() );

    //            //reset speed back
    //            this.speed = 10f;

    //            //for column checkrow
    //            GameManager.Instance.CheckRow(int.Parse(this.gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex(), score, this.gameObject);
    //            this.NotTouched = true;


    //            //reset Touched bool 
    //            StartCoroutine(StopTouch(other.gameObject));

    //            //Check GameOver
    //            GameManager.Instance.GameOver();




    //            //Debug.Log("!!SCORE : " + gameObject.GetComponent<Square>().Score + " to " + other.gameObject.GetComponent<Square>().Score);

    //        }

    //        gameObject.name = gameObject.transform.GetSiblingIndex().ToString();
    //        //other.gameObject.GetComponent<Square>().touched = false;

    //        //Check for boops

    //    }

    //    //Make it green again
    //    if (gameObject.transform.parent != null && gameObject.CompareTag("square"))
    //    {

    //        if (gameObject.transform.parent.childCount < 5)
    //        {
    //            if (gameObject.transform.parent.GetComponent<Spot>().Blocked == false)
    //            {
    //                //Debug.Log("u can ");
    //                gameObject.transform.parent.GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
    //            }
    //        }
    //    }


    //    if (this.score >= 256)
    //    {
    //        this.IsTop = true;

    //        this.gameObject.transform.parent = null;
    //        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    //    }
    //}


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






