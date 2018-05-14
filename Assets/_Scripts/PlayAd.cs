using UnityEngine.Advertisements;
using UnityEngine;

public class PlayAd : MonoBehaviour {

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult});
        }
           
    }


    private void HandleAdResult(ShowResult result)
    {

        switch (result)
        {
            case ShowResult.Finished:
                CoinManager.Instance.Coins+=5;
                break;
            case ShowResult.Skipped:
                CoinManager.Instance.Coins += 2;
                break;
            case ShowResult.Failed:
                Debug.Log("Failed");
                break;

        }

    }
}
