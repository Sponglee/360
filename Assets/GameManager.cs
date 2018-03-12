using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

    [SerializeField]
    private GameObject wheel;
    public GameObject squarePrefab;
    public Transform wheelSquares;

    
    public Text nextScore;
    public static int next_score;
    // Use this for initialization
    void Start () {
        next_score = Random.Range(1, 20);
        nextScore.text = next_score.ToString();
    }
	
	// Update is called once per frame
	void Update () {
      

        if (Input.GetKeyDown(KeyCode.A))
        {
            wheel.transform.Rotate(Vector3.forward, 20f);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            wheel.transform.Rotate(Vector3.forward, -20f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(squarePrefab, new Vector3(0f,0f,0f) , Quaternion.identity);
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
}
