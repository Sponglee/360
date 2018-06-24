using UnityEngine.UI;
using UnityEngine;

public class TitleManager : Singleton<TitleManager> {


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
        wheelPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].wheelPref;
      
        backPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].backPref;

        //fontPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].fontPref;


        menu.transform.GetChild(0).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;


        //Options menu
        menu.transform.GetChild(0).GetChild(6).GetChild(0).GetChild(5).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;
        menu.transform.GetChild(0).GetChild(6).GetChild(0).GetChild(5).GetComponent<Image>().color += new Color32(0, 0, 0, 255);

        //right menu
        menu.transform.GetChild(0).GetChild(6).GetChild(0).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;
        menu.transform.GetChild(0).GetChild(6).GetChild(0).GetComponent<Image>().color += new Color32(0, 0, 0, 255);

        //shop menu
        menu.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;
        menu.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Image>().color += new Color32(0, 0, 0, 255);

   
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
            default:
                Debug.LogError("Check the number that u pass to ApplyStyle");
                break;
        }
    }





    private void Awake()
    {
        CoinManager.Instance.shopCoinText = shopCurrencyText;
        shopCurrencyText.text = PlayerPrefs.GetInt("Coin", 60).ToString();
        themeIndex = PlayerPrefs.GetInt("Theme", 0);

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1);
        
    }


    public void InitializeTheme()
    {
        ApplyTheme(themeIndex);


        GameObject wheel = Instantiate(wheelPrefab);

        wheel.transform.SetParent(RotatorPref);


       
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
