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
            Debug.Log("CHANGED " + coins);
            PlayerPrefs.SetInt("Coin", coins);
        }
    }


    public Text coinText;
    public Text menuCoinText;

    // Use this for initialization
    void Start () {
        coins = PlayerPrefs.GetInt("Coin", 0);
        if (coinText == null)
            coinText = GameManager.Instance.ui.transform.GetChild(3).gameObject.GetComponent<Text>();
        if(menuCoinText == null)
            menuCoinText= GameManager.Instance.menu.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
     
        coinText.text = coins.ToString();
        menuCoinText.text = coins.ToString();
    }
}
