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
    public int nBottom = 8;

    //Next square's score
    public Text nextScore;
    public static int next_score;

    //scores
    public int scores;
    public Text ScoreText;

    // Obj list for pop checkrow
    List<GameObject> rowObjs;


    void Start()
    {
        //Maximum spawn is 8
        maxScore = 3;
        expandScore = 100;

        spots = new List<GameObject>();
        scores = 0;
        ScoreText.text = scores.ToString();

        //Random next score to appear (2^3 max <-------)
        next_score = (int)Mathf.Pow(2, Random.Range(1, 4));
        nextScore.text = next_score.ToString();

        //Initialize level (spots)
        GetSpots(nBottom);

        
        rowObjs = new List<GameObject>();
    }


    void Update()
    {

        //if inside outer ring and not blocked by extend and is not red   OR is the same score as next one and not blocked
        if (currentSpot.transform.childCount <= 5 && currentSpot.transform.GetChild(0).GetComponent<SpriteRenderer>().color != new Color32(255, 0, 0, 255) 
                        || (currentSpot.transform.childCount == 6 && next_score == currentSpot.transform.GetChild(currentSpot.transform.childCount - 1).GetComponent<Square>().Score) && !currentSpot.GetComponent<Spot>().Blocked)
        {
            //turn left or right
            if (Input.GetMouseButtonUp(0) && SwipeManager.Instance.Direction == SwipeDirection.None && Time.time > coolDown)
            {
                //Cooldown for spawn 0.5sec
                coolDown = Time.time + 0.5f;

                //spawn a square
                squareSpawn = Instantiate(squarePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                squareSpawn.GetComponent<Square>().Score = next_score;
                //get score for next turn (non-inclusive)
                next_score = (int)Mathf.Pow(2, Random.Range(1,maxScore+1));
                nextScore.text = next_score.ToString();

            }
        }
      

        if (scores >= expandScore)
        {
            Expand();
            expandScore += expandScore/2;
         
        }

            //Swipe manager
            if (SwipeManager.Instance.IsSwiping(SwipeDirection.Left) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //If square is falling - can't move
            if (squareSpawn != null && Mathf.Abs(squareSpawn.GetComponent<Rigidbody2D>().velocity.y) > 0.4)
            {
                //Debug.Log("NO");
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
                //Debug.Log("NO");
            }
            else
                wheel.transform.Rotate(Vector3.forward, -360 / nBottom);
        }

    }


    public void Merge(GameObject first)
    {
        int tmp = first.GetComponent<Square>().Score *= 2;
        Debug.Log(tmp);
       
        if (tmp > (int)Mathf.Pow(2, maxScore))
        {
            //maxScore = (int)Mathf.Log(tmp, 2);
         
            //UNBLOCK
            //Expand(1);
            //first.GetComponent<Collider2D>().isTrigger = true;
        }
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
            //last spot for expanding 
            LastSpot = nBottom;                             //maybe randomize this??

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
        rowObjs.Clear();
       
        //iterator for list
        int index = 0;
        int count = 0;
        int maxTurns = nBottom + 1;
        bool lapTwo = false;

        int firstIndex=0;
        int nextIndex = 0;
        bool fill = false;
        //Iterate through the circle more than 1 full circle ('count' elements)


        do
        {


            if (index == nBottom)
            {
                index = 0;
                lapTwo = true;
            }

            Debug.Log("index " + index + "count " + spots[index].transform.childCount + "square " + squareIndex);

            //if there's square with same squareIndex
            if (spots[index].transform.childCount > squareIndex)
            {
                //if its score is the same
                if (spots[index].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
                {
                    //if there we're at index 0
                    Debug.Log("here");
                    if (index - 1 < 0)
                    {
                        firstIndex = nBottom - 1;
                    }
                    else
                        firstIndex = index - 1;


                    Debug.Log("here " + firstIndex + " spot " + spots[firstIndex].name + "squareIndex " + spots[firstIndex].transform.childCount);
                    // if there's no object to the left and no objs yet    ADDING FIRST ONE
                    if (rowObjs.Count == 0)
                    {
                        //if there's something to the left
                        if (spots[firstIndex].transform.childCount < squareIndex + 1)
                        {

                            Debug.Log("here");
                            rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);

                        }
                        else
                        {
                            if (spots[firstIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score != checkScore)
                            {
                                Debug.Log("here");
                                rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                            }
                        }
                    }
                    else if (rowObjs.Count != 0 && !fill)
                    {
                        rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                    }

                    if (rowObjs.Count != 0)
                    {
                        Debug.Log("here");
                        if (index + 1 >= nBottom - 1)
                        {
                            nextIndex = 0;
                        }
                        else
                            nextIndex = index + 1;


                       
                        Debug.Log("here");
                        //there's something to the right
                        if (spots[nextIndex].transform.childCount > squareIndex)
                        {
                            if (spots[nextIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
                            {
                                if (lapTwo)
                                    maxTurns++;
                                
                                index++;
                                count++;
                                continue;
                            }
                            else
                            {
                                if (rowObjs.Count < 3)
                                {
                                    rowObjs.Clear();
                                }
                                fill = true;
                                index++;
                                count++;
                                continue;

                            }
                        }
                        else
                        {
                            if (rowObjs.Count < 3)
                            {
                                rowObjs.Clear();
                                
                            }
                            fill = true;
                            index++;
                            count++;
                            continue;



                        }

                    }
                }
            }
            else
            {
                index++;
                count++;
                continue;
            }

            Debug.Log("index " + index);
            index++;
            count++;
        }
        while (count <= maxTurns);
        Pop(rowObjs);
        index = 0;



















        //do
        //{
          
        //    //2nd run 
        //    if (index == nBottom)
        //    {
        //        index = 0;
        //        lapTwo = true;
        //    }
        //    //if threre's square on this row
        //    if (spots[index].transform.childCount > squareIndex)
        //    {
        //        if (spots[index].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore && !fill)
        //        {
        //            firstIndex = index;
        //            rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);

        //            if (lapTwo)
        //                maxTurns++;
        //        }
        //        else if (fill && spots[index].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
        //        {
        //            rowObjs.Clear();
        //            fill = false;
        //            rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
        //           
        //        }
                
        //        else
        //        {
        //            if (lapTwo)
        //            {
        //                break;
        //            }
        //            else if (rowObjs.Count<3)
        //            {
        //                rowObjs.Clear();
                       
        //            }
        //            else if (rowObjs.Count>=3)
        //            {
        //                if (firstIndex - 1 < 0)
        //                    firstIndex = nBottom;
        //                if(spots[firstIndex - 1].transform.childCount > squareIndex && spots[firstIndex - 1].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
        //                {
        //                    fill = true;
        //                }
        //                else break;
                        
        //            }
                       

        //        }

        //    }
        //    else
        //    {
        //        if (lapTwo)
        //        {
        //            break;
        //        }
        //        else if (rowObjs.Count<3)
        //        {
        //            rowObjs.Clear();
                    
        //        }
        //        else if (rowObjs.Count>=3)
        //        {
        //            fill = true;
        //        }
                    

        //    }
        //    index++;
        //    count++;

        //    if (count == maxTurns)
        //    {


        //        break;
        //    }
        //}
        //while (count <= maxTurns);

      
        //if (rowObjs.Count >= 3)
        //    Pop(rowObjs);
        //count = 0;
        //index = 0;
        //maxTurns = nBottom + 1;
    }

    //Kill all adjacent squares
    public void Pop(List<GameObject> rowObjs)
    {
            foreach (GameObject rowObj in rowObjs)
            {
                //Update the score
                scores += rowObj.GetComponent<Square>().Score;
                ScoreText.text = scores.ToString();

                if (rowObj.transform.parent != null)
                {
                    rowObj.transform.position += new Vector3(0, 0, 10);
                    if(!rowObj.transform.parent.GetComponent<Spot>().Blocked)
                    {
                        rowObj.transform.parent.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
                    }
                   
                }
                   
                rowObj.transform.parent = null;
                rowObj.GetComponent<Collider2D>().isTrigger = true;

            }
            rowObjs.Clear();
        
       
    }
    // Add more columns to the field
    public void Expand(int retract=0)
    {
        //Check if next spot is green and circle isnt full
        if (LastSpot != 0 && spots[LastSpot - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().color != new Color32(255, 0, 0, 255) && retract == 0)
        {
            spots[LastSpot - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
            spots[LastSpot - 1].GetComponent<Spot>().Blocked = true;
            LastSpot -= 1;
          
        }
        else if (LastSpot != nBottom && spots[LastSpot].transform.GetChild(0).GetComponent<SpriteRenderer>().color == new Color32(255, 0, 0, 255) && retract != 0 && spots[LastSpot]!=null)
        {
           
            spots[LastSpot].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
            spots[LastSpot].GetComponent<Spot>().Blocked = false;

            LastSpot += 1;
        }
        else
            LastSpot = nBottom;
    }
    

    public void GameOver()
    {
        if (currentSpot.transform.childCount == 6)
        {
            //full spot colors red and opens another one
            currentSpot.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
           
          
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






    }







}