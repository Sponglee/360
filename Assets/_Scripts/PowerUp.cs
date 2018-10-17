using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public GameObject tooltip;
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
            {
                gameObject.GetComponent<Image>().color = new Color32(156,87,71,246);
                tooltip.SetActive(true);
                

                //Enable crosses on each square
                GameObject[] tmpSquarePwrUps = GameObject.FindGameObjectsWithTag("square");

                foreach (var tmp in tmpSquarePwrUps)
                {
                    if (tmp.transform.parent.CompareTag("spot"))
                        tmp.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
            else
            {
                //count cost of selectSquare
                if(GameManager.Instance.SquareDestroyed)
                {
                    CoinManager.Instance.Coins -= pCost;
                    GameManager.Instance.SquareDestroyed = false;
                }
                gameObject.GetComponent<Image>().color = new Color32(255,255,255,246);
                tooltip.SetActive(false);

                //Disable crosses on each square
                GameObject[] tmpSquarePwrUps = GameObject.FindGameObjectsWithTag("square");

                foreach (var tmp in tmpSquarePwrUps)
                {
                    if(tmp.transform.parent.CompareTag("spot"))
                        tmp.transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }


    [SerializeField]
    private int pCost;

    public GameObject powerUpPref;


    private void Update()
    {
        //Deselect when gamemanager bool is false
        if (selectionUI == true)
        {
            switch (gameObject.transform.GetSiblingIndex())
            {
                case 0:
                    SelectionUI = GameManager.Instance.SelectHammer;
                    break;
                case 1:
                    SelectionUI = GameManager.Instance.SelectBomb;
                    break;
                case 2:
                    SelectionUI = GameManager.Instance.SelectDrill;
                    break;
            }

        }
    }


    //Square power up
    public void SelectSquare(int powerUp)
    {
        //GameManager.Instance.tutorialManager.powerUpAnim[powerUp].SetBool("Highlight", false);

        SelectionUI = true;
        switch (powerUp)
        {
            case 0:
                GameManager.Instance.SelectHammer = true;
                GameManager.Instance.SelectBomb = false;
                GameManager.Instance.SelectDrill = false;
                break;
            case 1:
                GameManager.Instance.SelectBomb = true;
                GameManager.Instance.SelectDrill = false;
                GameManager.Instance.SelectHammer = false;
                break;
            case 2:
                GameManager.Instance.SelectDrill = true;
                GameManager.Instance.SelectHammer = false;
                GameManager.Instance.SelectBomb = false;
                break;
        }
    }

    public void DeselectSquare(int powerUp)
    {
        SelectionUI = false;
        switch (powerUp)
        {
            case 0:
                GameManager.Instance.SelectHammer = false;
                break;
            case 1:
                GameManager.Instance.SelectBomb = false;
                break;
            case 2:
                GameManager.Instance.SelectDrill = false;
                break;
        }


    }





    //Click handler
    public void PowerUpPressed()
    {
        int index = gameObject.transform.GetSiblingIndex();
        if(!GameManager.Instance.gameOverInProgress && !GameManager.Instance.GameOverBool)
        {
            if (CoinManager.Instance.Coins >= pCost)
            {


                switch (index)
                {
                    //DRILL
                    case 2:
                        {
                            if (!SelectionUI)
                                SelectSquare(2);
                            else
                            {
                                DeselectSquare(0);
                                DeselectSquare(1);
                                DeselectSquare(2);
                            }

                        }

                        break;
                    //BOMB
                    case 1:
                        {

                            if (!SelectionUI)
                                SelectSquare(1);
                            else
                            {
                                DeselectSquare(0);
                                DeselectSquare(1);
                                DeselectSquare(2);
                            }


                        }

                        break;
                    //HAMMER
                    case 0:
                        {
                            if (!SelectionUI)
                                SelectSquare(0);
                            else
                            {
                                DeselectSquare(0);
                                DeselectSquare(1);
                                DeselectSquare(2);
                            }
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
      

   

    
}



      







