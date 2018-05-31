using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    private int pCost;

    public GameObject powerUpPref;

    // Use this for initialization
    void Start()
    {

    }

    public void SelectSquare()
    {
        Debug.Log("SELECT");
    }



    public void PowerUpPressed()
    {
        int index = gameObject.transform.GetSiblingIndex();

        if (CoinManager.Instance.Coins >= pCost)
        {
            CoinManager.Instance.Coins -= pCost;

            switch (index)
            {
                case 0:
                    Instantiate(powerUpPref);
                    break;
                case 1:
                    Instantiate(powerUpPref);
                    break;
                case 2:
                    SelectSquare();
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("REEEE");
            CoinManager.Instance.ShowAd();
        }
    }


}



      







