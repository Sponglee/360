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
    //Vertical transform of top spot
    public GameObject currentSpot;

    //All the spots around the wheel
    public List<GameObject> spots;


    // number of objects
    public int nBottom = 20;
    //Next square's score
    public Text nextScore;
    public static int next_score;


    void Start()
    {
        spots = new List<GameObject>();



        //Random next score to appear
        next_score = (int)Mathf.Pow(2, Random.Range(1, 4));
        nextScore.text = next_score.ToString();

        //Initialize level (spots)
        GetSpots(nBottom);



        //foreach(GameObject spot in spots)
        //{

        //    GameObject tmp = Instantiate(squarePrefab, spot.transform.position, spot.transform.rotation);
        //    tmp.GetComponent<Square>().bottom = true;
        // //   tmp.transform.SetParent(spot.transform);
        //    tmp.transform.Rotate(180, 90, -90);
        //}


    }


    void Update()
    {
        //if inside outer ring
        if (currentSpot.transform.childCount <= 5)
        {
            //turn left or right
            if (Input.GetMouseButtonUp(0) && SwipeManager.Instance.Direction == SwipeDirection.None)
            {
                Debug.Log(">>>>>" + SwipeManager.Instance.Direction);
                //spawn a square
                squareSpawn = Instantiate(squarePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                squareSpawn.GetComponent<Square>().Score = next_score;
                //get score for next turn
                next_score = (int)Mathf.Pow(2, Random.Range(1, 4));
                nextScore.text = next_score.ToString();

            }
        }
        else
        {
            Debug.Log("TOO MANY, SORLEY");
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
                wheel.transform.Rotate(Vector3.forward, 360 / nBottom);
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
        //first.transform.SetParent(GameManager.Instance.currentSpot.transform);              //comment this for II var
        
        first.GetComponent<Square>().Score *= 2;

        first.GetComponent<SpriteRenderer>().color = new Color32(200, 200, 200, 255);
        //first.isStatic = true;

        Debug.Log("MERGE");

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
            // make the object face the center
            var rot = Quaternion.FromToRotation(Vector3.forward, center - pos);
            GameObject tmp = Instantiate(spotPrefab, pos, Quaternion.LookRotation(Vector3.back));
            tmp.name = i.ToString();
            tmp.transform.SetParent(wheel.transform.GetChild(0));
            tmp.transform.LookAt(center, Vector3.right);
            tmp.transform.Rotate(0, 90, 0);
            spots.Add(tmp);
            currentSpot = spots[0];
        }

    }

    public void CheckRow(int spotIndex, int squareIndex, int checkScore)
    {
        //Circle around indexes(like CAESAR CS50)
        int nextSpotIndex;
        int prevSpotIndex;
        int left=0;
        int right=0;

        if (spotIndex-1 < 0)
        {
            prevSpotIndex = spots.Count;
            nextSpotIndex = spotIndex + 1;
        }
        else if (spotIndex+1>spots.Count)
        {
            prevSpotIndex = spotIndex - 1;
            nextSpotIndex = 0;
        }
        else
        {
            nextSpotIndex = spotIndex + 1;
            prevSpotIndex = spotIndex - 1;
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