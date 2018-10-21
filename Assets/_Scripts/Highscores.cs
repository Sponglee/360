using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;



public class Highscores : Singleton<Highscores>
{
    public Image[] leaderButtons;

    public Text[] highscoreTexts;

    public Text errorText;
    public string[] privateCodes;
    public string[] publicCodes;


    public string privateCode;
    public string publicCode;
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;

    public void Start()
    { 
        
        DontDestroyOnLoad(gameObject);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    public void AddNewHighscore(string username, int score,int dbIndex)
    {
        privateCode = privateCodes[dbIndex];
        StartCoroutine(UploadNewHighscore(username, score));
    }

    IEnumerator UploadNewHighscore(string username, int score)
    {

        username = Clean(username);

        WWW www = new WWW(webURL + privateCode + "/add-pipe/" + WWW.EscapeURL(username) + "/" + score.ToString());
        Debug.Log(www.url);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
            print("Upload Successful");
        else
        {
            print("Error uploading: " + www.error);
        }

        Debug.Log(username + " : " + score);
    }

    public void DownloadHighscores(int dbIndex)
    {
        for (int i = 0; i < leaderButtons.Length; i++)
        {
            if (i == dbIndex)
                leaderButtons[i].color = new Color32(171, 61, 50, 255);
            else
                leaderButtons[i].color = Color.white;
        }


        publicCode = publicCodes[dbIndex];
        StartCoroutine("DownloadHighscoresFromDatabase");
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
       
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighscores(www.text);
        }
        else
        {
           errorText = GameObject.FindGameObjectWithTag("DebugName").GetComponent<Text>();
           errorText.text = www.error + "\nPlease try again later..";
        }
    }

    //Clean off unwanted characters
    string Clean(string s)
    {
        s = s.Replace("/", "");
        s = s.Replace("|", "");
        return s;

    }

    //Prefabs for Score Display

    public GameObject scoreElementPref;
    public Transform scoreElementContainer;
   
    //Format and display highscores
    void FormatHighscores(string textStream)
    {
        //grab a reference for a container
        scoreElementContainer = GameObject.FindGameObjectWithTag("LeaderContainer").transform;


        errorText = GameObject.FindGameObjectWithTag("DebugName").GetComponent<Text>();
        errorText.enabled = false;

        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresList = new Highscore[entries.Length];

        //Reset scoreboard
        foreach (Transform child in scoreElementContainer)
        {
            Destroy(child.gameObject);
        }

        //Add every entry to the list
        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highscoresList[i] = new Highscore(username, score);




           
            GameObject tmp = Instantiate(scoreElementPref, scoreElementContainer);

            tmp.transform.GetChild(0).GetComponentInChildren<Text>().text = highscoresList[i].username;
            tmp.transform.GetChild(1).GetComponentInChildren<Text>().text = highscoresList[i].score.ToString(); 
            
            if(highscoresList[i].username == PlayerPrefs.GetString("PlayerName","offlineUser"))
            {
                tmp.transform.GetChild(0).GetComponentInChildren<Text>().color = new Color32 (255,247,100,255);
                //Debug.Log(textStream + " : " + highscoresList[i].score.ToString());
                //highscoreTexts[dbIndex].text = highscoresList[i].score.ToString();
            }
        }
    }

}

public struct Highscore
{
    public string username;
    public int score;

    public Highscore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }

}