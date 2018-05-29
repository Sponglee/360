using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : Singleton<CoinManager> {


    //Currency
    [SerializeField]
    private int coins;
    public int Coins
    {   get
        {
          
            return coins;
        }
        set
        {
            coins = value;
            coinText.text = coins.ToString();
            PlayerPrefs.SetInt("Coin", coins);
        }
    }


    public Text coinText;
    

    // Use this for initialization
    void Start () {

        
        //persistant coin manager
        DontDestroyOnLoad(gameObject);

        coins = PlayerPrefs.GetInt("Coin", 0);
        coinText.text = coins.ToString();
        
	}
	

}
