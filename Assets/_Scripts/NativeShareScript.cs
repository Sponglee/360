﻿using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class NativeShareScript : MonoBehaviour
{
    public GameObject CanvasShareObj;

    //For sharing
    public GameObject snap;
    public GameObject pause;
    public GameObject powerUpPanel;


    private bool isProcessing = false;
    private bool isFocus = false;


    public string subject, ShareMessage, url;
    public string ScreenshotName = "screenshot.png";
   



public void ShareBtnPress()
    {
        if (!isProcessing && !GameManager.Instance.MenuUp)
        {
            CanvasShareObj.SetActive(true);
            StartCoroutine(CallSocialShareRoutine());
        }
    }


    public struct ConfigStruct
    {
        public string title;
        public string message;
    }
    [DllImport("__Internal")] private static extern void showAlertMessage(ref ConfigStruct conf);
    public struct SocialSharingStruct
    {
        public string text;
        public string url;
        public string image;
        public string subject;
    }
    [DllImport("__Internal")] private static extern void showSocialSharing(ref SocialSharingStruct conf);
    public void CallSocialShare(string title, string message)
    {
        ConfigStruct conf = new ConfigStruct();
        conf.title = title;
        conf.message = message;
        showAlertMessage(ref conf);
        isProcessing = false;
    }
    public static void CallSocialShareAdvanced(string defaultTxt, string subject, string url, string img)
    {
        SocialSharingStruct conf = new SocialSharingStruct();
        conf.text = defaultTxt;
        conf.url = url;
        conf.image = img;
        conf.subject = subject;
        showSocialSharing(ref conf);
    }
    IEnumerator CallSocialShareRoutine()
    {
        isProcessing = true;
        string screenShotPath = Application.persistentDataPath + "/" + ScreenshotName;
        ScreenCapture.CaptureScreenshot(ScreenshotName);
        yield return new WaitForSeconds(1f);
        CallSocialShareAdvanced(ShareMessage, subject, url, screenShotPath);

        //yield return new WaitUntil(() => isFocus);
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







