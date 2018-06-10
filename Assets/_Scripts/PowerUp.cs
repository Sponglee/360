using UnityEngine.UI;
using UnityEngine;
using System.Collections;

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
        GameManager.Instance.tutorialManager.powerUpAnim[0].SetBool("Highlight", false);
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
                //DRILL
                case 2:
                    {
                        if (GameManager.Instance.currentSpot.transform.childCount>0 && GameManager.Instance.currentSpot.transform.childCount < 5)
                        {

                            Debug.Log("NO ROTATION");


                        
                            //********************TUTORIAL*********DRILL
                            if (GameManager.Instance.tutorialManager.tutorialStep == 5)
                            {
                              
                                
                                GameManager.Instance.tutorialManager.powerUpAnim[2].SetBool("Highlight", false);


                                GameManager.Instance.tutorialManager.tutorialTrigger.Invoke();
                                CoinManager.Instance.Coins += 5;
                                GameManager.Instance.tutorialManager.button.SetActive(true);
                                StartCoroutine(StopCloseTut());
                            }
                            //*****************************


                            CoinManager.Instance.Coins -= pCost;
                            GameObject tmp = Instantiate(powerUpPref, GameManager.Instance.currentSpawn.transform.position, Quaternion.identity);
                            tmp.transform.SetParent(GameManager.Instance.currentSpot.transform);

                        }
                        else
                        {
                            AudioManager.Instance.PlaySound("stop");
                        }

                    }
                    
                    break;
                //BOMB
                case 1:
                    {
                        if (GameManager.Instance.currentSpot.transform.childCount > 0 && GameManager.Instance.currentSpot.transform.childCount < 5)
                        {
                           

                            CoinManager.Instance.Coins -= pCost;
                            GameObject tmp = Instantiate(powerUpPref, GameManager.Instance.currentSpawn.transform.position, Quaternion.identity);
                            tmp.transform.SetParent(GameManager.Instance.currentSpot.transform);

                                Debug.Log("DRILL NO ROTATION");


                           
                            //********************TUTORIAL*********BOMB
                            if (GameManager.Instance.tutorialManager.tutorialStep == 4)
                            {
                                GameManager.Instance.tutorialManager.powerUpAnim[1].SetBool("Highlight", false);
                                GameManager.Instance.tutorialManager.powerUpAnim[2].SetBool("Highlight", true);



                                GameManager.Instance.tutorialManager.tutorialTrigger.Invoke();
                                CoinManager.Instance.Coins += 3;
                                StartCoroutine(StopTutDrill());
                            }

                            //*****************************

                        }
                        else
                        {
                            AudioManager.Instance.PlaySound("stop");
                        }
                    }

                    break;
                //HAMMER
                case 0:
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

    //Coroutine to fill squares for the Drill tutorial
    public IEnumerator StopTutDrill()
    {
        yield return new WaitForSeconds(1f);
        //PREPEARE FOR DRILL TUTORIAL
        int tutScore = 2;

        for (int i = 0; i < 5; i++)
        {
            GameObject tutSpawn;

            if (GameManager.Instance.currentSpot.transform.childCount < 4)
            {
                tutSpawn = Instantiate(GameManager.Instance.squarePrefab, GameManager.Instance.currentSpawn.transform.position, Quaternion.identity, GameManager.Instance.currentSpawn.transform);
                tutSpawn.GetComponent<Square>().Score = tutScore;
                yield return new WaitForSeconds(0.2f);
                
            }
            tutScore *= 2;
        }
    }

    //Close tutorial cooldown
    public IEnumerator StopCloseTut()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.tutorialManager.tutorialCanvas.gameObject.SetActive(false);

    }
}



      







