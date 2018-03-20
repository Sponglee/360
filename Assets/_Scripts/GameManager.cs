using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    

    [SerializeField]
    public GameObject wheel;
    public GameObject squarePrefab;
    public GameObject spotPrefab;
    public Transform wheelSquares;

    //prefab for controlling movement while falling
    GameObject squareSpawn = null;
    public int maxScore;
    public int expandScore;

    //Vertical transform of top spot
    public GameObject currentSpot;
    //next spot to turn green
    public int LastSpot { get; set; }

    //All the spots around the wheel
    public List<GameObject> spots;

    //spawn cooldown
    private float coolDown;

    // number of objects
    public int nBottom = 20;
    //Next square's score
    public Text nextScore;
    public static int next_score;

    //scores
    public int scores;
    public Text ScoreText;



    void Start()
    {
        //Maximum spawn is 8
        maxScore = 4;
        expandScore = 100;

        spots = new List<GameObject>();
        scores = 0;
        ScoreText.text = scores.ToString();

        //Random next score to appear
        next_score = (int)Mathf.Pow(2, Random.Range(1, 4));
        nextScore.text = next_score.ToString();

        //Initialize level (spots)
        GetSpots(nBottom);
    }


    void Update()
    {
        //if inside outer ring
        if (currentSpot.transform.childCount <= 5 && currentSpot.transform.GetChild(0).GetComponent<SpriteRenderer>().color != new Color32(255, 0, 0, 255)|| (currentSpot.transform.childCount == 6 && next_score == currentSpot.transform.GetChild(currentSpot.transform.childCount - 1).GetComponent<Square>().Score))
        {
            //turn left or right
            if (Input.GetMouseButtonUp(0) && SwipeManager.Instance.Direction == SwipeDirection.None && Time.time > coolDown)
            {
                //Cooldown for spawn 0.5sec
                coolDown = Time.time + 0.5f;

                //spawn a square
                squareSpawn = Instantiate(squarePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                squareSpawn.GetComponent<Square>().Score = next_score;
                //get score for next turn
                next_score = (int)Mathf.Pow(2, Random.Range(1, maxScore));
                nextScore.text = next_score.ToString();

            }
        }
      

        if (scores >= expandScore)
        {
            Expand();
            expandScore *= 2;
        }

            //Swipe manager
            if (SwipeManager.Instance.IsSwiping(SwipeDirection.Left) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //If square is falling - can't move
            if (squareSpawn != null && Mathf.Abs(squareSpawn.GetComponent<Rigidbody2D>().velocity.y) > 0.4)
            {
                Debug.Log("NO");
            }
            else
            {
                wheel.transform.Rotate(Vector3.forward, 360 / nBottom);
            
            }
                
        }
        else if (SwipeManager.Instance.IsSwiping(SwipeDirection.Right) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (squareSpawn != null && Mathf.Abs(squareSpawn.GetComponent<Rigidbody2D>().velocity.y) > 0.4)

            {
                Debug.Log("NO");
            }
            else
                wheel.transform.Rotate(Vector3.forward, -360 / nBottom);
        }

    }


    public void Merge(GameObject first)
    {
        int tmp = first.GetComponent<Square>().Score *= 2;
        if (tmp>= (int)Mathf.Pow(2, maxScore))
        {
            maxScore = (int)Mathf.Log(tmp, 2);
            
        }
        //first.GetComponent<SpriteRenderer>().color = new Color32(200, 200, 200, 255);
    }

    public Vector3 RandomCircle(Vector3 center, float radius, int a)
    {
        //Debug.Log(a);
        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    //Sets up spots for spawns
    public void GetSpots(int numberObjects)
    {
        float rad = wheel.transform.GetChild(0).GetComponent<CircleCollider2D>().radius;

        var center = wheel.transform.position;
        for (int i = 0; i < numberObjects; i++)
        {
            int a = 360 / numberObjects * i;
            var pos = RandomCircle(center, rad, a);
            GameObject tmp = Instantiate(spotPrefab, pos, Quaternion.LookRotation(Vector3.back));

            //open up 5 first spots for player
            if (i == numberObjects - 1 || i == 0 || i == 1 )
            {
                tmp.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
                //the end spot for expanding
                LastSpot =1;
            }

            tmp.name = i.ToString();
            tmp.transform.SetParent(wheel.transform.GetChild(0));
            tmp.transform.LookAt(center, Vector3.right);
            tmp.transform.Rotate(0, 90, 0);
            spots.Add(tmp);
            currentSpot = spots[0];
        }
    }

    //Checks for 3 in a row
    public void CheckRow(int spotIndex, int squareIndex, int checkScore)
    {
        //iterator for spots(more than 1 full circle)
        int count = 0;
        //iterator for list
        int index = 0;
        int row = 0;
        int maxTurns = nBottom+1;
        bool lapTwo = false;
        List<GameObject> rowObjs = new List<GameObject>();
      
        //Iterate through the circle more than 1 full circle ('count' elements)
        do
        {  
            if (index == nBottom)
            {
                index = 0;
                lapTwo = true;
            }
            if (spots[index].transform.childCount > squareIndex)
            {
                if (spots[index].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
                {  
                    rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                    row++;
                    {
                        if (lapTwo)
                            maxTurns++;
                    }
                }
                else
                {
                    if (row < 3)
                    {
                        rowObjs.Clear();
                        row = 0;
                    }
                }

            }
            else
            {
                if(row<3)
                {
                    row = 0;
                    rowObjs.Clear();
                }
            }
            index++;
            count++;

            if (count == maxTurns)
            {
                Pop(rowObjs, row);
                row = 0;
                rowObjs.Clear();
                count = 0;
                index = 0;
                maxTurns = nBottom + 1;
                break;
            }
        }
        while (count <= maxTurns); 
    }

    //Kill all adjacent squares
    public void Pop(List<GameObject> rowObjs, int row)
    {
        //if there're 3 in a row - BAM
        if (row >= 3)
        {
            foreach (GameObject rowObj in rowObjs)
            {
                Debug.Log(rowObj.transform.GetChild(0).GetComponentInChildren<Text>().text);

                //Update the score
                scores += rowObj.GetComponent<Square>().Score;
                ScoreText.text = scores.ToString();


                if (rowObj.transform.parent != null)
                {
                    rowObj.transform.position += new Vector3(0, 0, 10);
                    rowObj.transform.parent.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
                }
                   
                rowObj.transform.parent = null;
                rowObj.GetComponent<Collider2D>().isTrigger = true;

            }
            rowObjs.Clear();
        }
        rowObjs.Clear();
    }
    // Add more columns to the field
    public void Expand()
    {
        //Check if next spot is green and circle isnt full
        if (LastSpot != 0 && spots[LastSpot + 1].transform.GetChild(0).GetComponent<SpriteRenderer>().color != new Color32(0, 255, 0, 255) && LastSpot!=nBottom)
        {
            spots[GameManager.Instance.LastSpot + 1].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
            LastSpot += 1;
        }
        else
           LastSpot = 0;
    }
    

    public void GameOver()
    {
        if (currentSpot.transform.childCount == 6)
        {
            //full spot colors red and opens another one
            currentSpot.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
           
            Debug.Log("YAYAYAYAYAY");
        }
        int reds = 0;

        foreach (GameObject spot in spots)
        {
            if (spot.transform.GetChild(0).GetComponent<SpriteRenderer>().color == new Color32(255, 0, 0, 255))
            {
                reds++;
            }
        }
        if (reds == spots.Count && (next_score != currentSpot.transform.GetChild(currentSpot.transform.childCount - 1).GetComponent<Square>().Score))
        {
            nextScore.text = "GAME OVER";
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!GAMOVER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }






        ////Check for nearest same squares
        //if (spots[prevSpotIndex].transform.Find(squareIndex.ToString()) != null)
        //{
        //    left = spots[prevSpotIndex].transform.Find(squareIndex.ToString()).GetComponent<Square>().Score;
        //    Debug.Log(" ><>L<<>><> " + left + "<><><><><>  " + spots[prevSpotIndex].transform.Find(squareIndex.ToString()));
        //}

        //if (spots[nextSpotIndex].transform.Find(squareIndex.ToString()) != null)
        //{
        //    right = spots[nextSpotIndex].transform.Find(squareIndex.ToString()).GetComponent<Square>().Score;
        //    Debug.Log(" ><>R<<>><> " + right + "<><><><><>  " + spots[nextSpotIndex].transform.Find(squareIndex.ToString()));
        //}
        //if (right != 0 && left != 0)
        //{
        //    if (left == checkScore && checkScore == right)
        //    {
        //        Debug.Log("BAM!!!!!");
        //    }
        //}

    }







}