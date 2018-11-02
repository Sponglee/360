using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FunctionHandler : Singleton<FunctionHandler> {

    
   
    public void OpenMenuHandler()
    {
        GameManager.Instance.OpenMenu();
    }


    public void RestartHandler()
    {
        GameManager.Instance.Restart();
    }

    public void ContinueHandler()
    {
        if (CoinManager.Instance.Coins >= CoinManager.Instance.contCost)
        {
            CoinManager.Instance.Coins -= CoinManager.Instance.contCost;
            GameManager.Instance.Continue();
        }
        else
        {
            CoinManager.Instance.ShowAd();
        }
       
    }



    public void ShowAdHandler()
    {
        CoinManager.Instance.ShowAd();
    }

    public void EmailUs()
    {
        //email Id to send the mail to
        string email = "solidHinken@gmail.com";
        //subject of the mail
        string subject = MyEscapeURL("360!BLOCKS FEEDBACK");
        //body of the mail which consists of Device Model and its Operating System
        string body = MyEscapeURL("Please Enter your message here\n\n\n\n" +
         "________" +
         "\n\nPlease Do Not Modify This\n\n" +
         "Model: " + SystemInfo.deviceModel + "\n\n" +
            "OS: " + SystemInfo.operatingSystem + "\n\n" +
         "________\n\n");
        //Open the Default Mail App
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }




//THEME CHANGER

public void ChangeThemeHandler(GameObject index)
    {
        int themeIndex = index.transform.GetSiblingIndex();
        //Check if skin is in availability (bit flag)
        if ((CoinManager.Instance.SkinAvailability & 1 << themeIndex) == 1 << themeIndex)
        {
            

            PlayerPrefs.SetInt("Theme", themeIndex);
            MainMenu();

        }
        else
        {
            int cost = int.Parse(index.transform.GetChild(0).GetComponentInChildren<Text>().text);
            if(CoinManager.Instance.Coins >= cost)
            {
                CoinManager.Instance.Coins -= cost;

                //bitshift index for memorizing unlocks
                CoinManager.Instance.SkinAvailability += 1 << themeIndex;
               
                PlayerPrefs.SetInt("Theme", themeIndex);
                MainMenu();
            }

            else
            {
                Debug.Log("YOU DONT HAVE THE SKIN. BUY IT? " + cost);
            }
        }






        //InitializeTheme();
    }

    //Watch tutorial again
    public void TutorialAgain()
    {
       
        PlayerPrefs.SetInt("TutorialStep", 0);
        MenuStart(1);
    }

    //Restarts game
    public void MenuRestart()
    {
        MenuStart(1);
    }


 

    //Quit
    public void MenuQuit()
    {
        Application.Quit();
    }


    //direction ****** 0 - down  1 - right 2 - up ***********
    private IEnumerator StopMenu(float dir, GameObject tmpMenu, int direction)
    {
        Vector3 offset;

        float timeOfTravel = 1; //time after object reach a target place 
        float currentTime = 0; // actual floting time 
        float normalizedValue;

        

        if (direction == 0)
        {
            offset = new Vector3(dir + 10f, 0, 0);
           // difference = Mathf.Abs(tmpMenu.transform.position.x - gameObject.transform.position.x);
        }
        else if (direction == 1)
        {
            offset = new Vector3(0, dir + 10f, 0);
           // difference = Mathf.Abs(tmpMenu.transform.position.y - gameObject.transform.position.y);
        }
        //For leaderboard (down)
        else
        {
            offset = new Vector3(0, dir - 10f, 0);
            // difference = Mathf.Abs(tmpMenu.transform.position.y - gameObject.transform.position.y);
        }

        if (dir==0)
        {
            while (currentTime <= timeOfTravel)
            {
                currentTime += Time.deltaTime;
                normalizedValue = currentTime / timeOfTravel; // we normalize our time 
                //Debug.Log("runnin +  " + timeOfTravel + "  :  " + currentTime);
                tmpMenu.transform.position = Vector3.Lerp(tmpMenu.transform.position, tmpMenu.transform.parent.position, normalizedValue);
                yield return null;  
            }
        }
        else
        {
            while (currentTime <= timeOfTravel)
            {
                currentTime += Time.deltaTime;
                normalizedValue = currentTime / timeOfTravel; // we normalize our time 
                //Debug.Log("runnin BACK");
                tmpMenu.transform.position = Vector3.Lerp(tmpMenu.transform.position, tmpMenu.transform.parent.position + offset, normalizedValue);

                yield return null;
            }
            
        }
    }


    //Pick Leaderboard table
    public void PickTable(int dbIndex)
    {
        Highscores.Instance.DownloadHighscores(dbIndex);
    }


    //MENU MOVE FUNCTIONS
    public void LocalMenu(GameObject localMenu)
    {
        StartCoroutine(StopMenu(0, localMenu, 1));
    }


    public void Shop(GameObject localMenu)
    {
        StartCoroutine(StopMenu(0, localMenu, 1));
    }

    //For keeping track of board state
    public bool leaderBoardOpen = false;
    //LEADERBOARD FUNCTION
    public void LeaderBoards(GameObject localMenu)
    {
        leaderBoardOpen = true;
        Highscores.Instance.DownloadHighscores(0);
        StartCoroutine(StopMenu(0, localMenu, 2));
    }

    //RETURN TO MAIN MOVE FUNCTIONS
    public void BackFromShop(GameObject localMenu)
    {
        StartCoroutine(StopMenu(2500, localMenu, 1));

    }

    public void BackFromLocal(GameObject localMenu)
    {
        StartCoroutine(StopMenu(2500, localMenu, 0));

    }

    public void BackFromBoard(GameObject localMenu)
    {
        leaderBoardOpen = false;
        StartCoroutine(StopMenu(-2500, localMenu, 2));

    }

    ////Time game
    //public void MenuStartTimed()
    //{
    //    //Ingame restart
    //    if (GameManager.Instance != null)
    //    {
           
    //        GameManager.Instance.NewGame();
            
    //    }
    //    //New game if mode is not 1 (timed) in title menu
    //    else if (TitleManager.Instance != null && PlayerPrefs.GetInt("GameMode", 1) != 1)
    //    {
    //        //Debug.Log(PlayerPrefs.GetInt("GameMode",99)); 
    //        TitleManager.Instance.TitleNewGame();
    //    }
    //    FadeOut();
    //    SceneManager.LoadScene("main");
    //   // SceneManager.UnloadScene("title");
    //}


    //Relax game
    public void MenuStart(int gameMode)
    {


        PlayerPrefs.SetInt("GameMode", gameMode);


        Debug.Log(PlayerPrefs.GetInt("GameMode", 99));


        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
           
        }
        //New game if mode is not 0 (relax) in title menu
        else /*if (TitleManager.Instance != null && PlayerPrefs.GetInt("GameMode", 0) != 0)*/
        {
            TitleManager.Instance.TitleNewGame();
        }
        FadeOut();
        SceneManager.LoadScene("main");
    }

    ////Relax game
    //public void MenuStartDzen()
    //{


    //    if (GameManager.Instance != null)
    //    {
    //        GameManager.Instance.NewGame();

    //    }
    //    //New game if mode is not 2 (dzen) in title menu
    //    else if (TitleManager.Instance != null && PlayerPrefs.GetInt("GameMode", 2) != 2)
    //    {
    //        //Debug.Log(PlayerPrefs.GetInt("GameMode", 99));
    //        TitleManager.Instance.TitleNewGame();
    //    }
    //    FadeOut();
    //    SceneManager.LoadScene("dzen");
    //}

    public void MainMenu()
    {
        if(GameManager.Instance != null)
        {
            if (!GameManager.Instance.GameOverBool)
                GameManager.Instance.SaveGame();
            else
                GameManager.Instance.NewGame();
            CoinManager.Instance.MenuAd();
        }
       
        SceneManager.LoadScene("title");
        //SceneManager.UnloadScene("main");
    }







    //Fade In functions


    public void FadeIn(CanvasGroup fadeGroup)
    {
        //FadeIn
        if (Time.time < 3f)
            fadeGroup.alpha = 1 - Time.time;
    }

    public void FadeOut()
    {
        CanvasGroup fadeGroup = FindObjectOfType<CanvasGroup>();
        //FadeOut
        if (Time.time > 0)
        {
            if (fadeGroup.alpha >= 1)
            {
                //SceneManager.LoadScene(String.Format("{0}", scene));
            }
        }
    }



    public Sprite volumeIcon;
    public Sprite volumeMute;

    public GameObject volumeUI;

    
    public void VolumeHandler(float value)
    {
        AudioManager.Instance.VolumeChange(value);

        volumeUI = GameObject.FindGameObjectWithTag("volume");

        if (value == 0)
        {
            volumeUI.GetComponent<Image>().sprite = volumeMute;
        }
        else
            volumeUI.GetComponent<Image>().sprite = volumeIcon;

    }


  


    //********************DEBUG FUNCTIONS ------******************************
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        
    }
    
    public void ResetHighscores()
    {
        PlayerPrefs.SetInt("HighscoreTimed", 0);
        PlayerPrefs.SetInt("HighscoreRelax", 0);
        PlayerPrefs.SetInt("HighscoreDzen", 0);
        
        TitleManager.Instance.TitleNewGame();
        SceneManager.LoadScene("title");

    }


    public void MoreCoins()
    {
        CoinManager.Instance.Coins += 10;
        
    }

    public void ShowURL(string targetUrl)
    {
        Application.OpenURL(targetUrl);
    }

    public int i = 0;

    public void DebugScoreAdd()
    {
        
            i++;
            Highscores.Instance.AddNewHighscore(string.Format("user{0}", i), 10000+i,0);
    }

    
}
