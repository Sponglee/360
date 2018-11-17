//using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : Singleton<CoinManager> {


    //For editor convenience, pls ignore
    public GameObject fadeCanvas;

    //Refference for shop currency info
    public Text shopCoinText;
        
    public int fullAdWatch;
    public int partAdWatch;
    //Theme unlocks
    private int skinAvailability;
    public int SkinAvailability
    {
        get
        {
            return skinAvailability;
        }

        set
        {
            skinAvailability = value;
            PlayerPrefs.SetInt("SkinAvailability", CoinManager.Instance.skinAvailability);
        }
    }


    //Currency
    [SerializeField]
    private int coins;
    public int contCost;
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
            if (shopCoinText !=null)
            {
                //Update coins for the shop aswell
                shopCoinText.text = coins.ToString();
            }
            
        }
    }

  
    public Text coinText;
    

    // Use this for initialization
    void Awake() {


        //Initialize the ad and skins
                                                                    //Advertisement.Initialize("3af5ea4b-4854-464f-b6cd-6286807539a8");
        //For editor usage, pls ignore
        fadeCanvas.SetActive(true);
        
        
        
        
        //Check what skins are available
        SkinAvailability = PlayerPrefs.GetInt("SkinAvailability", 1);
        //persistant coin manager
        DontDestroyOnLoad(gameObject);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }


        coins = PlayerPrefs.GetInt("Coin", 20);
        coinText.text = coins.ToString();
        
	}

    //Open up an ad
    public void ShowAd()
    {
        //if (Advertisement.IsReady())
        //{
        //    Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult });
        //    Time.timeScale = 0;
        //}


        //DEBUG WATCH AD
        //CoinManager.Instance.Coins += fullAdWatch;
    }

    public void MenuAd()
    {
                                                                                        //if (Advertisement.IsReady())
                                                                                        //{
                                                                                        //    if (GameManager.Instance != null)
                                                                                        //    GameManager.Instance.AdInProgress = true;
                                                                                        //    Advertisement.Show("video", new ShowOptions() { resultCallback = HandleAdResultMenu });
                                                                                        //    Time.timeScale = 0;
                                                                                        //}

        //DEBUG WATCH AD
        //CoinManager.Instance.Coins += fullAdWatch;
    }

    //Recieve result from watching
                                                                                        //private void HandleAdResult(ShowResult result)
                                                                                        //{

                                                                                        //    switch (result)
                                                                                        //    {
                                                                                        //        case ShowResult.Finished:
                                                                                        //            {
                                                                                        //                CoinManager.Instance.Coins += fullAdWatch;
                                                                                        //                Time.timeScale = 1;
                   
                                                                                        //                break;
                                                                                        //            }
                                                                                        //        case ShowResult.Skipped:
                                                                                        //            {
                                                                                        //                //CoinManager.Instance.Coins += partAdWatch;
                                                                                        //                Time.timeScale = 1;
                    
                                                                                        //                break;
                                                                                        //            }
                                                                                        //        case ShowResult.Failed:
                                                                                        //            {
                                                                                        //                Time.timeScale = 1;
                                                                                        //                Debug.Log("Failed");
                    
                                                                                        //                break;

                                                                                        //            }

                                                                                        //    }

                                                                                        //}

                                                                                        ////Recieve result from watching
                                                                                        //private void HandleAdResultMenu(ShowResult result)
                                                                                        //{

                                                                                        //    switch (result)
                                                                                        //    {
                                                                                        //        case ShowResult.Finished:
                                                                                        //            {
                    
                                                                                        //                Time.timeScale = 1;

                                                                                        //                break;
                                                                                        //            }
                                                                                        //        case ShowResult.Skipped:
                                                                                        //            {
                                                                                        //                //CoinManager.Instance.Coins += partAdWatch;
                                                                                        //                Time.timeScale = 1;

                                                                                        //                break;
                                                                                        //            }
                                                                                        //        case ShowResult.Failed:
                                                                                        //            {
                                                                                        //                Time.timeScale = 1;
                                                                                        //                Debug.Log("Failed");

                                                                                        //                break;

                                                                                        //            }

                                                                                        //    }

                                                                                        //}
}
