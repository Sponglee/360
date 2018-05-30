using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    [SerializeField]
    private int pCost;
    
	// Use this for initialization
	void Start () {
       
	}
	
	
    public void PowerUpPressed()
    {
        if (CoinManager.Instance.Coins >= pCost)
        {
            CoinManager.Instance.Coins -= pCost;


        }
        else
        {
            CoinManager.Instance.ShowAd();
        }
    }
}
