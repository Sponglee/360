using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FunctionHandler : MonoBehaviour {

    
	
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
        MenuStartTimed();
    }

    //Restarts game
    public void MenuRestart()
    {
        MenuStartTimed();
    }


 

    //Quit
    public void MenuQuit()
    {
        Application.Quit();
    }


    public void LocalMenu(GameObject localMenu)
    {
        StartCoroutine(StopMenu(0, localMenu, true));
    }

    private IEnumerator StopMenu(float dir, GameObject tmpMenu, bool horizontal)
    {
        Vector3 offset;

        float timeOfTravel = 1; //time after object reach a target place 
        float currentTime = 0; // actual floting time 
        float normalizedValue;

        

        if (horizontal)
        {
            offset = new Vector3(dir + 10f, 0, 0);
           // difference = Mathf.Abs(tmpMenu.transform.position.x - gameObject.transform.position.x);
        }
        else
        {
            offset = new Vector3(0, dir + 10f, 0);
           // difference = Mathf.Abs(tmpMenu.transform.position.y - gameObject.transform.position.y);
        }


        if (dir==0)
        {
            while (currentTime <= timeOfTravel)
            {
                currentTime += Time.deltaTime;
                normalizedValue = currentTime / timeOfTravel; // we normalize our time 
                Debug.Log("runnin +  " + timeOfTravel + "  :  " + currentTime);
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
                Debug.Log("runnin BACK");
                tmpMenu.transform.position = Vector3.Lerp(tmpMenu.transform.position, tmpMenu.transform.parent.position + offset, normalizedValue);

                yield return null;
            }
            
        }
    }

    public void BackFromLocal(GameObject localMenu)
    {
        StartCoroutine(StopMenu(1900, localMenu, true));

    }



    public void Shop(GameObject localMenu)
    {
        StartCoroutine(StopMenu(0, localMenu, false));
    }

    public void BackFromShop(GameObject localMenu)
    {
        StartCoroutine(StopMenu(-2050, localMenu, false));

    }


    //Time game
    public void MenuStartTimed()
    {
    
        if (GameManager.Instance != null)
        {
           
            GameManager.Instance.NewGame();
            
        }
        else if (TitleManager.Instance != null && PlayerPrefs.GetInt("GameMode", 0) == 0)
        {
            TitleManager.Instance.TitleNewGame();
        }
        FadeOut();
        SceneManager.LoadScene("main");
       // SceneManager.UnloadScene("title");
    }


    //Relax game
    public void MenuStartRelax()
    {

   
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
           
        }
        else if (TitleManager.Instance != null && PlayerPrefs.GetInt("GameMode", 1) == 1)
        {
            TitleManager.Instance.TitleNewGame();
        }
        FadeOut();
        SceneManager.LoadScene("relax");
    }

    public void MainMenu()
    {
        if(GameManager.Instance != null)
        {
            if (!GameManager.Instance.GameOverBool)
                GameManager.Instance.SaveGame();
            else
                GameManager.Instance.NewGame();
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
        TitleManager.Instance.TitleNewGame();
        SceneManager.LoadScene("title");

    }


    public void MoreCoins()
    {
        CoinManager.Instance.Coins += 10;
        
    }



}
