using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

    private bool gameOverCheckSpot = true;
    private float cooldown = 0.3f;

    public bool GameOverCheckSpot
    {
        get
        {
            return gameOverCheckSpot;
        }

        set
        {
            gameOverCheckSpot = value;
        }
    }

    public void Update()
    {
        cooldown -= Time.deltaTime;


        if(gameObject.GetComponent<SpriteRenderer>().color == GameManager.Instance.leRed
            && GameOverCheckSpot && !GameManager.Instance.GameOverBool && cooldown<0)
        {
            GameOverCheckSpot = false;
            cooldown = 0.3f;
            StartCoroutine(GameManager.Instance.StopGameOverShort(gameObject));
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("line") && gameObject.CompareTag("spot"))
        {
            
            GameManager.Instance.currentSpot = gameObject;
            AudioManager.Instance.PlaySound("rotationClick");
            //GameManager.Instance.centerAnim.SetTrigger("tilt");
            //Debug.Log(GameManager.Instance.currentSpot);
        }
        else if (other.CompareTag("line") && gameObject.CompareTag("spawn"))
        {
            GameManager.Instance.currentSpawn = gameObject;
        }
    }
}
