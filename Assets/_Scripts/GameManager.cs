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
    //for random expand spawns
    GameObject randSpawn = null;

    //checker for player spawn
    private bool isSpawn = false;
    public int maxScore;
    [SerializeField]
    public int scoreUpper;
    [SerializeField]
    public int expandMoves;
    [SerializeField]
    private int moves;

    //Vertical transform of top spot
    public GameObject currentSpot;
    //next spot to turn green
    public int LastSpot { get; set; }

    //All the spots around the wheel
    public List<GameObject> spots;

    //spawn cooldown
    private float coolDown;

    // number of objects
    public int nBottom;

    //Next square's score
    public Text nextScore;
    public Text upper;
    public Text nextShrink;
    public static int next_score;
    public Slider slider;

    //scores
    public int scores;
    public Text ScoreText;

    // Obj list for pop checkrow
    List<GameObject> rowObjs;


    void Start()
    {
        //Apply all the numbers 
        maxScore = 3;
        expandMoves = 3;
        moves = 0;
        scoreUpper = (int)Mathf.Pow(2, maxScore);
        nBottom = 10;
        spots = new List<GameObject>();
        scores = 0;
        ScoreText.text = scores.ToString();
        upper.text = string.Format("upper: {0}", scoreUpper);
        nextShrink.text = string.Format("next shrink: {0}", expandMoves - moves);
        slider.value = (expandMoves - moves) / expandMoves;
        

        //Random next score to appear (2^3 max <-------)
        next_score = (int)Mathf.Pow(2, Random.Range(1, 4));
        nextScore.text = next_score.ToString();

        //Initialize level (spots)
        GetSpots(nBottom);

        
        rowObjs = new List<GameObject>();
    }


    void Update()
    {

        //if inside outer ring and not blocked by extend and is not red   OR is the same score as next one
        if (currentSpot.transform.childCount <= 5 && currentSpot.transform.GetChild(0).GetComponent<SpriteRenderer>().color != new Color32(255, 0, 0, 255)
                        || (currentSpot.transform.childCount == 6 && next_score == currentSpot.transform.GetChild(currentSpot.transform.childCount - 1).GetComponent<Square>().Score) /*&& !currentSpot.GetComponent<Spot>().Blocked*/)
        {
            //turn left or right and if randomSpawn isnt moving
            if (randSpawn != null && Mathf.Abs(randSpawn.GetComponent<Rigidbody2D>().velocity.y) > 0.4)
            {
                //Debug.Log("No");
            }
            else if (Input.GetMouseButtonUp(0) && SwipeManager.Instance.Direction == SwipeDirection.None && Time.time > coolDown)
            {
                //Cooldown for spawn 0.5sec
                coolDown = Time.time + 0.5f;

                //spawn a square
                squareSpawn = Instantiate(squarePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                squareSpawn.GetComponent<Square>().Score = next_score;
                //get score for next turn (non-inclusive)
                next_score = (int)Mathf.Pow(2, Random.Range(1,maxScore+1));
                nextScore.text = next_score.ToString();
                squareSpawn.GetComponent<Square>().IsSpawn = true;
            }
        }
      

        if (moves > expandMoves-1)
        {
            Expand();
            moves = 0;
            slider.value = 1;

            //expandMoves += expandMoves/2;
            nextShrink.text = string.Format("next shrink: {0}", expandMoves - moves);
            slider.value = (float)(expandMoves - moves) / expandMoves;
            //Debug.Log(" exp-moves " + (expandMoves - moves) + " expM " + expandMoves + "||||| " + slider.value);

        }

            //Turn left
            if (SwipeManager.Instance.IsSwiping(SwipeDirection.Left) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //if squares are falling - can't move
                if (squareSpawn != null && Mathf.Abs(squareSpawn.GetComponent<Rigidbody2D>().velocity.y) > 0.4)
                {
                
                    //Debug.Log("NO");
                }
                //if randomSpawn is falling - can't move
                else if (randSpawn != null && Mathf.Abs(randSpawn.GetComponent<Rigidbody2D>().velocity.y) > 0.4)
                {
                   // Debug.Log("NO");
                }
                else
                {
                    
                  
                        wheel.transform.Rotate(Vector3.forward, 360 / nBottom);
            
                }
                
            }
            //Turn right
            else if (SwipeManager.Instance.IsSwiping(SwipeDirection.Right) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                //if squareSpawn is falling - can't move
                if (squareSpawn != null && Mathf.Abs(squareSpawn.GetComponent<Rigidbody2D>().velocity.y) > 0.4)
                {
                    //Debug.Log("NO");
                }
                //if randomSpawn is falling - can't move
                else if (randSpawn != null && Mathf.Abs(randSpawn.GetComponent<Rigidbody2D>().velocity.y) > 0.4)
                {
                   // Debug.Log("NO");
                }
                else
                {
                        wheel.transform.Rotate(Vector3.forward, -360 / nBottom);
                }
               
            }
       
    }


    public void Merge(GameObject first)
    {
        int tmp = first.GetComponent<Square>().Score *= 2;
        first.GetComponent<Square>().ApplyStyle(tmp);

        if (tmp > scoreUpper)
        {
            scoreUpper *=2;
            Instance.upper.text = string.Format("upper: {0}", scoreUpper);

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
        int index = spotIndex;
        int i = 1;
        int j = 1;
        int count = 0;
        int maxTurns = nBottom + 1;
        bool lapTwo = false;

        //index of start of rowObjs (outside nbottom numbers to be safe)
        int startIndex = nBottom + 10;
        int endIndex = nBottom + 11;
        int firstIndex = 0;
        int nextIndex = 0;
        bool fill = false;
        //Iterate through the circle more than 1 full circle ('count' elements)

        GameObject tmpSquare = spots[spotIndex].transform.GetChild(squareIndex).gameObject;

        //add placed square to rowOBjs
        rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);


        do
        {
           


            //if there's no start yet
            if (startIndex > nBottom)
            {
                //grab spot to the left
                index = index - i;

                                    //passing through 0
                                    if (index - i < 0)
                                    {
                                        index = nBottom - i;
                                    }

                                    //if there we're at index 0 i spots to the left
                                    if (index - 1 < 0)
                                    {
                                        firstIndex = nBottom - 1;
                                    }
                                    else
                                        firstIndex = index - 1;


                //if there's square with same squareIndex
                if (spots[index].transform.childCount > squareIndex)
                {
                    //if its score is the same
                    if (spots[index].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
                    {
                        //if there's nothing to the left
                        if (spots[firstIndex].transform.childCount < squareIndex + 1)
                        {
                            rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                            //if never set up yet
                            if (startIndex > nBottom)
                            {
                                // start pf a row
                                startIndex = index;
                                index = spotIndex;
                                continue;
                            }

                        }
                        else
                        {
                            //or there's not that score
                            if (spots[firstIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score != checkScore)
                            {
                                rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                                //if never set up yet
                                if (startIndex > nBottom)
                                {
                                    startIndex = index;
                                    index = spotIndex;
                                    continue;
                                }
                            }
                            else if (spots[firstIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
                            {
                                startIndex = nBottom + 10;
                                i++;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        //if score isnt same - move back to spotIndex and continue
                        startIndex = spotIndex;
                        index = spotIndex;
                        continue;
                    }
                }
                else
                {
                    //if score isnt same - move back to spotIndex and continue
                    startIndex = spotIndex;
                    index = spotIndex;
                    continue;
                }

            }
            //if we found left end
            else if (startIndex < nBottom)
            {

                index = index + j;

                        //passing through nBottom (0)
                        if (index +j == nBottom)
                        {
                            index = 0;
                        }

                        //if there we're at index 0 i spots to the left
                        if (index - 1 < 0)
                        {
                            firstIndex = nBottom - 1;
                        }
                        else
                            firstIndex = index - 1;




                //if there's square with same squareIndex
                if (spots[index].transform.childCount > squareIndex)
                {
                    //if its score is the same
                    if (spots[index].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
                    {
                        //if there's nothing to the right
                        if (spots[nextIndex].transform.childCount < squareIndex + 1)
                        {
                            rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                            //if never set up yet
                            if (endIndex > nBottom)
                            {
                                // start pf a row
                                endIndex = index;
                                break;
                            }

                        }
                        else
                        {
                            //or there's not that score
                            if (spots[nextIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score != checkScore)
                            {
                                rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                                //if never set up yet
                                if (startIndex > nBottom)
                                {
                                    endIndex = index;
                                    break;
                                }
                            }
                            else if (spots[nextIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
                            {
                                rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                                j++;
                            }
                        }
                    }
                    else
                    {
                        //if score isnt same - move back to spotIndex and continue
                        endIndex = spotIndex;
                        break;
                    }
                }
                else
                {
                    //if score isnt same - move back to spotIndex and continue
                    endIndex = spotIndex;
                    break;
                }
            }


            
           
        }
        while (endIndex > nBottom + 1);

        if (rowObjs.Count<3)
        {
            Debug.Log(endIndex - startIndex + "  " + rowObjs.Count);
            rowObjs.Clear();
            moves++;
        }
        else
        {

            Pop(rowObjs);
            index = 0;


        }
       











        //do
        //{

        //    //Second run starting 
        //    if (index == nBottom)
        //    {
        //        index = 0;
                
        //    }

        //    if (index == spotIndex && count >0)
        //    {
        //        lapTwo = true;
        //    }

        //    //if there's square with same squareIndex
        //    if (spots[index].transform.childCount > squareIndex)
        //    {
        //        //if its score is the same
        //        if (spots[index].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
        //        {
        //            //if there we're at index 0
        //            if (index - 1 < 0)
        //            {
        //                firstIndex = nBottom - 1;
        //            }
        //            else
        //                firstIndex = index - 1;


                  
        //            // if there's no object to the left and no objs yet    ADDING FIRST ONE
        //            if (rowObjs.Count == 0)
        //            {
        //                //if there's something to the left
        //                if (spots[firstIndex].transform.childCount < squareIndex + 1)
        //                {
        //                    rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
        //                    //if never set up yet
        //                    if (startIndex > nBottom)
        //                    {
        //                        startIndex = firstIndex;
        //                    }
                            
        //                }
        //                else
        //                {
        //                    if (spots[firstIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score != checkScore)
        //                    {
        //                        rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
        //                        //if never set up yet
        //                        if (startIndex > nBottom)
        //                        {
        //                            startIndex = firstIndex;
        //                        }
        //                    }
        //                }
        //            }
        //            else if (rowObjs.Count != 0 && !fill)
        //            {
        //                rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
        //            }

        //            if (rowObjs.Count != 0)
        //            {
        //                if (index + 1 > nBottom - 1)
        //                {
        //                    nextIndex = 0;
        //                }
        //                else
        //                    nextIndex = index + 1;

        //                //there's something to the right
        //                if (spots[nextIndex].transform.childCount > squareIndex)
        //                {
        //                    if (spots[nextIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
        //                    {
        //                        if (lapTwo)
        //                            maxTurns++;
                                
        //                        index++;
        //                        count++;
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        if (rowObjs.Count < 3 && startIndex < nBottom + 1)
        //                        {
        //                            rowObjs.Clear();
                                    
        //                            //exit the loop if less than 3
        //                            if (Mathf.Abs(startIndex - index) < 2)
        //                                break;
        //                        }
        //                        else
        //                        {
        //                            //if filled row - exit
        //                            fill = true;
        //                            break;
        //                        }
                                    
        //                        index++;
        //                        count++;
        //                        continue;

        //                    }
        //                }
        //                else
        //                {
        //                    if (rowObjs.Count < 3 && startIndex<nBottom+1)
        //                    {
                                
        //                        rowObjs.Clear();
                              
        //                        //exit the loop if less than 3

        //                        if (Mathf.Abs(startIndex - index) < 2)
        //                            break;
        //                    }
        //                    else
        //                        fill = true;
        //                    index++;
        //                    count++;
        //                    continue;



        //                }

        //            }
        //        }
        //    }
        //    else
        //    {
        //        index++;
        //        count++;
        //        continue;
        //    }
        //    index++;
        //    count++;
        //}
        //while (count <= maxTurns);
        ////reset it back to out of reach number
        //startIndex = nBottom + 10;
        
       

    }

    // update moves
    public void Moves()
    {
        moves++;
        nextShrink.text = string.Format("next shrink: {0}", expandMoves - moves);
        slider.value = (float)(expandMoves - moves) / expandMoves;
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
    public void Expand()
    {
        int rng = 0;
        int randScore = (int)Mathf.Pow(2, Random.Range(1, maxScore + 1));

        // While Spot[rng] is not current spot - spawn randomSpawn at random spots
        do
        {
         
            rng = Random.Range(0, nBottom - 1);
            

            if (spots[rng].transform.childCount <6)
            {
                Debug.Log("SPAWN");
                //spawn a square at random spot with random score
                randSpawn = Instantiate(squarePrefab, spots[rng].transform.position + new Vector3(50, 0, 0), Quaternion.identity);
                randSpawn.GetComponent<Square>().ExpandSpawn = true;
                randSpawn.GetComponent<Square>().Score = randScore;
                randSpawn.transform.SetParent(spots[rng].transform);
                randSpawn.name = randSpawn.transform.GetSiblingIndex().ToString();
                //spawn at the same radius as nextSpawn
                randSpawn.transform.localPosition = new Vector3(7, 0, 0);
               
                //Rotate spawns towards center
                Vector3 diff = randSpawn.transform.parent.position - randSpawn.transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                randSpawn.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
                //make spot red if 6th child
                if(randSpawn.transform.parent.childCount == 6)
                {
                    randSpawn.transform.parent.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
                }
            }
            else
            {
                Debug.Log("Can't Find any");
                
            }
            //else if (spots[rng].transform.childCount == 6 
            //            && randScore == spots[rng].transform.GetChild(spots[rng].transform.childCount - 1).GetComponent<Square>().Score)
            //{
            //    Debug.Log("same ");
            //    break;
            //}
        }
        while (rng == int.Parse(currentSpot.name));

      



    }
    

    public void GameOver()
    {
        if (currentSpot.transform.childCount == 6)
        {
            //full spot colors red and opens another one
            currentSpot.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
           
          
        }
        int reds = 0;
        float timer = 4f;

        do
        {
            timer -= Time.deltaTime;

        }
        while (timer >= 0);

        foreach (GameObject spot in spots)
        {
            if (spot.transform.GetChild(0).GetComponent<SpriteRenderer>().color == new Color32(255, 0, 0, 255) && !spot.GetComponent<Spot>().Blocked)
            {
                if (spot.transform.GetChild(spot.transform.childCount - 1) !=null /*&& spot.transform.GetChild(spot.transform.childCount - 1).GetComponent<Square>().Score != next_score*/)
                {
                    reds++;
                }
              
            }
            else if(spot.GetComponent<Spot>().Blocked)
            {
                reds++;
            }
        }
        if (reds == spots.Count /*&& (next_score != currentSpot.transform.GetChild(currentSpot.transform.childCount - 1).GetComponent<Square>().Score)*/)
        {
            nextScore.text = "GAME OVER";
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!GAMOVER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }








    }







}