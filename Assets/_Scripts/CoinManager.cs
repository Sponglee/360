﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : Singleton<CoinManager> {


    //Currency
    [SerializeField]
    private int coins;
    public int Coins
    {get{return coins;}set{coins = value;}}


    public Text coinText;
    public Text menuCoinText;

    // Use this for initialization
    void Start () {

        coinText = GameManager.Instance.ui.transform.GetChild(3).gameObject.GetComponent<Text>();
        menuCoinText = GameManager.Instance.menu.transform.GetChild(3).gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        coinText.text = coins.ToString();
        menuCoinText.text = coins.ToString();
    }
}
