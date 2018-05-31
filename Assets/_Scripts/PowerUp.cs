using UnityEngine.UI;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //select toggle color change
    private bool selectionUI = false;
    public bool SelectionUI
    {
        get
        {
            return selectionUI;
        }

        set
        {
            selectionUI = value;
            //Switch color on valuechange
            if(selectionUI)
                gameObject.GetComponent<Image>().color = new Color32(156,87,71,246);
            else
            {
                //count cost of selectSquare
                if(GameManager.Instance.SquareDestroyed)
                {
                    CoinManager.Instance.Coins -= pCost;
                    GameManager.Instance.SquareDestroyed = false;
                }
                gameObject.GetComponent<Image>().color = new Color32(255,255,255,246);
            }
        }
    }


    [SerializeField]
    private int pCost;

    public GameObject powerUpPref;


    private void Update()
    {
        //Deselect when gamemanager bool is false
        if(selectionUI==true)
        {
            SelectionUI = GameManager.Instance.SelectPowerUp;
        }
    }


    //Square power up
    public void SelectSquare()
    {
        SelectionUI = true;
        GameManager.Instance.SelectPowerUp = true;  

    }

    public void DeselectSquare()
    {
        SelectionUI = false;
        GameManager.Instance.SelectPowerUp = false;

    }





    //Click handler
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
                    {
                        if (!SelectionUI)
                            SelectSquare();
                        else
                            DeselectSquare();
                    }
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



      







