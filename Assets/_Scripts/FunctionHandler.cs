using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        GameManager.Instance.Continue();
    }


    public void ChangeThemeHandler(GameObject index)
    {
        int themeIndex = index.transform.GetSiblingIndex();
        PlayerPrefs.SetInt("Theme", themeIndex);

        NewGame();
        //InitializeTheme();
    }



    //Restarts game
    public void MenuRestart()
    {
        NewGame();
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
                Debug.Log("runnin");
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

    public void NewGame()
    {
        SceneManager.LoadScene("main");
       // SceneManager.UnloadScene("title");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("title");
        //SceneManager.UnloadScene("main");
    }





}
