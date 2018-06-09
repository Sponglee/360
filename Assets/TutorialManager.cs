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
}
