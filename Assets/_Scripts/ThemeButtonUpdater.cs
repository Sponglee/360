using UnityEngine;
using UnityEngine.UI;

public class ThemeButtonUpdater : MonoBehaviour {

    public int itemCost;

	// Use this for initialization
	void Awake ()
    {
        UpdateThemeButton(gameObject);
    }

    public void UpdateThemeButton(GameObject gameObject)
    {
        if ((CoinManager.Instance.SkinAvailability & 1 << gameObject.transform.GetSiblingIndex()) == 1 << gameObject.transform.GetSiblingIndex())
        {
            gameObject.transform.GetChild(0).GetComponentInChildren<Text>().text = "SELECT";
            gameObject.transform.GetChild(0).GetComponentInChildren<Text>().color = Color.gray;
            gameObject.transform.GetChild(0).GetChild(1).GetComponentInChildren<Text>().color = Color.gray;
            gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
        else
        {
        
                gameObject.transform.GetChild(0).GetComponentInChildren<Text>().text = itemCost.ToString();
                gameObject.transform.GetChild(0).GetComponentInChildren<Text>().color = Color.white;
                gameObject.transform.GetChild(0).GetChild(1).GetComponentInChildren<Text>().color = Color.white;
                gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            
        }

        if (PlayerPrefs.GetInt("Theme", 0) == gameObject.transform.GetSiblingIndex())
        {
            gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(0, 0, 0, 100);

        }
    }
}



