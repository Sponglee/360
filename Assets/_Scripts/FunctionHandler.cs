using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionHandler : MonoBehaviour {

	
    public void OpenMenuHandler()
    {
        GameManager.Instance.OpenMenu();
    }


    public void RestartHandler()
    {
        GameManager.Instance.Restart();
    }

    public void ContinueHandler()
    {
        GameManager.Instance.Continue();
    }


    public void ChangeThemeHandler(int index)
    {
        GameManager.Instance.ChangeTheme(index);
    }
}
