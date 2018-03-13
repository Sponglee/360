using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

    [SerializeField]
    public GameObject wheel;
    public GameObject squarePrefab;
    public GameObject spotPrefab;
    public Transform wheelSquares;


    public List<GameObject> spots;

    // number of objects
    public int nBottom=20;

    public Text nextScore;
    public static int next_score;
    // Use this for initialization
    void Start () {
        spots = new List<GameObject>();


        next_score = (int)Mathf.Pow(2, Random.Range(1, 4));
        nextScore.text = next_score.ToString();

        GetSpots(nBottom);

        //foreach(GameObject spot in spots)
        //{

        //    GameObject tmp = Instantiate(squarePrefab, spot.transform.position, spot.transform.rotation);
        //    tmp.GetComponent<Square>().bottom = true;
        // //   tmp.transform.SetParent(spot.transform);
        //    tmp.transform.Rotate(180, 90, -90);
        //}


    }
	
	// Update is called once per frame
	void Update () {
      

        if (Input.GetKeyDown(KeyCode.A))
        {
            wheel.transform.Rotate(Vector3.forward, 360/nBottom);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            wheel.transform.Rotate(Vector3.forward, -360/nBottom);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
          
            GameObject squareSpawn = Instantiate(squarePrefab, new Vector3(0f,0f,0f) , Quaternion.identity);
            squareSpawn.GetComponent<Square>().Score = next_score;
            
            next_score = (int)Mathf.Pow(2, Random.Range(1, 4));
            nextScore.text = next_score.ToString();
        }
    }


    public void Merge(GameObject first)
    {
        first.transform.SetParent(GameManager.Instance.wheelSquares.transform);
        first.GetComponent<Square>().Score *= 2;

        first.GetComponent<SpriteRenderer>().color = new Color32(200, 200, 200, 255);
        first.isStatic = true;
        
        Debug.Log("WIN");

    }

    public void Shrink()
    {

    }

 

    public Vector3 RandomCircle(Vector3 center, float radius, int a)
    {
        Debug.Log(a);
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
            tmp.transform.SetParent(wheel.transform.GetChild(0));
            tmp.transform.LookAt(center, Vector3.right);
            tmp.transform.Rotate(0, 90, 0);
            spots.Add(tmp);
        }

    }

}
