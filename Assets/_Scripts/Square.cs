using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    [SerializeField]
    private bool bottom=false;
    private int row;
    public int Row
    { get { return row; } set { row = value; } }
    [SerializeField]
    private int score;
    public int Score
    { get {return score;} set {score = value;} }

   




    // Use this for initialization
    void Start () {


        GameManager.Instance.nextScore.text = GameManager.next_score.ToString();
        if (!bottom)
        {
         
            GameManager.next_score = Random.Range(1, 20);
            score = GameManager.next_score;
           

        }


       
        gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();

    }
	
	// Update is called once per frame
	void Update () {

     
      
	}

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("wheel") || other.gameObject.GetComponent<Square>().row == 0) 
        {
            if (transform != null)
                transform.SetParent(GameManager.Instance.wheelSquares.transform);
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(200, 200, 200, 255);
            gameObject.isStatic = true;
            this.row = 0;
        }

        if (other.gameObject.CompareTag("square"))
        {
           
                if (this.score == other.gameObject.GetComponent<Square>().Score)
                {
                Debug.Log("BOOP");
                GameManager.Instance.Merge(gameObject);
                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();
                Destroy(other.gameObject);
                }
           
        }

       
        
    }
}
