using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    [SerializeField]
    public bool bottom=false;
    private int row;
    public int Row
    { get { return row; } set { row = value; } }
    [SerializeField]
    private int score;
    public int Score
    { get {return score;} set {score = value;} }

    public Transform Column
    {get{return column;}set{column = value;}}

    private Transform column;



    // Use this for initialization
    void Start () {
        gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();
      
    }
	
	// Update is called once per frame
	void Update () {

     
      
	}

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("spot") && !bottom)
        {

            gameObject.transform.position = other.gameObject.transform.position;
            gameObject.transform.SetParent(other.gameObject.transform);
            this.column = other.gameObject.transform;
            bottom = true;

        }

        if (other.gameObject.CompareTag("square"))
        {
            if (this.score == other.gameObject.GetComponent<Square>().Score && !bottom)
            {
                Debug.Log("BOOP");
                if (other.gameObject.GetComponent<Square>().Score == 64)
                {
                    GameManager.Instance.Merge(gameObject);
                    Destroy(gameObject);
                }
                else
                    GameManager.Instance.Merge(gameObject);

                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = score.ToString();
                Destroy(other.gameObject);
            }
            else if (this.score != other.gameObject.GetComponent<Square>().Score)
            {
                gameObject.transform.SetParent(GameManager.Instance.wheel.transform.GetChild(1));
                gameObject.GetComponent<SpriteRenderer>().color = new Color32(200, 200, 200, 255);
                gameObject.isStatic = true;

            }
            }
        }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("center") && gameObject.CompareTag("square") && !bottom)
        {
            // SHRINKS THE WHEEl
            float r = GameManager.Instance.wheel.transform.GetChild(0).GetComponent<CircleCollider2D>().radius;
            int n = 18;
            float angle = 360 / n;
            float r_n = r * (2 - angle / 180) / 2;
            n--;
            GameManager.Instance.wheel.transform.GetChild(0).GetComponent<CircleCollider2D>().radius = r_n;
            Destroy(gameObject);
        }

    }
}





