using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class NativeShareScript : MonoBehaviour
{
    public GameObject CanvasShareObj;

    //For sharing
    public GameObject snap;
    public GameObject pause;
    public GameObject powerUpPanel;


    private bool isProcessing = false;
    private bool isFocus = false;

    //public void Start()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}
    public void ShareBtnPress()
    {
        if (!isProcessing && !GameManager.Instance.MenuUp)
        {
            CanvasShareObj.SetActive(true);
            StartCoroutine(ShareScreenshot());
        }
    }

    IEnumerator ShareScreenshot()
    {
        isProcessing = true;
        GameManager.Instance.shareButton.SetActive(false);
        GameManager.Instance.powerUpPanel.SetActive(false);
        GameManager.Instance.ui.transform.GetChild(0).gameObject.SetActive(false);



        yield return new WaitForEndOfFrame();
        Time.timeScale = 0;
        ScreenCapture.CaptureScreenshot("screenshot.png");
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();



        string destination = Path.Combine(Application.persistentDataPath, "screenshot.png");

        yield return new WaitForSecondsRealtime(0.2f);

        if (!Application.isEditor)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"),
                uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"),
                "Can you beat my score? #360!  https://play.google.com/store/apps/details?id=com.Ace.threesixty");
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser",
                intentObject, "Share your new score");
            currentActivity.Call("startActivity", chooser);

            yield return new WaitForSecondsRealtime(1);
        }

      


        yield return new WaitUntil(() => isFocus);
        CanvasShareObj.SetActive(false);

        GameManager.Instance.shareButton.SetActive(true);
        GameManager.Instance.powerUpPanel.SetActive(true);
        GameManager.Instance.ui.transform.GetChild(0).gameObject.SetActive(true);


        isProcessing = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        Time.timeScale = 1;
        isFocus = focus;
        GameManager.Instance.shareButton.SetActive(true);
        GameManager.Instance.ui.transform.GetChild(0).gameObject.SetActive(true);
        GameManager.Instance.powerUpPanel.SetActive(true);
    }
}