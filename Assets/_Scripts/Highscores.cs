﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Highscores : Singleton<Highscores>
{

    public string privateCode;
    public string publicCode;
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;

   

    public void AddNewHighscore(string username, int score)
    {
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

    public void DownloadHighscores()
    {
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
            print("Error Downloading: " + www.error);
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