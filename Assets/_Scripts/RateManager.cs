using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateManager : MonoBehaviour {

    public GameObject gemsText;
    public string storeLink = "https://";
    private bool RatePressed = false;
    //public Animator rateAnimation;

    private int rated = 0;

    void Start()
    {
        
        rated = PlayerPrefs.GetInt("Rated", 0);

        if (rated == 1 && gemsText !=null)
        {
            //rateAnimation.enabled = false;
            gemsText.SetActive(false);
        }
        else if(gemsText != null)
            gemsText.SetActive(true);

    }


    public void RateBtn()
    {
        if(rated == 0)
        {
            rated = 1;
            PlayerPrefs.SetInt("Rated", rated);
            Application.OpenURL(storeLink);
            RatePressed = true;
            
        }
        else
        {
            Application.OpenURL(storeLink);
        }
        

    }

    private void OnApplicationFocus(bool focus)
    {
        if(RatePressed)
        {
            CoinManager.Instance.Coins += 10;
            gemsText.SetActive(false);
            RatePressed = false;
        }
    }
}
