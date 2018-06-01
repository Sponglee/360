using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager> {

    public int firstLaunch;




	// Use this for initialization
	void Start () {
        firstLaunch = PlayerPrefs.GetInt("FirstLauch", 0);


	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Clicked()
    {
        Debug.Log("CLICKED");
    }
}
