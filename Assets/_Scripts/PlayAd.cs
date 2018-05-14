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
                Debug.Log("Player Gains +5 gems");
                break;
            case ShowResult.Skipped:
                Debug.Log("Didn't watch whole ad");
                break;
            case ShowResult.Failed:
                Debug.Log("Failed");
                break;

        }

    }
}
