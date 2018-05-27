using UnityEngine.UI;
using UnityEngine;

public class UIScript : Singleton<UIScript> {


    [SerializeField]
    private Text pointsText;

    public Text[] ValueTextAray;



    public void ShowLeaderBoards()
    {
        Debug.Log("SHown");
        PlayGamesScript.ShowLeaderBoardsUI();
    }
    
    public void UpdatePointsText()
    {
        pointsText.text = GameManager.Instance.scores.ToString();
    }


    public void UpdateAll()
    {
        for (int i = 0; i < CloudVariables.ImportantValues.Length; i++)
        {
            //ValueTextArray[i].text = CloudVariables.ImportantValues[i].ToString();
        }
    }


    public void Save()
    {
        //PlayGamesScript.Instance.SaveData();
    }

}
