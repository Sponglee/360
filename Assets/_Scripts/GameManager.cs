﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>

{ 
    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    public GameObject wheel;
    public GameObject squarePrefab;
    public GameObject spotPrefab;
    public GameObject spawnPrefab;
    public GameObject gridPrefab;
    public Transform line;

    //prefab for controlling movement while falling
    GameObject squareSpawn = null;
    //for random expand spawns
    GameObject randSpawn = null;
    
    public int randSpawnCount;
    //checker for player spawn
    private bool firstRands = true;

    public int maxScore;
    [SerializeField]
    public int scoreUpper;
    [SerializeField]
    public int expandMoves;
    [SerializeField]
    private float stopDelay;

    //[SerializeField]
    //private bool IsRunning = false;
   
    [SerializeField]
    private int moves = 0;
    public int Moves
    {
        get
        {
            return moves;
        }

        set
        {
            moves = value;
        }
    }
    //Vertical transform of top spot
    public GameObject currentSpot;
    // spawn point
    public GameObject currentSpawn;
    //next spot to turn green
    public int LastSpot { get; set; }
    private bool IsFurtherMerge = false;



    //All the spots around the wheel
    public List<GameObject> spots;
    public List<GameObject> spawns;
    public GameObject[,] grids;

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

    //// Obj list for pop checkrow
    //List<GameObject> rowObjs;
    // list of rowObjs to execute(resets each click)
    List<List<GameObject>> popObjs;

    // list of furthre pop/merge things
    List<GameObject> furtherPopObjs;

    List<RandValues> rands;
    //list of randSpawns
    List<GameObject> randSpawns;
    List<GameObject> tmpSquares;

    //Toggle while rand are dropping
    private bool randSpawning = false;
    int tmpRands;

    // struct to hold randomSpawn values
    public struct RandValues
    {
        public int Rng { get; set; }
        public int RandScore { get; set; }
    }

    //For getspots
    Vector3 center;
    float rad;

    public bool CheckInProgress = false;
    private bool RotationProgress = false;
    public float rotationDuration = 0.2f;
    private bool noMoves=false;


    //for ui check
    private bool mouseDown = false;
    private bool MenuUp = false;






    //spot rotation positions
    private Vector3 clickDirection;
    private float clickAngle;
    private float dirAngle;
    int checkClickSpot;

    int rotSpot;
    Vector3 lineDir;
    Vector3 spotDir;

    //swipe cooldown resistance
    float followCoolDown;
    bool rotDigit;
    Quaternion initRotation;
    Vector3 initClick;
    bool firstClick = true;
    bool followExit = false;
    //For clickspawn
    bool cantSpawn = true;
    //for direction detection
    string direc;

    /*---------------------
     * */

    public float follow__Delay = 0.2f;
    public float follow__Angle = 0.5f;
    public float dif__Angle= 0.5f;
    public float differ__Angle=0.1f;

    void Start()
    {
        
        //Apply all the numbers 
        maxScore = 3;
        expandMoves = 3;
        // count of randomSpawns 
        randSpawnCount = 3;
        scoreUpper = (int)Mathf.Pow(2, maxScore);
        nBottom = 10;
        spots = new List<GameObject>();
        spawns = new List<GameObject>();
        grids = new GameObject[nBottom, 5];
       
        scores = 0;
        ScoreText.text = scores.ToString();
        upper.text = string.Format("upper: {0}", scoreUpper);
        nextShrink.text = string.Format("next shrink: {0}", expandMoves - Moves);
        slider.value = (expandMoves - Moves) / expandMoves;


        //Random next score to appear (2^3 max <-----)
        next_score = (int)Mathf.Pow(2, Random.Range(1, 4));
        nextScore.text = next_score.ToString();

        //Initialize level (spots)
        GetSpots();


        //rowObjs = new List<GameObject>();
        popObjs = new List<List<GameObject>>();

        tmpSquares = new List<GameObject>();

        furtherPopObjs = new List<GameObject>();
        //for first spwan of 2
        tmpRands = randSpawnCount;

    }

 

    void Update()
    {
        //wheel.transform.up line
        //GLDebug.DrawLine(wheel.transform.up - new Vector3(0, 7f), wheel.transform.position, Color.red, 0, true);
        if (rotSpot != -1)
        {
            // spot sticky line
            //GLDebug.DrawLine(spots[rotSpot].transform.position, wheel.transform.position, Color.cyan, 0, true);
          
        }


        //========================

        if (!IsPointerOverUIObject() && Input.GetMouseButtonDown(0) && !MenuUp && !randSpawning)
        {
            mouseDown = true;
            cantSpawn = false;
            checkClickSpot = int.Parse(currentSpot.name);
            //FollowMouse delay
            followCoolDown = Time.time + follow__Delay;

            initClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickAngle = GetFirstClickAngle();
           
        
            //Debug.Log("CHECK " + checkClickAngle);


            clickDirection = wheel.transform.up / Mathf.Sin(clickAngle);
            initRotation = wheel.transform.rotation;
            

        }


        if (!IsPointerOverUIObject() && Input.GetMouseButton(0) && !RotationProgress && !noMoves && !randSpawning && !MenuUp)
        {
            //initialClick /clickAngle
            //GLDebug.DrawLine(initClick, wheel.transform.position, Color.magenta, 0, true);

            //dirangle
            //GLDebug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), wheel.transform.position, Color.white, 0, true);


            //=============================================


            dirAngle = GetFirstClickAngle();




            //CHecks direction for rotation
            if (!RotationProgress)
            {
                if ((clickAngle - dirAngle) < -dif__Angle)
                    direc = "-";
                else if ((clickAngle - dirAngle) > dif__Angle)
                    direc = "+";
                else
                    cantSpawn = false;    
            }
          
                

            //Debug.Log(direc);




            // touch resistance (firstClick for smooth rotation after first displacement
            if (Mathf.Abs(clickAngle - dirAngle) < follow__Angle && !RotationProgress && firstClick)
            {
                //for click spawn
                if (Mathf.Abs(clickAngle - dirAngle) > dif__Angle)
                    cantSpawn = false;
                return;
            }
            else
            {
                cantSpawn = true;
              
                    

               
                    firstClick = false;
                    FollowMouse(clickAngle, clickDirection);
               
                  
                
            }



        }
          
         

            if (Input.GetMouseButtonUp(0) && Time.time > coolDown && !RotationProgress && !noMoves && !randSpawning && !MenuUp)
            {
              
                if (mouseDown)
                {
                   
                    if (int.Parse(currentSpot.name) == checkClickSpot)
                    {
                      //  Debug.Log(" NO MOVE");
                        //no closestSpot
                        rotSpot = -1;
                    }

                    if (cantSpawn && !RotationProgress)
                        StartCoroutine(Rotate(int.Parse(currentSpot.name), rotationDuration));
                
                    firstClick = true;
                    mouseDown = false;
                }

                if (!cantSpawn && currentSpot.transform.childCount <= 4 && currentSpot.GetComponent<SpriteRenderer>().color != new Color32(255, 0, 0, 255))
                {

                    ClickSpawn();
                    cantSpawn = true;
                }

        }

    }


    private float GetFirstClickAngle()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);


        Vector3 direction = screenPos - wheel.transform.position;

        return Mathf.Atan2(Vector3.Dot(Vector3.back, Vector3.Cross(wheel.transform.up, direction)), Vector3.Dot(wheel.transform.up, direction)) * Mathf.Rad2Deg;

    }

   



    private void FollowMouse(float startAngle, Vector3 startDirection)
    {

        followExit = false;
        float angle = GetFirstClickAngle();

        wheel.transform.Rotate(Vector3.forward, startAngle - angle);
        int firstSpot;
        int nextSpot;
        int spot = int.Parse(currentSpot.name);

        //passing through 0
        if (spot - 1 < 0)
        {
            firstSpot = nBottom - 1;
        }
        else
            firstSpot = spot - 1;

        //check next left one after getting index-1
        if (spot + 1 == nBottom)
        {
            nextSpot = 0;
        }
        else
            nextSpot = spot + 1;


        int[] spotDist = { spot, firstSpot, nextSpot };

        //check which spot is closer:

        rotSpot = ClosestSpot(spotDist);

        //Debug.Log(rotSpot);
        
    }


    //Smooth rotation coroutine
    IEnumerator Rotate(int spot, float duration = 0.2f, bool digit=true)
    {
        RotationProgress = true;
        float angle;
        int thisSpot;
        int firstSpot;
        int nextSpot;


       

       
        //if there was no followmouse
        if (rotSpot == -1)
        {
            // cycle through 0

            if (checkClickSpot - 1 < 0)
            {
                firstSpot = nBottom - 1;
            }
            else
                firstSpot = checkClickSpot - 1;

            //check next left one after getting index-1
            if (spot + 1 == nBottom)
            {
                nextSpot = 0;
            }
            else
                nextSpot = checkClickSpot + 1;


            //choose a direction

            if (direc == "-")

                thisSpot = firstSpot;
            else if (direc == "+")
                thisSpot = nextSpot;
            else
                thisSpot = int.Parse(currentSpot.name);


            Vector3 lineDir = line.position - wheel.transform.position;
            Vector3 spotDir = spots[thisSpot].transform.position - wheel.transform.position;

            angle = Mathf.Atan2(Vector3.Dot(Vector3.back, Vector3.Cross(lineDir, spotDir)), Vector3.Dot(lineDir, spotDir)) * Mathf.Rad2Deg;
           // Debug.Log("curr spot " + checkClickSpot + "this angle " + angle + " > " + direc);
        }
        else
        {
            Vector3 lineDir = line.position - wheel.transform.position;
            Vector3 spotDir = spots[rotSpot].transform.position - wheel.transform.position;
           
            angle = Mathf.Atan2(Vector3.Dot(Vector3.back, Vector3.Cross(lineDir, spotDir)), Vector3.Dot(lineDir, spotDir)) * Mathf.Rad2Deg;

            Debug.Log("@" + angle);
        }

        Quaternion from = wheel.transform.rotation;
        Quaternion to = from * Quaternion.Euler(0, 0, angle);
     
        float elapsed = 0.0f;
        while (elapsed < duration)
        {

            wheel.transform.rotation = Quaternion.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;

            yield return null;
        }
        float differ;
        Quaternion difRot = wheel.transform.rotation;
        
        if (rotSpot != -1)
             differ = rotSpot * (360 / nBottom) - difRot.eulerAngles.z;
        else
             differ = int.Parse(currentSpot.name) * (360 / nBottom) - difRot.eulerAngles.z;

        //Get rid of difference flaw to the left
        if (Mathf.Abs(differ) < differ__Angle)
        {
            Debug.Log("YE "+ differ);
            Quaternion finalRot = difRot * Quaternion.Euler(0, 0, differ);
            wheel.transform.rotation = finalRot;
        }

        RotationProgress = false;

        // for sticky ray
        //rotSpot = -1;
    }


    private int ClosestSpot(int[] spotDist)
    {
        //Debug.Log(">>"+spotDist.Length);
        float minDist = Mathf.Infinity;
        int tMin= -1;
        Vector3 checkPos = line.position;
        foreach (int t in spotDist)
        {
            float dist = Vector3.Distance(spots[t].transform.position, line.position);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }


    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if (results.Count > 0) return results[0].gameObject.CompareTag("ui");
        else return false;

    }


    private void ClickSpawn()
    {

        coolDown = Time.time + 0.5f;

            //spawn a square
            squareSpawn = Instantiate(squarePrefab, currentSpawn.transform.position, Quaternion.identity);
            squareSpawn.GetComponent<Square>().Score = next_score;
            //get score for next turn (non-inclusive)
            next_score = (int)Mathf.Pow(2, Random.Range(1, maxScore + 1));
            nextScore.text = next_score.ToString();
            squareSpawn.GetComponent<Square>().IsSpawn = true;

    }

    public void Merge(GameObject first, GameObject second=null)
    {
        StartCoroutine(StopMerge(first,second));
    }



    //delay for merge
    private IEnumerator StopMerge(GameObject first, GameObject second=null)
    {

        yield return new WaitForSeconds(0.2f);
        Destroy(second);
        //if (first == null)
        //{
        //    //yield break;
        //}
        if (first != null)
        {
            int tmp = first.GetComponent<Square>().Score *= 2;
            first.GetComponent<Square>().ApplyStyle(tmp);

            if (tmp > scoreUpper)
            {
                scoreUpper *= 2;
                Instance.upper.text = string.Format("upper: {0}", scoreUpper);

            }
            first.GetComponent<Square>().Touched = false;
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
    public void GetSpots()
    {
        rad = wheel.transform.GetChild(0).GetComponent<CircleCollider2D>().radius;
        center = wheel.transform.position;


        //Spots, spawns and grid for movement
        SpawnSpots(spotPrefab, rad, 1, spots);
        SpawnSpots(spawnPrefab, rad + 5.5f, 1, spawns);

        currentSpot = spots[0];
        currentSpawn = spawns[0];


        SpawnSpots(gridPrefab, rad + 0.55f, 5, null, grids);

    }

    private void SpawnSpots(GameObject prefab, float rad, int count, List<GameObject> lists = null, GameObject[,] grids = null)
    {

        for (int i = 0; i < nBottom; i++)
        {
            for (int j = 0; j < count; j++)
            {

                int a = 360 / nBottom * i;
                var pos = RandomCircle(center, rad + 0.9f * j, a);
                GameObject tmp = Instantiate(prefab, pos, Quaternion.LookRotation(Vector3.back));
                tmp.name = i.ToString();

                int toggle = 0;

                if (prefab.CompareTag("spot"))
                {
                    toggle = 0;
                }
                else if (prefab.CompareTag("spawn"))
                {
                    toggle = 1;
                }
                else if (prefab.CompareTag("grid"))
                {
                    toggle = 2;
                }

                tmp.transform.SetParent(wheel.transform.GetChild(toggle));
                tmp.transform.LookAt(center, Vector3.right);
                tmp.transform.Rotate(0, 90, 0);
                if (grids != null)
                {
                    grids[i, j] = tmp;
                    tmp.transform.SetParent(GameManager.Instance.spawns[i].transform);
                    tmp.name = (j + 1).ToString();
                }
                else
                {
                    lists.Add(tmp);

                }
            }
        }


    }



    //Checks for 3 in a row
    public void CheckRow(int spotIndex, int squareIndex, int checkScore, GameObject tmpSquare)
    {
       
        //Debug.Log("ScheckRow"+spotIndex);
        List<GameObject> rowObjs = new List<GameObject>();

        //iterator for list
        int index = spotIndex;
        //noMoves = false;
        //index of start of rowObjs (outside nbottom numbers to be safe)
        int startIndex = nBottom + 10;
        int endIndex = nBottom + 11;
        int firstIndex = 0;
        int nextIndex = 0;

        //add placed square to rowOBjs
        rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);

        do
        {
            //if there's no start yet
            if (startIndex > nBottom)
            {
                //passing through 0
                if (index - 1 < 0)
                {
                    index = nBottom - 1;
                }
                else
                    index = index - 1;

                //check next left one after getting index-1
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
                                rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                                startIndex = nBottom + 10;
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
                //passing through nBottom (0)
                if (index + 1 > nBottom - 1)
                {
                    index = 0;
                }
                else
                    index = index + 1;
                //check next one after setting index+1
                if (index + 1 > nBottom - 1)
                {
                    nextIndex = 0;
                }
                else
                    nextIndex = index + 1;
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
                                if (endIndex > nBottom)
                                {
                                    endIndex = index;
                                    break;
                                }
                            }
                            else if (spots[nextIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore)
                            {
                                rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                                continue;
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

        // 2 row
        if (rowObjs.Count < 2)
        {
            
            // expand moves++ if this happened by player
            if (tmpSquare.GetComponent<Square>().IsSpawn || IsFurtherMerge)
            {

                ExpandMoves();

                if (Moves > expandMoves - 1)
                {

                    Expand();
                    Moves = 0;
                    slider.value = 1;

                    //expandMoves += expandMoves/2;
                    nextShrink.text = string.Format("next shrink: {0}", expandMoves - Moves);
                    slider.value = (float)(expandMoves - Moves) / expandMoves;
                }
            }
        }
        else
        {
            //Get rid of the one we keep
            rowObjs.Remove(tmpSquare);

            //add its tmpSquare to list
            if(!tmpSquares.Contains(tmpSquare))
            {
                tmpSquares.Add(tmpSquare);
            }
           
            //If there's no same rowObj in pop - add
            if (!popObjs.Contains(rowObjs))
            {
                    popObjs.Add(rowObjs);
            }
        }

        StartCoroutine(StopPop(popObjs, tmpSquares, wheel));
    }


    IEnumerator StopPop(List<List<GameObject>> thisPopObjs, List<GameObject> tmpSquares, GameObject wheel)
    {
      
        int count = 0;
        //Debug.Log("STARTING COURUTINE   " + thisPopObjs.Count + " === " + rowObjs[0].GetComponent<Square>().Score);
        yield return new WaitForSeconds(0.2f);
        
        foreach (List<GameObject> rowObjs in thisPopObjs)
        {
              
                Pop(rowObjs, tmpSquares[count]);
                //yield return new WaitForSeconds(0.2f);
                StartCoroutine(FurtherMerge(tmpSquares[count]));
                count++;
            
        }
        //Clear pop objs list
        for (int i = 0; i < popObjs.Count; i++)
        {
            if (popObjs[i].Count == 0)
                popObjs.RemoveAt(i);
        }
        for (int i = 0; i < tmpSquares.Count; i++)
        {
                tmpSquares.RemoveAt(i);
        }
    }


    //Kill all adjacent squares
    public void Pop(List<GameObject> rowObjs, GameObject tmpSquare= null)
    {
       
        //Keep one that has fallen
        Merge(tmpSquare);

        // Move others
        if (rowObjs.Count != 0)
        {
            foreach (GameObject rowObj in rowObjs)
            {
                //Update the score
                if (rowObj.GetComponent<Square>() != null)
                {
                    scores += rowObj.GetComponent<Square>().Score;
                    ScoreText.text = scores.ToString();
                }

                if (rowObj.transform.parent != null)
                {
                    //rowObj.transform.position += new Vector3(0, 0, 10);
                    if (!rowObj.transform.parent.GetComponent<Spot>().Blocked)
                    {
                        rowObj.transform.parent.GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
                    }

                }
              if (rowObj.transform != null && tmpSquare != null)
                {
                    rowObj.GetComponent<Square>().SquareTmpSquare = tmpSquare.transform;
                    rowObj.GetComponent<Collider2D>().isTrigger = true;


                    StartCoroutine(FurtherMerge(rowObj));
                    //Detach this square from parent
                    rowObj.transform.parent = null;
                }
                

            }
            rowObjs.Clear();
        }
    }

    public IEnumerator FurtherMerge(GameObject tmpSquare)
    {
            IsFurtherMerge = true;
            //Debug.Log("START OINK " + Time.deltaTime);

            yield return new WaitForSeconds(0.2f);
            if (tmpSquare != null)
            {
                //for (int i = tmpSquare.transform.GetSiblingIndex(); i < tmpSquare.transform.parent.childCount; i++)
                //{
                    tmpSquare.transform.localPosition += new Vector3(0.5f, 0, 0);
                //    Debug.Log(" BOUNCE " + i);
                //}
              
            }
            //Debug.Log("OINK " + Time.deltaTime);

            IsFurtherMerge = false;
    }


    // update moves
    public void ExpandMoves()
    {
        Moves++;
        nextShrink.text = string.Format("next shrink: {0}", expandMoves - Moves);
        slider.value = (float)(expandMoves - Moves) / expandMoves;
    }




    public void Expand()
    {
      
         
        //spawn only 2 when upper is 8

        if (scoreUpper == 8)
        {
            randSpawnCount = tmpRands - 1;
          
        }
        else
            randSpawnCount = tmpRands;


        RandValues tmp = new RandValues();
        rands = new List<RandValues>();

        //Rand Score checker
        List<int> randCheckTmp = new List<int>();

        //randSpawn is upper Power -1 always
        int upperPow = (int)Mathf.Log(scoreUpper, 2)-1;
        List<int> randList = new List<int>();
        //get free spots


        foreach(GameObject spot in spots)
        {
            if (spot.transform.childCount < 5
                    && spot.name != currentSpot.name)
            {
                randList.Add(int.Parse(spot.name));
            }
        }
        //if there's atleast 3 spots to spawn randoms
        if (randList.Count >= randSpawnCount)
        {
            rands.Clear();
            
            for (int i = 0; i < randSpawnCount; i++)
            {
                tmp.Rng = randList[Random.Range(0, randList.Count)];
                tmp.RandScore = 0;

                if (rands.Count >= 1 && ListContains(rands, tmp))
                {
                    while (ListContains(rands, tmp))
                    {
                        tmp.Rng = randList[Random.Range(0, randList.Count)];
                       
                        tmp.RandScore = 0;

                    }
                    
                }

                do
                {
                    tmp.RandScore = (int)Mathf.Pow(2, Random.Range(1, upperPow + 1));
                   // Debug.Log(" RANDSCORE " + tmp.RandScore);
                }
                while (randCheckTmp.Contains(tmp.RandScore) || tmp.RandScore >= 256);
                randCheckTmp.Add(tmp.RandScore);
                    
                   
                rands.Add(tmp);
                //Debug.Log(rands[rands.Count - 1].Rng + " " + rands[rands.Count - 1].RandScore);

            }


            StartCoroutine(StopRandom(rands));
           
        }
        else
        {
            rands.Clear();
        }
    }

    //Spread random spawns
    private IEnumerator StopRandom(List<RandValues>rands)
    {
        randSpawning = true;
        //spawn all
        foreach (RandValues rand in rands)
        {
            yield return new WaitForSeconds(0.2f);
            //Debug.Log("========");
            SpawnRandom(rand.Rng, rand.RandScore);
        }
        randSpawning = false;
    }






    //Check if list of classes contains same rng
    bool ListContains(List<RandValues> list, RandValues check)
    {
        foreach (RandValues n in list)
        {
            if (n.Rng == check.Rng)
            {
               
                return true;
            }
             
        }
        return false;
    }


    //Check if list of classes contains same rng
    bool RandScoreContains(List<RandValues> list, RandValues check)
    {
        foreach (RandValues n in list)
        {
            if (n.RandScore == check.Rng)
            {

                return true;
            }

        }
        return false;
    }

    // Spawn randomSquare
    public void SpawnRandom(int rng, int randScore)
    {
        randSpawns = new List<GameObject>();
        //Debug.Log("SPAWN");
        //spawn a square at random spot with random score
        randSpawn = Instantiate(squarePrefab, spawns[rng].transform.position, Quaternion.identity);
        randSpawn.GetComponent<Square>().ExpandSpawn = true;
        randSpawn.GetComponent<Square>().Score = randScore;
        randSpawn.transform.SetParent(spots[rng].transform);
        randSpawn.name = randSpawn.transform.GetSiblingIndex().ToString();
        

        //Rotate spawns towards center
        Vector3 diff = randSpawn.transform.parent.position - randSpawn.transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        randSpawn.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
        //make spot red if 6th child
        if (randSpawn.transform.parent.childCount == 5)
        {
            randSpawn.transform.parent.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
        }

        randSpawns.Add(randSpawn);
    }
  


    public void GameOver()
    {
        if (currentSpot.transform.childCount == 5)
        {
            //full spot colors red and opens another one
            currentSpot.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);

        }
        int reds = 0;

        foreach (GameObject spot in spots)
        {
            if (spot.GetComponent<SpriteRenderer>().color == new Color32(255, 0, 0, 255) && !spot.GetComponent<Spot>().Blocked)
            {
                if (spot.transform.GetChild(spot.transform.childCount - 1) != null /*&& spot.transform.GetChild(spot.transform.childCount - 1).GetComponent<Square>().Score != next_score*/)
                {
                    reds++;
                }

            }
            else if (spot.GetComponent<Spot>().Blocked)
            {

                reds++;
            }
        }

        StartCoroutine(StopGameOver(reds));


    }


    private IEnumerator StopGameOver(int reds)
    {
        yield return new WaitForSeconds(0.2f);
        if (reds == spots.Count /* && (next_score != currentSpot.transform.GetChild(currentSpot.transform.childCount - 1).GetComponent<Square>().Score)*/)
        {
            noMoves = true;

            yield return new WaitForSeconds(0.4f);
            if (reds == spots.Count)
            {
                OpenMenu();
                nextScore.text = "GameOver";
                menu.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = string.Format("your score: {0}", scores);
            }
            else
                noMoves = false;
        }
    }


    public void OpenMenu()
    {
        ui.SetActive(!ui.activeSelf);
        menu.SetActive(!menu.activeSelf);
        menu.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = string.Format("your score : {0}", scores);
        MenuUp = !MenuUp;
    }


    //Restarts game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        OpenMenu();
        //GameOverMenu.SetActive(false);
        //In case game was paused before
        Time.timeScale = 1;
    }

    //Quit
    public void Quit()
    {
        Application.Quit();
    }





    public void TweakAngle(string value)
    {
        dif__Angle = float.Parse(value);
    }

    public void TweakDelay(string value)
    {
        follow__Delay = float.Parse(value);
    }

    public void TweakFollow(string value)
    {
        follow__Angle = float.Parse(value);
    }

    public void TweakDiffer(string value)
    {
        differ__Angle = float.Parse(value);
    }
}