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
           

            switch (index)
            {
                //BOMB
                case 0:
                    {
                        if (GameManager.Instance.currentSpot.transform.childCount>0)
                        {
                            CoinManager.Instance.Coins -= pCost;
                            GameObject tmp = Instantiate(powerUpPref, GameManager.Instance.currentSpawn.transform.position, Quaternion.identity);
                            tmp.transform.SetParent(GameManager.Instance.currentSpot.transform);
                        }
                        else
                        {
                            Debug.Log("NOPE");
                        }

                    }
                    
                    break;
                //DRILL
                case 1:
                    {
                        if (GameManager.Instance.currentSpot.transform.childCount > 0)
                        {
                            CoinManager.Instance.Coins -= pCost;
                            GameObject tmp = Instantiate(powerUpPref, GameManager.Instance.currentSpawn.transform.position, Quaternion.identity);
                            tmp.transform.SetParent(GameManager.Instance.currentSpot.transform);

                        }
                        else
                        {
                            Debug.Log("NOPE");
                        }
                    }

                    break;
                //SELECT
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



      







