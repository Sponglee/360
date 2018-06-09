using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class TutorialManager : Singleton<TutorialManager> {

    public int tutorialStep;

    public Canvas tutorialCanvas;

    public UnityEvent tutorialTrigger;

    public string[] tutorialText;
    

	// Use this for initialization
	void Start () {
       

        if (tutorialStep <= 5)
        {

            tutorialCanvas.gameObject.SetActive(true);
        }
        else
            tutorialCanvas.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        tutorialStep = PlayerPrefs.GetInt("TutorialStep", 0);
    }


    public void Clicked()
    {
        PlayerPrefs.SetInt("TutorialStep", tutorialStep + 1);
        Debug.Log(tutorialStep);
        //tutorialCanvas.gameObject.SetActive(false);
        tutorialCanvas.transform.GetComponentInChildren<Text>().text = tutorialText[PlayerPrefs.GetInt("TutorialStep", 0)];

      

    }
}
