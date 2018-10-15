using UnityEngine.UI;
using UnityEngine;
using System;




public class TitleManager : Singleton<TitleManager> {

    //debug NAME
    public Text debugName;

    public int themeIndex;
    public Text highScoreTimedText;
    public Text highScoreRelaxText;
    public Text highScoreDzenText;
    public Text shopCurrencyText;

    public GameSerializer serializer;

    public Transform RotatorPref;
    public GameObject styleHolderPrefab;
    public GameObject menu;
    public GameObject wheelPrefab;
    public GameObject backPrefab;

    [SerializeField]
    private Slider volumeSlider;

    // Helps ApplyStyle to grab numbers/color

    void ApplyThemeFromHolder(int index)
    {
        //wheelPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].wheelPref;
      
        backPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].backPref;

        //fontPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].fontPref;


        menu.transform.GetChild(0).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;


        //Options menu
        menu.transform.GetChild(0).GetChild(7).GetChild(0).GetChild(5).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;
        menu.transform.GetChild(0).GetChild(7).GetChild(0).GetChild(5).GetComponent<Image>().color += new Color32(0, 0, 0, 255);

        //right menu
        menu.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;
        menu.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Image>().color += new Color32(0, 0, 0, 255);

        //shop menu
        menu.transform.GetChild(0).GetChild(8).GetChild(0).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;
        menu.transform.GetChild(0).GetChild(8).GetChild(0).GetComponent<Image>().color += new Color32(0, 0, 0, 255);
        
        //board menu
        menu.transform.GetChild(0).GetChild(6).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;
        menu.transform.GetChild(0).GetChild(6).GetComponent<Image>().color += new Color32(0, 0, 0, 255);

        // Set a ui
        // uiPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].uiPref;

    }

    //Gets Values from style script for each square
    public void ApplyTheme(int num)
    {
        switch (num)
        {
            case 0:
                ApplyThemeFromHolder(0);
                break;
            case 1:
                ApplyThemeFromHolder(1);
                break;
            case 2:
                ApplyThemeFromHolder(2);
                break;
            case 3:
                ApplyThemeFromHolder(3);
                break;
            case 4:
                ApplyThemeFromHolder(4);
                break;
            case 5:
                ApplyThemeFromHolder(5);
                break;
            case 6:
                ApplyThemeFromHolder(6);
                break;
            case 7:
                ApplyThemeFromHolder(7);
                break;
            case 8:
                ApplyThemeFromHolder(8);
                break;
            case 9:
                ApplyThemeFromHolder(9);
                break;
            case 10:
                ApplyThemeFromHolder(10);
                break;
            case 11:
                ApplyThemeFromHolder(11);
                break;
            case 12:
                ApplyThemeFromHolder(12);
                break;
            case 13:
                ApplyThemeFromHolder(13);
                break;
            case 14:
                ApplyThemeFromHolder(14);
                break;
            default:
                Debug.LogError("Check the number that u pass to ApplyStyle");
                break;
        }
    }


    //For Leaderboard buttons sort 
    int CompareObNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }

    private void Awake()
    {
        CoinManager.Instance.shopCoinText = shopCurrencyText;
        shopCurrencyText.text = PlayerPrefs.GetInt("Coin", 20).ToString();
        themeIndex = PlayerPrefs.GetInt("Theme", 0);

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1);

        //Set Leaderboard buttons references
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("LeaderButtons");
        //Sort leaderboard buttons
        Array.Sort(tmp, CompareObNames);
      

        for (int i = 0; i < tmp.Length; i++)
        {
            Highscores.Instance.leaderButtons[i] = tmp[i].GetComponent<Image>();
        }
       
       
    }

    

    public void InitializeTheme()
    {
        ApplyTheme(themeIndex);


        //GameObject wheel = Instantiate(wheelPrefab);

        //wheel.transform.SetParent(RotatorPref);


       
        Instantiate(backPrefab);

      
       
      
        //highScoreText = menu.transform.GetChild(0).GetChild(4).gameObject.GetComponent<Text>();

        
        highScoreTimedText.text = PlayerPrefs.GetInt("HighscoreTimed", 0).ToString();

        highScoreRelaxText.text = PlayerPrefs.GetInt("HighscoreRelax", 0).ToString();

        highScoreDzenText.text = PlayerPrefs.GetInt("HighscoreDzen", 0).ToString();

    }

    public void Start()
    {
       
        InitializeTheme();

       

    }

    public void TitleNewGame()
    {
        serializer.CreateNewGame();
    }


}
