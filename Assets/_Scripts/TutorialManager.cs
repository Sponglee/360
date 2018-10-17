using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class TutorialManager : Singleton<TutorialManager> {

    public int tutorialStep;
    public GameObject button;

    public Canvas tutorialCanvas;

    public UnityEvent tutorialTrigger;

    //Asking window
    public GameObject surePanel;

    public string[] tutorialTextRU;
    public string[] tutorialTextEN;
   

    public string[] tutorialText;
    public GameObject[] tutorialTouch;

    public Animator[] powerUpAnim;


    // Use this for initialization
    void Start () {

       
        tutorialStep = PlayerPrefs.GetInt("TutorialStep", 0);
     
        if (tutorialStep <= 1)
        {
            if (PlayerPrefs.GetInt("Language", 0) == 0)
            {
                tutorialText = tutorialTextEN;
            }
            else
                tutorialText = tutorialTextRU;

            tutorialCanvas.gameObject.SetActive(true);
            tutorialTouch[PlayerPrefs.GetInt("TutorialStep", 0)].SetActive(true);
        }
        else
            tutorialCanvas.gameObject.SetActive(false);
    }
	


    public void Clicked()
    {
        Debug.Log("TRIGGERED !!!! " + tutorialStep);
        tutorialTouch[PlayerPrefs.GetInt("TutorialStep", 0)].SetActive(false);
        tutorialStep++;
        PlayerPrefs.SetInt("TutorialStep", tutorialStep);

        //tutorialCanvas.gameObject.SetActive(false);
        if (tutorialStep <= 1)
            tutorialTouch[PlayerPrefs.GetInt("TutorialStep", 0)].SetActive(true);
        else
            tutorialCanvas.enabled = false;
      

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
        tutorialCanvas.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        GameManager.Instance.tutorialManager.tutorialCanvas.gameObject.SetActive(false);

    }



    public void ChangeLanguage()
    {
        int language = PlayerPrefs.GetInt("Language", 0);


        language++;

        if (language == 2)
            language = 0;

        PlayerPrefs.SetInt("Language", language);


        if (language == 0)
        {
            tutorialCanvas.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "RU";
            tutorialText = tutorialTextEN;

        }
        else
        {
            tutorialCanvas.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "EN";
            tutorialText = tutorialTextRU;
        }

          
    
        tutorialCanvas.transform.GetComponentInChildren<Text>().text = tutorialText[PlayerPrefs.GetInt("TutorialStep", 0)];
    }


    public void SkipTutorial()
    {
        PlayerPrefs.SetInt("TutorialStep", 3);
        tutorialStep = 3;
        tutorialCanvas.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SkipTutorialClick()
    {
        surePanel.SetActive(true);
    }


    public void Sure(int value)
    {
        if (value == 0)
        {
            surePanel.SetActive(false);
        }
        else
            SkipTutorial();
    }
}
