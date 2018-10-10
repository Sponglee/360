using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateManager : MonoBehaviour {

    public GameObject gemsText;
    public string storeLink = "https://";
    public Animator rateAnimation;

    private int rated = 0;

    void Start()
    {
        
        rated = PlayerPrefs.GetInt("Rated", 0);

        if (rated == 1)
        {
            rateAnimation.enabled = false;
            gemsText.SetActive(false);
        }
        else
            gemsText.SetActive(true);

    }


    public void RateBtn()
    {
        if(rated == 0)
        {
            rated = 1;
            PlayerPrefs.SetInt("Rated", rated);
            Application.OpenURL(storeLink);
            CoinManager.Instance.Coins += 10;
            gemsText.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            Application.OpenURL(storeLink);
        }
        

    }

    
}
