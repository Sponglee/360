using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour
{
   

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

    //Bool to avoid double x2 merge scores
    private bool doublingPriority = false;
    public bool DoublingPriority
    {
        get
        {
            return doublingPriority;
        }

        set
        {
            doublingPriority = value;
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

    [SerializeField]
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
    //Bool for bug fixing (check later)
    //[SerializeField]
    //private bool checkCoolDown=false;
    //public bool CheckCoolDown
    //{
    //    get
    //    {
    //        return checkCoolDown;
    //    }

    //    set
    //    {
    //        checkCoolDown = value;
    //    }
    //}

    private float checkTimer;

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
    private bool mergeCheck = false;
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
        SquareText.GetComponent<Outline>().effectColor = SquareStyleHolder.Instance.SquareStyles[index].TextColor;
    }
    //Gets Values from style script for each square
    public void ApplyStyle(int num)
    {
        switch (num)
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
    void Start()
    {
        //Apply theme

        SquareText.font = GameManager.Instance.fontPrefab;

        checkTimer = 4f;

        if (ExpandSpawn)
        {
            // gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();


            ApplyStyle(this.score);

        }
        else if(!ExpandSpawn && gameObject.CompareTag("square"))
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
    void FixedUpdate()
    {
        
        //0 score bug fix

        if (int.Parse(SquareText.text) == 0)
        {
            SquareText.text = this.score.ToString();

        }
       
       
        

        //Check if something is moving
        if (gameObject.CompareTag("square"))
        {
            curPos = gameObject.transform.localPosition;
            if (curPos == lastPos && gameObject.transform.parent != null)
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
        }
          

        if (this.gameObject.transform.parent != null && !this.gameObject.transform.parent.CompareTag("outer"))
        {
          
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
        else if (this.IsTop == true )
        {
            if (!gameObject.CompareTag("square"))
            {
                Debug.Log("HERE");
                gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.wheel.transform.position, Speed * Time.deltaTime);
            }
            else
            {
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                IsTop = false;
                AudioManager.Instance.PlaySound("256");
                //Debug.Log("YOSH");
                gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.wheel.transform.position, Speed * Time.deltaTime);
            }
           
        }
        else
        {
            Debug.Log("NOT YOSH");
            //GameManager.Instance.SomethingIsMoving = true;
            if (SquareTmpSquare != null)
                gameObject.transform.position = Vector2.MoveTowards(transform.position, squareTmpSquare.position, Speed * Time.deltaTime);
            else
            {
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                gameObject.transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.wheel.transform.position, Speed * Time.deltaTime);
                
            }
        }



        // Boundary
        if (Mathf.Abs(transform.position.y) > 100 || Mathf.Abs(transform.position.x) > 100)
        {
            Destroy(gameObject);
        }
        //reached tmpSquare
        else if (squareTmpSquare != null && transform.position == squareTmpSquare.position)
        {
            if (GameManager.Instance.checkObjs.Contains(gameObject))
            {
                //Debug.Log("reached TMPSQR " + squareTmpSquare.transform.parent.name);
            }
            Destroy(gameObject);
        }



    }


    private IEnumerator StopLeft()
    {
        yield return new WaitForSeconds(3f);
        if (gameObject.transform.parent != null && CheckLeftRight())
        {
            GameManager.Instance.checkObjs.Enqueue(gameObject);
            //checkCoolDown = true;
        }
        //else
        //    CheckCoolDown = false;
    }

    private bool CheckLeftRight()
    {
        //checkaround
       
            //if there's no start yet
            int index = int.Parse(gameObject.transform.parent.name);
            int firstIndex;
            int nextIndex;

            //check next left one after getting index-1
            if (index - 1 < 0)
            {
                firstIndex = GameManager.Instance.nBottom - 1;
            }
            else
                firstIndex = index - 1;

            //check next one after setting index+1
            if (index + 1 > GameManager.Instance.nBottom - 1)
            {
                nextIndex = 0;
            }
            else
                nextIndex = index + 1;



            //Check if left or right is available and == score
            if (GameManager.Instance.spots[nextIndex].transform.childCount > gameObject.transform.GetSiblingIndex() 
            && !gameObject.transform.parent.CompareTag("outer"))
            {
                //Debug.Log("ENUF " + GameManager.Instance.spots[nextIndex].transform.GetChild(gameObject.transform.GetSiblingIndex()) + " " + nextIndex + " : " + index  + " : " + firstIndex + " | " + GameManager.Instance.spots[nextIndex].transform.GetChild(gameObject.transform.GetSiblingIndex()).GetComponent<Square>().Score + " || " + this.score);
                if (GameManager.Instance.spots[nextIndex].transform.GetChild(gameObject.transform.GetSiblingIndex()).GetComponent<Square>().Score == this.score)
                {
                    //Debug.Log("RIGHT");
                    //Check again
                    return true;
                }
            }
             if (!gameObject.transform.parent.CompareTag("outer") && GameManager.Instance.spots[firstIndex].transform.childCount > gameObject.transform.GetSiblingIndex())
            {
            //Debug.Log("ENUF " + GameManager.Instance.spots[firstIndex].transform.GetChild(gameObject.transform.GetSiblingIndex()) + " " + nextIndex + " : " + index + " : " + firstIndex + " | " + GameManager.Instance.spots[firstIndex].transform.GetChild(gameObject.transform.GetSiblingIndex()).GetComponent<Square>().Score + " || " + this.score);
            if (GameManager.Instance.spots[firstIndex].transform.GetChild(gameObject.transform.GetSiblingIndex()).GetComponent<Square>().Score == this.score)
                {
                    //Debug.Log("LEFT"); 
                    //Check again
                    return true;
                }
            }

        return false;
    }











    private IEnumerator StopConsumable(GameObject other, GameObject gameObject = null, int spot = 0, int squareIndex = 0)
    {
      
        //Destoy bomb
       

        if (gameObject != null && gameObject.CompareTag("drill"))
        {
            Instantiate(GameManager.Instance.explosionPref, other.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);

        }
        if (gameObject != null && other.CompareTag("bomb"))
        {
           
            Instantiate(GameManager.Instance.explosionPref, other.transform.position, Quaternion.identity);
            
            yield return new WaitForSeconds(0.2f);

            Destroy(other.gameObject);
            //if there's no start yet
            int index = spot;
            int firstIndex;
            int nextIndex;

            //check next left one after getting index-1
            if (index - 1 < 0)
            {
                firstIndex = GameManager.Instance.nBottom - 1;
            }
            else
                firstIndex = index - 1;

            //check next one after setting index+1
            if (index + 1 > GameManager.Instance.nBottom - 1)
            {
                nextIndex = 0;
            }
            else
                nextIndex = index + 1;

            if (GameManager.Instance.spots[firstIndex].transform.childCount > squareIndex)
            {
                GameObject firstTmp = GameManager.Instance.spots[firstIndex].transform.GetChild(squareIndex).gameObject;
                Instantiate(GameManager.Instance.explosionPref, firstTmp.transform.position, Quaternion.identity);
                Destroy(firstTmp);
            }
            if (GameManager.Instance.spots[nextIndex].transform.childCount > squareIndex)
            {
                GameObject nextTmp = GameManager.Instance.spots[nextIndex].transform.GetChild(squareIndex).gameObject;
                Instantiate(GameManager.Instance.explosionPref, nextTmp.transform.position, Quaternion.identity);
                Destroy(nextTmp);
            }

            Destroy(gameObject);
        }


        GameManager.Instance.SomethingIsMoving = false;
    }




    // DOUBT IF NEEDED SEE FIXED UPDATE
    public void OnCollisionEnter2D(Collision2D other)
    {

        //Power up collisions
        #region Powerup interaction



        if (other.gameObject.CompareTag("square") && this.gameObject.CompareTag("drill"))
        {
            GameManager.Instance.Merge(gameObject, null, other.gameObject);
            StartCoroutine(StopConsumable(other.gameObject, gameObject));
        }
        else if (other.gameObject.CompareTag("square") && this.gameObject.CompareTag("bomb"))
        {
            GameManager.Instance.Merge(gameObject, null, null);
            
            StartCoroutine(StopConsumable(gameObject, other.gameObject, int.Parse(GameManager.Instance.currentSpot.name), other.gameObject.transform.GetSiblingIndex()));
           
        }

       //Move past spot
        if (other.gameObject.CompareTag("spot") && this.gameObject.CompareTag("drill"))
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.transform.SetParent(null);
            this.IsTop = true;
        }
        else if (other.gameObject.CompareTag("spot") && this.gameObject.CompareTag("bomb"))
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.transform.SetParent(null);
            this.IsTop = true;
        }


        #endregion


        if (other.gameObject.CompareTag("spot") && this.gameObject.CompareTag("square"))
        {
            Debug.Log(gameObject.transform.parent.name + "( " + score + ")");
            //reset speed back
            this.speed = 10f;

            //for column checkrow
            if (gameObject.transform.parent != null /*&& CheckLeftRight()*/)
            {
                if (CheckLeftRight())
                {
                 
                    GameManager.Instance.checkObjs.Enqueue(gameObject);
                    //Debug.Log("DA" + GameManager.Instance.checkObjs.Count);
                }
                else
                {
                    GameManager.Instance.CheckAbove(int.Parse(gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex());
                    Debug.Log("above spot " + this.Score);
                    if (this.IsSpawn)
                    {
                        GameManager.Instance.ExpandMoves();
                        GameManager.Instance.ResetExpand(gameObject);
                    }

                }
            
               
                AudioManager.Instance.PlaySound("bump");
            }



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

                GameManager.Instance.Merge(gameObject, null, other.gameObject);
                //Debug.Log("MRG");

            }
            else if (this.score != other.gameObject.GetComponent<Square>().Score)
            {
                Debug.Log(gameObject.transform.parent.name + "( " + score + ")");
                //reset speed back
                this.speed = 10f;

                //for column checkrow
                if (gameObject.transform.parent != null /*&& CheckLeftRight()*/)
                {
                    if (CheckLeftRight())
                    {
                        //Debug.Log("DA");
                        GameManager.Instance.checkObjs.Enqueue(gameObject);

                    }
                    else
                    {
                        GameManager.Instance.CheckAbove(int.Parse(gameObject.transform.parent.name), gameObject.transform.GetSiblingIndex());
                        Debug.Log("above square " + this.score);
                        if (this.IsSpawn)
                        {
                            GameManager.Instance.ExpandMoves();
                            GameManager.Instance.ResetExpand(gameObject);
                        }
                    }

                    Debug.Log("spot " + this.Score);
                    AudioManager.Instance.PlaySound("bump");
                    //checkCoolDown = true;
                }


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
                gameObject.transform.parent.GetComponent<SpriteRenderer>().color = GameManager.Instance.leGreen;
            }
            else if (gameObject.transform.parent.childCount == 4)
            {
                gameObject.transform.parent.GetComponent<SpriteRenderer>().color = GameManager.Instance.leYellow;
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
        if (other.CompareTag("center") /*&& (gameObject.CompareTag("square")*/)
        {
            if (this.score == 256)
                GameManager.Instance.Tops++;

            //Debug.Log("destroy this");
            Destroy(gameObject);
        }


       


    }




}






