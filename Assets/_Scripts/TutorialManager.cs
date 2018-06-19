using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class TutorialManager : Singleton<TutorialManager> {

    public int tutorialStep;
    public GameObject button;

    public Canvas tutorialCanvas;

    public UnityEvent tutorialTrigger;

    public string[] tutorialText;

    public Animator[] powerUpAnim;

	// Use this for initialization
	void Start () {

        tutorialStep = PlayerPrefs.GetInt("TutorialStep", 0);
     
        if (tutorialStep <= 5)
        {
            
            tutorialCanvas.gameObject.SetActive(true);
            tutorialCanvas.transform.GetComponentInChildren<Text>().text = tutorialText[PlayerPrefs.GetInt("TutorialStep", 0)];
        }
        else
            tutorialCanvas.gameObject.SetActive(false);
    }
	


    public void Clicked()
    {
        Debug.Log("TRIGGERED !!!! " + tutorialStep);
        tutorialStep++;
        PlayerPrefs.SetInt("TutorialStep", tutorialStep);
            
        //tutorialCanvas.gameObject.SetActive(false);
        tutorialCanvas.transform.GetComponentInChildren<Text>().text = tutorialText[PlayerPrefs.GetInt("TutorialStep", 0)];

      

    }

    public void CloseTutorial()
    {
        tutorialCanvas.gameObject.SetActive(false);
    }

    //Coroutine to fill squares for the Drill tutorial
    public IEnumerator StopTutDrill()
    {
        yield return new WaitForSeconds(1f);
        //PREPEARE FOR DRILL TUTORIAL
        int tutScore = 2;

        for (int i = 0; i < 4; i++)
        {
            GameObject tutSpawn;

            if (GameManager.Instance.currentSpot.transform.childCount < 4)
            {
                tutSpawn = Instantiate(GameManager.Instance.squarePrefab, GameManager.Instance.currentSpawn.transform.position, Quaternion.identity, GameManager.Instance.currentSpawn.transform);
                tutSpawn.GetComponent<Square>().Score = tutScore;
                yield return new WaitForSeconds(0.2f);

            }
            tutScore *= 2;
        }
    }

    //Close tutorial cooldown
    public IEnumerator StopCloseTut()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.tutorialManager.tutorialCanvas.gameObject.SetActive(false);

    }
}
