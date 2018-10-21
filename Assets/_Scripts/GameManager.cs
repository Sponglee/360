using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>

{
    public TutorialManager tutorialManager;

    //Progress variables
    public Slider progressSlider;
    public float currentLevel;
    public float levelUp;
    public float experience;
    public bool LevelIsUp = false;




    public bool AdInProgress = false;

    public GameObject shareButton;
    public GameObject pause;
    public GameObject powerUpPanel;


    public Sprite timeSprite;
    public Sprite relaxSprite;
    public Sprite dzenSprite;


    public GameObject drillPref;
    public GameObject bombPref;
    public GameObject hammerPref;
    public GameObject swoopPref;
    public GameObject pizzazPref;

    public Color32 leRed;
    public Color32 leGreen;
    public Color32 leYellow;


    [SerializeField]
    public GameObject ui;
    public GameObject uiPrefab;
    [SerializeField]
    public GameObject menu;

    //Keep track of spawns for save
    Board currentBoard;
    public GameSerializer serializer;


    [SerializeField]
    public GameObject wheelPrefab;
    public GameObject centerPref;
    public GameObject squarePrefab;
    public GameObject spotPrefab;
    public GameObject spawnPrefab;
    public GameObject gridPrefab;
    public GameObject backPrefab;
    public GameObject styleHolderPrefab;
    public Font fontPrefab;

    //Clickprefab 256
    public GameObject coinPrefab;

    public Transform line;

    [SerializeField]
    private Slider volumeSlider;


    [SerializeField]
    public GameObject uiSquarePrefab;

    //Tutorial NextScore and lock spawn
    int tutNum = 0;



    //prefab for controlling movement while falling
    GameObject squareSpawn = null;
    //for random expand spawns
    GameObject randSpawn = null;

    //check if it's game over
    public bool GameOverBool = false;


    // PowerUP cs
    public bool SelectHammer = false;
    public bool SelectBomb = false;
    public bool SelectDrill = false;
    //Keep track of select powerup usage
    public bool SquareDestroyed = false;

    //index of checkRow for multiple turn pops
    public int checkRowIndex = 0;

    //after gameover
    [SerializeField]
    private bool endGameCheck;

    public int randSpawnCount;
    //for 256 counts
    private int tops;
    public int Tops
    {
        get
        {
            return tops;
        }

        set
        {
            if (value != 0)
            {
                //topCount.gameObject.SetActive(true);
                if (value != tops)
                {
                    //topCount.text = string.Format(" x{0}", value);
                    tops = value;
                }

            }


        }
    }

    public int maxScore;
    [SerializeField]
    public int scoreUpper;
    [SerializeField]
    public int expandMoves;
    [SerializeField]
    private float stopDelay;

    //[SerializeField]
    //private bool IsRunning = false;

    [SerializeField]
    private int moves = 0;
    public int Moves
    {
        get
        {
            return moves;
        }

        set
        {
            moves = value;
        }
    }

    // for TIME EXPAND
    public float fMoves = 0f;




    //Vertical transform of top spot
    public GameObject currentSpot;
    // spawn point
    public GameObject currentSpawn;

    //Changable objects (for ApplyTheme)
    public int themeIndex;
    public GameObject wheel;
    public GameObject backGround;

    public bool TurnInProgress = false;

    public Stack<GameObject> pewObjs;

    //All the spots around the wheel
    public List<GameObject> spots;
    public List<GameObject> spawns;
    public GameObject[,] grids;

    //spawn cooldown
    private float coolDown;

    //for turn
    public float turnDelay = 0.5f;
    public float turnCoolDown;


    // number of objects
    public int nBottom;

    //Next square's score
    public Text nextScore;
    public Text upper;
    public Text topCount;
    public static int next_score;
    public Image slider;
    public float sliderFill;

    public Image nextRound;

    //scrolling text
    public GameObject FltText;
    public GameObject coinFltText;
    public GameObject pr_coinFltText;

    //scores
    public int dbSceneIndex;
    public int scores;
    public bool highscoreSoundBool = false;
    public int highscores;
    public Text scoreText;
    public Text highScoreText;

    //// Obj list for pop checkrow
    //List<GameObject> rowObjs;
    // list of rowObjs to execute(resets each click)
    List<List<GameObject>> popObjs;

    List<RandValues> rands;
    //list of randSpawns
    List<GameObject> randSpawns;
    List<GameObject> tmpSquares;

    //Checkrow Stack
    public Stack<GameObject> turnCheckObjs;
    public Stack<GameObject> checkObjs;
    //Toggle while rand are dropping
    private bool randSpawning = false;
    int tmpRands;

    // struct to hold randomSpawn values
    public struct RandValues
    {
        public int Rng { get; set; }
        public int RandScore { get; set; }
    }

    //For getspots
    Vector3 center;
    float rad;

    public bool SomethingIsMoving = false;
    public bool CheckInProgress = false;

    //For checkrow cornercases (simultaneous pops of same score)
    public bool FurtherProgress = false;



    public bool MergeInProgress = false;
    public bool RotationProgress = false;
    public float rotationDuration = 0.1f;
    private bool noMoves = false;

    //for ui check
    private bool mouseDown = false;
    public bool MenuUp = false;

    //spot rotation positions
    private Vector3 clickDirection;
    private float clickAngle;
    private float dirAngle;
    int checkClickSpot;

    int rotSpot;


    bool firstClick = true;

    //For clickspawn
    bool cantSpawn = true;
    public bool tutorialSpawnLock;
    bool NoClickSpawn = false;
    public bool NoRotation = false;
    public int tutCurrentSpot = -1;

    //FollowMouse resistance
    public float follow__Delay;
    public float follow__Angle;
    //Touch Resistance
    public float spawn__Angle;
    //Finish followup rotation
    public float differ__Angle;
    //for finish followup
    float differ = 0;
    public bool gameOverInProgress = false;

    Vector3 initClick;


    //TIME EXPAND
    float timeOfTravel = 1; //time after object reach a target place 
    float currentTime = 0; // actual floting time 
    float normalizedValue;

    #region Styles
    // Helps ApplyStyle to grab numbers/color
    void ApplyThemeFromHolder(int index)
    {
        wheelPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].wheelPref;
        centerPref = ThemeStyleHolder.Instance.ThemeStyles[index].wheelPref.transform.GetChild(5).gameObject;
        squarePrefab = ThemeStyleHolder.Instance.ThemeStyles[index].squarePref;
        spotPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].spotPref;
        backPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].backPref;

        gridPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].gridPref;
        spawnPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].spawnPref;

        FltText = ThemeStyleHolder.Instance.ThemeStyles[index].fltTextPref;

        styleHolderPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].styleHolderPref;

        leYellow = ThemeStyleHolder.Instance.ThemeStyles[index].yellowPref;
        leRed = ThemeStyleHolder.Instance.ThemeStyles[index].redPref;
        fontPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].fontPref;

        shareButton.GetComponent<Button>().colors = ThemeStyleHolder.Instance.ThemeStyles[index].shareButton;
        shareButton.transform.GetChild(0).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].shareButtonText;
        //Slider theme
        progressSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].shareButtonText;
        progressSlider.transform.GetChild(2).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].shareButtonText;
        progressSlider.transform.GetChild(3).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].shareButtonText;

        menu.transform.GetChild(0).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;
        //right menu
        menu.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;
        menu.transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Image>().color += new Color32(0, 0, 0, 255);

        //shop menu
        menu.transform.GetChild(0).GetChild(8).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;
        menu.transform.GetChild(0).GetChild(8).GetComponent<Image>().color += new Color32(0, 0, 0, 255);

        //Options menu
        menu.transform.GetChild(0).GetChild(7).GetChild(0).GetChild(0).GetComponent<Image>().color = ThemeStyleHolder.Instance.ThemeStyles[index].menuPref;
        menu.transform.GetChild(0).GetChild(7).GetChild(0).GetChild(0).GetComponent<Image>().color += new Color32(0, 0, 0, 255);

        // Set a ui
        uiPrefab = ThemeStyleHolder.Instance.ThemeStyles[index].uiPref;

        line.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ThemeStyleHolder.Instance.ThemeStyles[index].linePref;
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





    private void Awake()
    {
        themeIndex = PlayerPrefs.GetInt("Theme", 0);
        if (PlayerPrefs.GetInt("TutorialStep", 0) == 0)
        {
            NewGame();
        }

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1);
    }


    public void InitializeTheme()
    {
        ApplyTheme(themeIndex);


        //Progress bar initialization
        currentLevel = PlayerPrefs.GetFloat("CurrentLevel", 1);
        experience = PlayerPrefs.GetFloat("Experience", 0);

        levelUp = Mathf.Round(Mathf.Exp(currentLevel) * 100f) + 1000f;

        progressSlider.value = experience / levelUp;
        progressSlider.transform.GetChild(2).GetComponentInChildren<Text>().text = currentLevel.ToString();
        progressSlider.transform.GetChild(3).GetComponentInChildren<Text>().text = (currentLevel+1).ToString();

        //**************************

        ui = Instantiate(uiPrefab);




        wheel = Instantiate(wheelPrefab, new Vector3(0, -7f, 11f), Quaternion.identity);
        backGround = Instantiate(backPrefab);

        slider = wheel.transform.GetChild(6).GetChild(0).GetComponent<Image>();

        nextScore = ui.transform.GetChild(1).gameObject.GetComponent<Text>();
        scoreText = ui.transform.GetChild(2).gameObject.GetComponent<Text>();
        highScoreText = ui.transform.GetChild(4).gameObject.GetComponent<Text>();

        nextRound = ui.transform.GetChild(3).gameObject.GetComponent<Image>();




        Instantiate(styleHolderPrefab);



    }

    // Helps ApplyStyle to grab numbers/color
    private void ApplyStyleFromHolder(int index)
    {
        nextScore.color = SquareStyleHolder.Instance.SquareStyles[index].SquareColor;
        //nextRound.color = SquareStyleHolder.Instance.SquareStyles[index].SquareColor; 


        //nextScore.GetComponent<Outline>().effectColor = SquareStyleHolder.Instance.SquareStyles[index].SquareColor;
    }
    //Gets Values from style script for each square
    private void ApplyStyle(int num)
    {
        switch (num)
        {
            case 2:
                ApplyStyleFromHolder(0);
                break;
            case 4:
                ApplyStyleFromHolder(1);
                break;
            case 8:
                ApplyStyleFromHolder(2);
                break;
            case 16:
                ApplyStyleFromHolder(3);
                break;
            case 32:
                ApplyStyleFromHolder(4);
                break;
            case 64:
                ApplyStyleFromHolder(5);
                break;
            case 128:
                ApplyStyleFromHolder(6);
                break;
            case 256:
                ApplyStyleFromHolder(7);
                break;
            //case 512:
            //    ApplyStyleFromHolder(8);
            //    break;
            default:
                Debug.LogError("Check the number that u pass to ApplyStyle");
                break;
        }
    }


    #endregion /Styles


    void Start()
    {


        //===========================================Initialize theme==============================================================
        InitializeTheme();
        //==========================================================================================================================


        //for saving board of scores



        //currentBoard = new Board();
        //currentBoard.pieces = new List<Piece>();

        serializer = new GameSerializer();


        turnCheckObjs = new Stack<GameObject>();
        checkObjs = new Stack<GameObject>();
        //objects that were stopped
        pewObjs = new Stack<GameObject>();
        //Apply all the numbers 
        maxScore = 3;

        //different expandMoves for each mode

        if (SceneManager.GetActiveScene().name == "Relax")
            expandMoves = 3;
        else
            expandMoves = 10;



        Tops = 0;
        // count of randomSpawns 
        randSpawnCount = 3;
        scoreUpper = (int)Mathf.Pow(2, maxScore);
        nBottom = 10;
        spots = new List<GameObject>();
        spawns = new List<GameObject>();
        grids = new GameObject[nBottom, 5];

        scores = 0;


        //Set a gameMode and highscore
        if (SceneManager.GetActiveScene().name == "Relax")
        {
            //for leaderboard
            dbSceneIndex = 2;
            PlayerPrefs.SetInt("GameMode", 0);
            highscores = PlayerPrefs.GetInt("HighscoreRelax", 0);
        }
        else if (SceneManager.GetActiveScene().name == "Main")
        {
            //for leaderboard
            dbSceneIndex = 1;
            PlayerPrefs.SetInt("GameMode", 1);
            highscores = PlayerPrefs.GetInt("HighscoreTimed", 1);
        }
        else if (SceneManager.GetActiveScene().name == "Dzen")
        {
            //for leaderboard
            dbSceneIndex = 0;
            PlayerPrefs.SetInt("GameMode", 2);
            highscores = PlayerPrefs.GetInt("HighscoreDzen", 2);
        }


        highScoreText.gameObject.SetActive(true);
        highScoreText.text = highscores.ToString();
        scoreText.text = scores.ToString();



        //Enable GAME MODE ICON 
        if (SceneManager.GetActiveScene().name == "Main")
        {
            ui.transform.GetChild(6).gameObject.SetActive(true);
            Image tmpImg = ui.transform.GetChild(6).gameObject.GetComponent<Image>();
            if (tmpImg != null)
                tmpImg.sprite = timeSprite;
        }
        else if (SceneManager.GetActiveScene().name == "Relax")
        {
            ui.transform.GetChild(6).gameObject.SetActive(true);
            Image tmpImg = ui.transform.GetChild(6).gameObject.GetComponent<Image>();
            if (tmpImg != null)
                tmpImg.sprite = relaxSprite;
        }
        else if (SceneManager.GetActiveScene().name == "Dzen")
        {
            ui.transform.GetChild(6).gameObject.SetActive(true);
            Image tmpImg = ui.transform.GetChild(6).gameObject.GetComponent<Image>();
            if (tmpImg != null)
                tmpImg.sprite = dzenSprite;
        }




        //upper.text = string.Format("{0}", scoreUpper);
        //NextShrink.text = string.Format("{0}", expandMoves - Moves);
        sliderFill = (expandMoves - Moves) / expandMoves;

        ////Gradually change slider fillAmount
        //StartCoroutine(SliderStop());




        //for final gameover
        endGameCheck = false;

        //Random next score to appear (2^3 max <-----)
        next_score = 2;
        nextScore.text = next_score.ToString();
        ApplyStyle(next_score);
        //Initialize level (spots)
        GetSpots();

        //rowObjs = new List<GameObject>();
        popObjs = new List<List<GameObject>>();
        tmpSquares = new List<GameObject>();

        //for first spwan of 2
        tmpRands = randSpawnCount;
        menu.SetActive(false);



        LoadGame("threesixty.dat");



        //********************TUTORIAL RESTRICTIONS*****************************If tutorial for POwer ups - no clicking


        //if (PlayerPrefs.GetInt("TutorialStep", 0) == 0 /*|| PlayerPrefs.GetInt("TutorialStep", 0) > 5*/)
        //{
        //    NoRotation = true;
        //}
        //else if (PlayerPrefs.GetInt("TutorialStep", 0) >= 1 /*&& PlayerPrefs.GetInt("TutorialStep", 0) < 6*/)
        //    NoRotation = false;



        //if (PlayerPrefs.GetInt("TutorialStep", 0) > 2 && PlayerPrefs.GetInt("TutorialStep", 0) < 6)
        //{
        //    //Hammer
        //    if (GameManager.Instance.tutorialManager.tutorialStep == 3)
        //    {

        //        GameManager.Instance.tutorialManager.powerUpAnim[0].SetBool("Highlight", true);



        //        //GameManager.Instance.tutorialManager.tutorialTrigger.Invoke();
        //        CoinManager.Instance.Coins += 2;

        //        //PREPARE FOR HAMMER TUTORIAL
        //        #region passing0

        //        //if there's no start yet
        //        List<int> tutSpawns = new List<int>();
        //        List<int> tutScores = new List<int>(new int[] { 2 });
        //        int dropIndex = int.Parse(GameManager.Instance.currentSpot.name);




        //        tutSpawns.Add(dropIndex);



        //        #endregion
        //        bool countBool = false;

        //        for (int i = 0; i < nBottom; i++)
        //        {
        //            if (spots[i].transform.childCount != 0)
        //                countBool = true;
        //            Debug.Log(spots[i].transform.childCount);
        //        }

        //        if (!countBool)
        //            StartCoroutine(StopTutBomb(tutSpawns, tutScores));

        //    }
        //    else if (GameManager.Instance.tutorialManager.tutorialStep == 4)
        //    {

        //        GameManager.Instance.tutorialManager.powerUpAnim[1].SetBool("Highlight", true);



        //        //GameManager.Instance.tutorialManager.tutorialTrigger.Invoke();
        //        CoinManager.Instance.Coins += 2;

        //        //PREPARE FOR BOMB TUTORIAL
        //        #region passing0

        //        //if there's no start yet
        //        List<int> tutSpawns = new List<int>();
        //        List<int> tutScores = new List<int>(new int[] { 8, 4, 2, 4, 8, 2, 8, 4, 8 });
        //        int dropIndex = int.Parse(GameManager.Instance.currentSpot.name);
        //        int firstIndex;
        //        int nextIndex;

        //        //check next left one after getting index-1
        //        if (dropIndex - 1 < 0)
        //        {
        //            firstIndex = GameManager.Instance.nBottom - 1;
        //        }
        //        else
        //            firstIndex = dropIndex - 1;

        //        //check next one after setting index+1
        //        if (dropIndex + 1 > GameManager.Instance.nBottom - 1)
        //        {
        //            nextIndex = 0;
        //        }
        //        else
        //            nextIndex = dropIndex + 1;

        //        tutSpawns.Add(firstIndex);
        //        tutSpawns.Add(dropIndex);
        //        tutSpawns.Add(nextIndex);

        //        #endregion
        //        int countInt = 0;

        //        for (int i = 0; i < nBottom; i++)
        //        {
        //            if (spots[i].transform.childCount != 0)
        //                countInt++;
        //            Debug.Log(spots[i].transform.childCount);
        //        }

        //        if (countInt < 3)
        //            StartCoroutine(StopTutBomb(tutSpawns, tutScores));


        //    }
        //    else if (GameManager.Instance.tutorialManager.tutorialStep == 5)
        //    {

        //        GameManager.Instance.tutorialManager.powerUpAnim[2].SetBool("Highlight", true);



        //        //GameManager.Instance.tutorialManager.tutorialTrigger.Invoke();
        //        CoinManager.Instance.Coins += 2;

        //        //PREPARE FOR DRILL TUTORIAL
        //        int countInt = 0;
        //        for (int i = 0; i < nBottom; i++)
        //        {
        //            if (spots[i].transform.childCount >= 3)
        //                countInt++;
        //            Debug.Log(spots[i].transform.childCount);
        //        }

        //        if (countInt < 3)
        //            StartCoroutine(TutorialManager.Instance.StopTutDrill());




        //    }
        //    NoClickSpawn = true;
        //}
        //else
        //{
        //    NoClickSpawn = false;

        //}


        //*************************END OF TUTORIAL RESTICTIONS******************************










    }


    private IEnumerator SliderStop()
    {
        float timeOfTravel = 1; //time after object reach a target place 
        float currentTime = 0; // actual floting time 
        float normalizedValue;

        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 

            slider.fillAmount = Mathf.Lerp(slider.fillAmount, sliderFill, normalizedValue);
            yield return null;
        }
    }




    //Time EXPAND
    private void FixedUpdate()
    {
        //Turn timer
        turnCoolDown -= Time.deltaTime;

        //Expand Moves for Timed
        if (SceneManager.GetActiveScene().name == "Main")
        {
            //Debug.Log(slider.fillAmount);
            //while (currentTime <= timeOfTravel)
            //{
            if (!MenuUp && PlayerPrefs.GetInt("TutorialStep", 0) > 1 && !GameOverBool && SceneManager.GetActiveScene().name == "Main")
            {
                fMoves += 0.01f;
                sliderFill = (float)(expandMoves - fMoves) / expandMoves;
                slider.fillAmount = sliderFill;
            }
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 
            slider.fillAmount = Mathf.Lerp(slider.fillAmount, sliderFill, normalizedValue);

            //}
            if (fMoves > expandMoves)
            {

                fMoves = 0;
                ResetExpandTime();
            }

        }





    }

    public IEnumerator StopTutBomb(List<int> tutSpawns, List<int> tutScores)
    {

        int scoreCount = 0;

        foreach (int tmp in tutSpawns)
        {

            if (spots[tmp].transform.childCount != 0)
            {
                //clear the field
                for (int i = 0; i < spots[tmp].transform.childCount; i++)
                {
                    spots[tmp].transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                    spots[tmp].transform.GetChild(i).SetParent(null);
                }


            }

            while (spots[tmp].transform.childCount < 1)
            {
                ////center lower than sides
                //if (spots[tmp] == currentSpot && spots[tmp].transform.childCount == 2)
                //{
                //    scoreCount++;
                //    break;
                //}

                randSpawn = Instantiate(squarePrefab, spawns[tmp].transform.position, Quaternion.identity);

                randSpawn.GetComponent<Square>().ExpandSpawn = true;
                randSpawn.GetComponent<Square>().Score = tutScores[scoreCount];
                scoreCount++;

                if (scoreCount == tutScores.Count)
                    scoreCount = 0;



                randSpawn.transform.SetParent(spots[tmp].transform);
                randSpawn.name = randSpawn.transform.GetSiblingIndex().ToString();

                //Rotate spawns towards center
                Vector3 diff = randSpawn.transform.parent.position - randSpawn.transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                randSpawn.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
                yield return new WaitForSeconds(0.2f);
                tutScores.Reverse();
            }

        }

    }

    //Delete pressed powerup object and add score
    public void DestroyPowerUp(GameObject tmp)
    {
        Vector3 fltOffset = new Vector3(0f, 0.1f, 5f);

        Destroy(tmp);

        //add score for explosion
        scores += tmp.GetComponent<Square>().Score;

        ProgressUpdate(tmp.GetComponent<Square>().Score);
        ////Progress update
        //experience += tmp.GetComponent<Square>().Score;
        //if(experience>=levelUp)
        //{
        //    currentLevel++;
        //    PlayerPrefs.SetFloat("CurrentLevel", currentLevel);
        //    progressSlider.transform.GetChild(2).GetComponentInChildren<Text>().text = currentLevel.ToString();
        //    progressSlider.transform.GetChild(3).GetComponentInChildren<Text>().text = (currentLevel + 1).ToString();
        //}
        //progressSlider.value = experience / levelUp;
        ////************progress****************


        scoreText.text = scores.ToString();
        //Spawn float text
        GameObject textObj = Instantiate(FltText, tmp.transform.position, tmp.transform.rotation);
        textObj.transform.position = tmp.transform.TransformPoint(tmp.transform.localPosition + fltOffset);
        textObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + tmp.GetComponent<Square>().Score.ToString();

    }

    //Progress update
    public void ProgressUpdate(int scoreUpdt)
    {
       
        experience += scoreUpdt;
        PlayerPrefs.SetFloat("Experience", experience);
        if (experience > levelUp && !LevelIsUp)
        {
            LevelIsUp = true;
            currentLevel++;
            PlayerPrefs.SetFloat("CurrentLevel", currentLevel);
            experience = experience - levelUp;
            levelUp = Mathf.Round(Mathf.Exp(currentLevel) * 100f) + 1000f;
            progressSlider.transform.GetChild(2).GetComponentInChildren<Text>().text = currentLevel.ToString();
            progressSlider.transform.GetChild(3).GetComponentInChildren<Text>().text = (currentLevel + 1).ToString();


            //GIFT SITUATION
            AudioManager.Instance.PlaySound("256");
            GameObject txtObj = Instantiate(GameManager.Instance.pr_coinFltText, progressSlider.transform.GetChild(3));
            txtObj.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "2";
            CoinManager.Instance.Coins += 2;
        }
        progressSlider.value = experience / levelUp;
        LevelIsUp = false;
    }
    //************progress****************



    void Update()
    {
        //=============================================================================================================
        //GLDebug.DrawLine(wheel.transform.up - new Vector3(0, 7f), wheel.transform.position, Color.red, 0, true);
        //if (rotSpot != -1)
        //{
        //    //spot sticky line
        //    GLDebug.DrawLine(spots[rotSpot].transform.position, wheel.transform.position, Color.cyan, 0, true);

        //}
        ////Debuging lines above
        ////===============================================================================================================
        //if (PlayerPrefs.GetInt("TutorialStep", 0) == 2)
        //{

        //    if (tutCurrentSpot != int.Parse(currentSpot.name))
        //    {
        //        NoRotation = true;
        //    }
        //}
        //if (PlayerPrefs.GetInt("TutorialStep", 0) == 1 || PlayerPrefs.GetInt("TutorialStep", 0) > 5)
        //{
        //    NoRotation = false;
        //    tutCurrentSpot = int.Parse(currentSpot.name);
        //}
        //else if (PlayerPrefs.GetInt("TutorialStep", 0) > 2 && PlayerPrefs.GetInt("TutorialStep", 0) < 6)
        //    NoRotation = true;
        //else
        //{

        //    NoRotation = false;
        //    NoClickSpawn = false;
        //}

        #region Input

        // USE **HAMMER
        if (IsPointerOverUIObject("square") && SelectHammer && Input.GetMouseButtonUp(0))
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count > 0)
            {
                //Get rid of selected square
                SelectHammer = false;
                Instantiate(hammerPref, results[0].gameObject.transform.position, Quaternion.identity);


                ////********************TUTORIAL*********HAMMER(SELECT)
                //if (GameManager.Instance.tutorialManager.tutorialStep == 3)
                //{



                //    GameManager.Instance.tutorialManager.tutorialTrigger.Invoke();
                //    GameManager.Instance.tutorialManager.powerUpAnim[1].SetBool("Highlight", true);



                //    CoinManager.Instance.Coins += 2;

                //    //PREPARE FOR BOMB TUTORIAL
                //    #region passing0

                //    //if there's no start yet
                //    List<int> tutSpawns = new List<int>();
                //    List<int> tutScores = new List<int>(new int[] { 8, 4, 2, 4, 8, 2, 8, 4, 8 });
                //    int dropIndex = int.Parse(GameManager.Instance.currentSpot.name);
                //    int firstIndex;
                //    int nextIndex;

                //    //check next left one after getting index-1
                //    if (dropIndex - 1 < 0)
                //    {
                //        firstIndex = GameManager.Instance.nBottom - 1;
                //    }
                //    else
                //        firstIndex = dropIndex - 1;

                //    //check next one after setting index+1
                //    if (dropIndex + 1 > GameManager.Instance.nBottom - 1)
                //    {
                //        nextIndex = 0;
                //    }
                //    else
                //        nextIndex = dropIndex + 1;

                //    tutSpawns.Add(firstIndex);
                //    tutSpawns.Add(dropIndex);
                //    tutSpawns.Add(nextIndex);


                //    #endregion
                //    StartCoroutine(StopTutBomb(tutSpawns, tutScores));

                //}
                ////*****************************


                //foreach (RaycastResult result in results)
                //{

              
                //Check if parent is square too (depending on what object first click was commited)
                if (results[0].gameObject.transform.parent.CompareTag("square") && results[0].gameObject.transform.parent.parent.CompareTag("spot"))
                {
                    DestroyPowerUp(results[0].gameObject.transform.parent.gameObject);
                    SquareDestroyed = true;
                }
                else if (results[0].gameObject.transform.parent.parent.CompareTag("square"))
                {
                    DestroyPowerUp(results[0].gameObject.transform.parent.parent.gameObject);
                    SquareDestroyed = true;
                }
                else if (results[0].gameObject.CompareTag("square"))
                {
                    DestroyPowerUp(results[0].gameObject);
                    SquareDestroyed = true;
                }
                else
                {
                    AudioManager.Instance.PlaySound("stop");
                }

            }
            else
            {
                AudioManager.Instance.PlaySound("stop");
            }

        }
        //Use **BOMB
        else if (IsPointerOverUIObject("square") && SelectBomb && Input.GetMouseButtonUp(0))
        {

            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count > 0)
            {
                //Get rid of selected square
                SelectBomb = false;
                //Instantiate(bombPref, results[0].gameObject.transform.position, Quaternion.identity);


                ////********************TUTORIAL*********BOMB
                //if (GameManager.Instance.tutorialManager.tutorialStep == 4)
                //{

                //    GameManager.Instance.tutorialManager.tutorialTrigger.Invoke();


                //    GameManager.Instance.tutorialManager.powerUpAnim[2].SetBool("Highlight", true);

                //    CoinManager.Instance.Coins += 3;
                //    StartCoroutine(TutorialManager.Instance.StopTutDrill());
                //}
                ////*****************************

                //Debug.Log("BOMB SELECTED  " + results[0].gameObject.name);

                int dropIndex = -1;
                int dropIndexSquare = -1;

                if (results[0].gameObject.transform.parent.CompareTag("square") && results[0].gameObject.transform.parent.parent.CompareTag("spot"))
                {
                    dropIndex = int.Parse(results[0].gameObject.transform.parent.parent.name);

                    //Debug.Log("1 " + results[0].gameObject.transform.parent.parent.name);

                    dropIndexSquare = results[0].gameObject.transform.parent.GetSiblingIndex();
                    SquareDestroyed = true;

                }
                else if (results[0].gameObject.transform.parent.parent.CompareTag("square"))
                {
                    dropIndex = int.Parse(results[0].gameObject.transform.parent.parent.parent.name);

                    //Debug.Log("2 " + results[0].gameObject.transform.parent.parent.parent.name);

                    dropIndexSquare = results[0].gameObject.transform.parent.parent.GetSiblingIndex();
                    SquareDestroyed = true;

                }
                else if (results[0].gameObject.CompareTag("square"))
                {
                    dropIndex = int.Parse(results[0].gameObject.transform.parent.name);

                    //Debug.Log("3 " + results[0].gameObject.transform.parent.name);

                    dropIndexSquare = results[0].gameObject.transform.GetSiblingIndex();
                    SquareDestroyed = true;
                }
                else
                {
                    AudioManager.Instance.PlaySound("stop");
                }



                int firstIndex;
                int nextIndex;

                //check next left one after getting index-1
                if (dropIndex - 1 < 0)
                {
                    firstIndex = GameManager.Instance.nBottom - 1;
                }
                else
                    firstIndex = dropIndex - 1;

                //check next one after setting index+1
                if (dropIndex + 1 > GameManager.Instance.nBottom - 1)
                {
                    nextIndex = 0;
                }
                else
                    nextIndex = dropIndex + 1;

                //Check if there's square to destroy
                if (spots[firstIndex].transform.childCount > dropIndexSquare)
                {
                    Debug.Log(spots[firstIndex].transform.parent.childCount + "   " + dropIndexSquare);
                    Instantiate(bombPref, spots[firstIndex].transform.GetChild(dropIndexSquare).position, Quaternion.identity);
                    DestroyPowerUp(spots[firstIndex].transform.GetChild(dropIndexSquare).gameObject);

                }


                if (spots[dropIndex].transform.childCount > dropIndexSquare)
                {
                    Debug.Log(spots[dropIndex].transform.parent.childCount + "   " + dropIndexSquare);
                    Instantiate(bombPref, spots[dropIndex].transform.GetChild(dropIndexSquare).position, Quaternion.identity);
                    DestroyPowerUp(spots[dropIndex].transform.GetChild(dropIndexSquare).gameObject);
                }

                if (spots[nextIndex].transform.childCount > dropIndexSquare)
                {
                    Debug.Log(spots[nextIndex].transform.parent.childCount + "   " + dropIndexSquare);
                    Instantiate(bombPref, spots[nextIndex].transform.GetChild(dropIndexSquare).position, Quaternion.identity);
                    DestroyPowerUp(spots[nextIndex].transform.GetChild(dropIndexSquare).gameObject);
                }
                SquareDestroyed = true;
            }
        }
        //USE **DRILL
        else if (IsPointerOverUIObject("square") && SelectDrill && Input.GetMouseButtonUp(0))
        {

            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count > 0)
            {
                //Get rid of selected square
                SelectDrill = false;
                //Instantiate(bombPref, results[0].gameObject.transform.position, Quaternion.identity);



                ////********************TUTORIAL*********DRILL
                //if (GameManager.Instance.tutorialManager.tutorialStep == 5)
                //{





                //    GameManager.Instance.tutorialManager.tutorialTrigger.Invoke();
                //    CoinManager.Instance.Coins += 5;
                //    //GameManager.Instance.tutorialManager.button.SetActive(true);
                //    NoClickSpawn = false;

                //    StartCoroutine(TutorialManager.Instance.StopCloseTut());
                //}
                //=========================================================

                //Debug.Log("DRILL SELECTED  " + results[0].gameObject.name);

                int dropIndex = -1;
                int dropIndexSquare = -1;

                if (results[0].gameObject.transform.parent.CompareTag("square") && results[0].gameObject.transform.parent.parent.CompareTag("spot"))
                {
                    dropIndex = int.Parse(results[0].gameObject.transform.parent.parent.name);
                    dropIndexSquare = results[0].gameObject.transform.parent.GetSiblingIndex();
                    SquareDestroyed = true;

                }
                else if (results[0].gameObject.transform.parent.parent.CompareTag("square"))
                {
                    dropIndex = int.Parse(results[0].gameObject.transform.parent.parent.parent.name);
                    dropIndexSquare = results[0].gameObject.transform.parent.parent.GetSiblingIndex();
                    SquareDestroyed = true;

                }
                else if (results[0].gameObject.CompareTag("square"))
                {
                    dropIndex = int.Parse(results[0].gameObject.transform.parent.name);
                    dropIndexSquare = results[0].gameObject.transform.GetSiblingIndex();
                    SquareDestroyed = true;
                }
                else
                {
                    AudioManager.Instance.PlaySound("stop");
                }


                if (spots[dropIndex].transform.childCount > dropIndexSquare && dropIndexSquare > 0)
                {
                    Instantiate(drillPref, spots[dropIndex].transform.GetChild(dropIndexSquare - 1).position, Quaternion.identity);
                    DestroyPowerUp(spots[dropIndex].transform.GetChild(dropIndexSquare - 1).gameObject);
                }


                if (spots[dropIndex].transform.childCount > dropIndexSquare)
                {
                    Instantiate(drillPref, spots[dropIndex].transform.GetChild(dropIndexSquare).position, Quaternion.identity);
                    DestroyPowerUp(spots[dropIndex].transform.GetChild(dropIndexSquare).gameObject);
                }

                if (spots[dropIndex].transform.childCount > dropIndexSquare && dropIndexSquare < 3)
                {
                    Instantiate(drillPref, spots[dropIndex].transform.GetChild(dropIndexSquare + 1).position, Quaternion.identity);
                    DestroyPowerUp(spots[dropIndex].transform.GetChild(dropIndexSquare + 1).gameObject);
                }


                SquareDestroyed = true;


            }
        }

        //Track clickSpawn clicks
        if (!IsPointerOverUIObject("ui") && !SelectHammer && !SelectBomb && !SelectDrill && Input.GetMouseButtonDown(0) && !MenuUp)
        {
            mouseDown = true;
            cantSpawn = false;
            checkClickSpot = int.Parse(currentSpot.name);
            clickAngle = GetFirstClickAngle();
            clickDirection = wheel.transform.up / Mathf.Sin(clickAngle);
            //initClick = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10f);

            //Continue check
            if (endGameCheck)
            {
                foreach (GameObject spot in spots)
                {
                    if (spot.transform.childCount != 0)
                        return;
                }
                SomethingIsMoving = false;
            }





        }

        //FOR CLICKS
        if (!IsPointerOverUIObject("ui") && !IsPointerOverUIObject("256") && !SelectHammer && !SelectBomb && !SelectDrill && Input.GetMouseButton(0) && !RotationProgress && !noMoves && !MenuUp)
        {
            // //=================================================================================================================================
            //     //initialClick /clickAngle
            //     GLDebug.DrawLine(initClick, wheel.transform.position, Color.magenta, 2, true);

            // //dirangle
            // GLDebug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), wheel.transform.position, Color.white, 2, true);
            //// =================================================================================================================================

            dirAngle = GetFirstClickAngle();

            // touch resistance (firstClick for smooth rotation after first displacement
            //if mouse angle < follow_angle - do nothing
            if (Mathf.Abs(clickAngle - dirAngle) < follow__Angle && !RotationProgress && firstClick)
            {
                return;
            }
            else
            {
                //for spawn clicks
                cantSpawn = true;

                firstClick = false;
                if (!randSpawning)
                {
                    if (!NoRotation)
                        FollowMouse(clickAngle, clickDirection);
                }
            }
        }

        if (!IsPointerOverUIObject("ui") && !SelectHammer && !SelectBomb && !SelectDrill && Input.GetMouseButtonUp(0) && Time.time > coolDown && !RotationProgress && !noMoves && !MenuUp)
        {
            // if clicked 
            if (mouseDown)
            {



                if (int.Parse(currentSpot.name) == checkClickSpot && !NoRotation)
                {

                    //Turn left
                    if ((SwipeManager.Instance.IsSwiping(SwipeDirection.Left) && !RotationProgress) && !NoRotation)
                    {

                        int nextClickSpot;
                        if (checkClickSpot + 1 == nBottom)
                        {
                            nextClickSpot = 0;
                        }
                        else
                            nextClickSpot = checkClickSpot + 1;
                        //spot to rotate towards
                        rotSpot = nextClickSpot;


                        StartCoroutine(FollowRotate(rotationDuration));


                        //********************TUTORIAL*********ROTATE
                        if (tutorialManager.tutorialStep == 1)
                        {
                            //Debug.Log("NO ROTATION");
                            tutorialManager.tutorialTrigger.Invoke();
                        }
                        //*****************************

                    }
                    //Turn right
                    else if ((SwipeManager.Instance.IsSwiping(SwipeDirection.Right) && !RotationProgress) && !NoRotation)
                    {
                        int firstClickSpot;
                        if (checkClickSpot - 1 < 0)
                        {
                            firstClickSpot = nBottom - 1;
                        }
                        else
                            firstClickSpot = checkClickSpot - 1;
                        //spot to rotate towards
                        rotSpot = firstClickSpot;


                        StartCoroutine(FollowRotate(rotationDuration));



                        //********************TUTORIAL*********ROTATE
                        if (tutorialManager.tutorialStep == 1)
                        {
                            //Debug.Log("NO ROTATION");
                            tutorialManager.tutorialTrigger.Invoke();
                        }
                        //*****************************

                    }
                    else if ((SwipeManager.Instance.IsSwiping(SwipeDirection.None) || Input.GetKeyDown(KeyCode.RightArrow)) && !RotationProgress && !NoRotation)
                    {
                        rotSpot = checkClickSpot;

                        StartCoroutine(FollowRotate(rotationDuration));
                    }
                    //centerAnim.SetTrigger("tilt");







                }
                // if moved more than degrees between spots - follow up to closest one
                else
                {
                    if (cantSpawn)
                    {

                        StartCoroutine(FollowRotate(rotationDuration));
                    }


                }



                //disble angle buffer 
                firstClick = true;
                mouseDown = false;
            }

            if (Mathf.Abs(clickAngle - dirAngle) < spawn__Angle && SwipeManager.Instance.IsSwiping(SwipeDirection.None) && !randSpawning && !cantSpawn && currentSpot.transform.childCount <= 4 && currentSpot.GetComponent<SpriteRenderer>().color != leRed)
            {

                if (SomethingIsMoving || MergeInProgress || CheckInProgress || TurnInProgress || NoClickSpawn)
                {
                    //Debug.Log("NOPE");
                    //camera shake?
                    return;
                }
                else
                {
                    coolDown = Time.time + 0.5f;




                    ClickSpawn();
                    cantSpawn = true;



                }
            }




        }



        //TURN Launch checkrows
        if (checkObjs.Count > 0 && turnCoolDown <= 0 /*&& !SomethingIsMoving && !MergeInProgress && !CheckInProgress && !TurnInProgress && !FurtherProgress*/)
        {

            //Debug.Log("TURN");
            turnCoolDown = turnDelay;
            turnCheckObjs = checkObjs;

            checkObjs = new Stack<GameObject>();

            //To make it check once
            TurnInProgress = true;
            //turnDelay = Time.deltaTime + 3f;
            StartCoroutine(Turn());
        }
        //else if (checkObjs.Count > 0 && TurnInProgress)
        //{

        //    if (Time.deltaTime > turnTimer)
        //    {
        //        //Debug.Log("NYETNYETNYET");
        //        //checkObjs.Clear();
        //        TurnInProgress = false;
        //        FurtherProgress = false;
        //        MergeInProgress = false;
        //    }
        //}

    }





    // Get angle for mousePosition
    private float GetFirstClickAngle()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 direction = screenPos - wheel.transform.position;

        return Mathf.Atan2(Vector3.Dot(Vector3.back, Vector3.Cross(wheel.transform.up, direction)), Vector3.Dot(wheel.transform.up, direction)) * Mathf.Rad2Deg;
    }




    //Turn wheel to mouse position
    private void FollowMouse(float startAngle, Vector3 startDirection)
    {
        //********************TUTORIAL*********ROTATE
        if (tutorialManager.tutorialStep == 1)
        {
            //Debug.Log("NO ROTATION");
            tutorialManager.tutorialTrigger.Invoke();
        }
        //*****************************

        //Angle to rotate to
        float angle = GetFirstClickAngle();

        wheel.transform.Rotate(Vector3.forward, startAngle - angle);
        int firstSpot;
        int nextSpot;
        int spot = int.Parse(currentSpot.name);

        //passing through 0
        if (spot - 1 < 0)
        {
            firstSpot = nBottom - 1;
        }
        else
            firstSpot = spot - 1;

        //check next left one after getting index-1
        if (spot + 1 == nBottom)
        {
            nextSpot = 0;
        }
        else
            nextSpot = spot + 1;


        int[] spotDist = { spot, firstSpot, nextSpot };

        //check which spot is closer:
        rotSpot = ClosestSpot(spotDist);



    }


    ////Smooth rotation coroutine
    IEnumerator FollowRotate(float duration = 0.2f, float angle = 0)
    {

        //To avoid interruptions
        RotationProgress = true;

        //if there was followmouse
        if (rotSpot != -1)
        {
            Vector3 lineDir = line.position - wheel.transform.position;
            Vector3 spotDir = spots[rotSpot].transform.position - wheel.transform.position;
            //angle to next spot to rotate to
            angle = Mathf.Atan2(Vector3.Dot(Vector3.back, Vector3.Cross(lineDir, spotDir)), Vector3.Dot(lineDir, spotDir)) * Mathf.Rad2Deg;
        }

        Quaternion from = wheel.transform.rotation;
        Quaternion to = from * Quaternion.Euler(0, 0, angle);

        //smooth lerp rotation loop
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            wheel.transform.rotation = Quaternion.Lerp(from, to, elapsed / duration);
            elapsed += Time.fixedDeltaTime;

            yield return null;
        }

        //Finish ROtation differ angle setup
        Quaternion difRot = wheel.transform.rotation;
        if (int.Parse(currentSpot.name) != checkClickSpot)
        {
            // angle that should be the rotation
            differ = (int.Parse(currentSpot.name)) * (360 / nBottom) - difRot.eulerAngles.z;
        }

        //Finish move actual movement (differ in rotate) Get rid of difference flaw to the left
        if (((Mathf.Abs(differ) <= differ__Angle) || Mathf.Abs(differ) >= 360 - differ__Angle) && differ != 0)
        {
            // Debug.Log(differ + " : " + int.Parse(currentSpot.name) + " < " + checkClickSpot);
            difRot = wheel.transform.rotation;
            Quaternion finalRot = difRot * Quaternion.Euler(0, 0, differ);
            wheel.transform.rotation = finalRot;
            differ = 0;
        }
        RotationProgress = false;




    }


    // Get spot which is closest to line. (cyan debug)    
    private int ClosestSpot(int[] spotDist)
    {
        //Debug.Log(">>"+spotDist.Length);
        float minDist = Mathf.Infinity;
        int tMin = -1;
        Vector3 checkPos = line.position;
        foreach (int t in spotDist)
        {
            float dist = Vector3.Distance(spots[t].transform.position, line.position);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }


    // Is touching ui
    public bool IsPointerOverUIObject(string obj)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if (results.Count > 0)
            return results[0].gameObject.CompareTag(obj);
        else
            return false;
    }






    //Spawn new square
    public void ClickSpawn(Transform spawnPosition = null)
    {

        //********************TUTORIAL*********CLICKSPAWN
        if (tutorialManager.tutorialStep == 0)
        {
            tutorialManager.tutorialTrigger.Invoke();
        }
        //*****************************




        //squareSpawn.GetComponent<Rigidbody2D>().
        //spawn a square
        if (spawnPosition != null)
            squareSpawn = Instantiate(squarePrefab, spawnPosition.position, Quaternion.identity);
        else
            squareSpawn = Instantiate(squarePrefab, currentSpawn.transform.position, Quaternion.identity);


        squareSpawn.GetComponent<Square>().IsSpawn = true;
        //Debug.Log(squareSpawn.GetComponent<Square>().IsSpawn);
        squareSpawn.GetComponent<Square>().Score = next_score;


        ////****************************TUTORIAL**************************************************
        //if (PlayerPrefs.GetInt("TutorialStep", 0) < 2)
        //{
        //    int tutStep = PlayerPrefs.GetInt("TutorialStep", 0);

        //    switch (tutStep)
        //    {
        //        //For Tap
        //        case 0:
        //            {
        //                tutNum = 2;

        //                break;
        //            }
        //        //for Rotate

        //        case 1:
        //            {

        //                tutNum = 2;
        //                break;
        //            }
        //        //for Merge

        //        case 2:
        //            {
        //                tutNum = 4;

        //                break;
        //            }
        //        //for Hammer

        //        case 3:
        //            {
        //                tutNum = 4;

        //                break;
        //            }
        //        //for Bomb

        //        case 4:
        //            {

        //                tutNum = 2;
        //                break;
        //            }
        //        //For Drill

        //        case 5:
        //            {

        //                tutNum = 2;
        //                break;
        //            }



        //        default:
        //            tutNum = 2;
        //            break;
        //    }

        //    next_score = tutNum;
        //    nextScore.text = next_score.ToString();

        //}
        ////**************************************************************************
        //else
            //get score for next turn (non-inclusive)
            next_score = (int)Mathf.Pow(2, Random.Range(1, maxScore + 1));



        nextScore.text = next_score.ToString();
        ApplyStyle(next_score);



    }


    //Build circle for spots
    public Vector3 RandomCircle(Vector3 center, float radius, int a)
    {
        //Debug.Log(a);
        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }





    //=======================================INPUT END ==============================================




    //Sets up spots for spawns
    public void GetSpots()
    {
        rad = wheel.transform.GetChild(0).GetComponent<CircleCollider2D>().radius;
        center = wheel.transform.position;

        //Spots, spawns and grid for movement
        SpawnSpots(spotPrefab, rad, 1, spots);
        SpawnSpots(spawnPrefab, rad + 5.5f, 1, spawns);

        currentSpot = spots[0];
        currentSpawn = spawns[0];

        SpawnSpots(gridPrefab, rad + 0.55f, 5, null, grids);
    }

    //Get spots, spawns and grid
    private void SpawnSpots(GameObject prefab, float rad, int count, List<GameObject> lists = null, GameObject[,] grids = null)
    {
        for (int i = 0; i < nBottom; i++)
        {
            for (int j = 0; j < count; j++)
            {
                int a = 360 / nBottom * i;
                var pos = RandomCircle(center, rad + 0.9f * j, a);
                GameObject tmp = Instantiate(prefab, pos, Quaternion.identity);
                //tmp.transform.LookAt(new Vector3(0, -7f, 0),Vector3.down);

                tmp.name = i.ToString();

                int toggle = 0;

                if (prefab.CompareTag("spot"))
                {
                    toggle = 0;
                }
                else if (prefab.CompareTag("spawn"))
                {
                    toggle = 1;
                }
                else if (prefab.CompareTag("grid"))
                {
                    toggle = 2;
                }
                //rotate to face camera
                tmp.transform.SetParent(wheel.transform.GetChild(toggle));
                tmp.transform.LookAt(center, Vector3.right);
                tmp.transform.Rotate(0, 90, -90);
                if (grids != null)
                {
                    grids[i, j] = tmp;
                    tmp.transform.SetParent(GameManager.Instance.spawns[i].transform);
                    tmp.name = (j + 1).ToString();
                }
                else
                {
                    lists.Add(tmp);
                }
            }
        }
    }



    #endregion /Input

    //Vertical merge
    public void Merge(GameObject first, List<GameObject> rowObjs, GameObject second = null, bool IsPowerUp = false)
    {
        int fltScore;

        //Check if vertical merge or horizontal merge
        if (rowObjs != null)
        {
            //Debug.Log(rowObjs.Count);
            fltScore = rowObjs.Count;
        }
        else
        {
            fltScore = 2;
        }


        StartCoroutine(StopMerge(first, fltScore, second, IsPowerUp));

        ////********************TUTORIAL*********MERGE
        //if (tutorialManager.tutorialStep == 2)
        //{
        //    //Debug.Log("NO ROTATION");
        //    tutorialManager.tutorialTrigger.Invoke();
        //    GameManager.Instance.tutorialManager.powerUpAnim[0].SetBool("Highlight", true);
        //    NoClickSpawn = true;
        //}
        ////*****************************

    }


    //Delay for merge
    private IEnumerator StopMerge(GameObject first, int fltScore, GameObject second = null, bool IsPowerUp = false)
    {
        //Play sound if powerup
        if (IsPowerUp && first.transform.parent != null)
        {
            AudioManager.Instance.PlaySound("powerup");
        }


        int tmp;
        tmp = first.GetComponent<Square>().Score *= 2;
        int tmpScore;
        //Stop checks while Merging
        if (first == null)
            yield break;

        //if vertical
        if (second != null)
            first.GetComponent<Square>().IsMerging = true;
        //if horizontal
        else
            first.GetComponent<Square>().IsSwooping = true;

        //for float text positioning and s core
        if (second == null)
            tmpScore = (fltScore + 1) * first.GetComponent<Square>().Score;
        else
            tmpScore = (fltScore) * first.GetComponent<Square>().Score;


        //double the score
        if (first != null && !first.GetComponent<Square>().DoublingPriority)
        {
            first.GetComponent<Square>().DoublingPriority = true;
            //avoid 512 and higher
            if (tmp > 256)
                tmp = 256;
        }
        else
            tmp = 0;



        yield return new WaitForSeconds(0.2f);


        if (!IsPowerUp)
        {
            //========================Text floating===================================================

            Vector3 fltOffset = new Vector3(0f, 0.1f, 5f);
            //Get some text out
            if (first != null)
            {
                GameObject textObj = Instantiate(FltText, first.transform.position, first.transform.rotation);

                //if vertical - at second
                if (second != null)
                {
                    AudioManager.Instance.PlaySound("merge");
                    textObj.transform.position = second.transform.TransformPoint(second.transform.localPosition + fltOffset);
                }
                //if horisontal - instantiate it at 1st square pos
                else
                {

                    // if horizontal AND more than 2
                    if (fltScore > 1)
                    {
                        GameObject pizzaz = Instantiate(pizzazPref, first.transform.position, Quaternion.identity);

                        AudioManager.Instance.PlaySound("lightning");

                        if (first.GetComponent<Square>().Score == 256)
                        {
                            first.GetComponent<Square>().DoubleCoins = true;
                        }
                        //move to pizzaz location
                        textObj.transform.position = first.transform.TransformPoint(first.transform.localPosition - fltOffset);

                        textObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.cyan;
                        textObj.transform.localScale *= 2;
                    }
                    //if just 2
                    else
                    {
                        AudioManager.Instance.PlaySound("swoop");
                        textObj.transform.position = first.transform.TransformPoint(first.transform.localPosition + fltOffset);
                    }
                }

                //if not moving
                if (first != null && !first.transform.parent.CompareTag("outer"))
                    textObj.transform.SetParent(wheel.transform.GetChild(1).GetChild(int.Parse(first.transform.parent.name)));

                // flt text text
                textObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + tmpScore.ToString();
            }
            else
            {
                GameObject textObj = Instantiate(FltText, first.transform.position, first.transform.rotation);

                if (second != null)
                {
                    textObj.transform.position = second.transform.TransformPoint(second.transform.localPosition + fltOffset);
                    textObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+" + tmpScore.ToString();
                }
            }

            //=======================

        }


        if (first != null && first.CompareTag("square"))
        {
            //update the square score
            first.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = tmp.ToString();

            if (first.GetComponent<Square>().Score == 256)
            {
                //TELL SQUARE THAT IT'S 256
                first.GetComponent<Square>().IsTop = true;

                //        Instantiate(coinPrefab, first.transform.position, Quaternion.identity, first.transform.parent);
                //        Destroy(first);

            }
        }

        Instantiate(swoopPref, first.transform.position, Quaternion.identity);
        Destroy(second);
        //if (first == null)
        //{
        //    //yield break;
        //}
        if (first != null && (first.CompareTag("square") || first.CompareTag("256")))
        {


            //update the image
            first.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = tmp.ToString();
            first.GetComponent<Square>().ApplyStyle(tmp);

            if (tmp > scoreUpper && tmp <= 256)
            {
                if (tmp != 256)
                    AudioManager.Instance.PlaySound("pickup");
                scoreUpper *= 2;
                //Instance.upper.text = string.Format("{0}", scoreUpper);
                //uiSquarePrefab.SetActive(true);
                //UISquare.Instance.ApplyUiStyle(scoreUpper);

            }
            //first.GetComponent<Square>().Touched = false;
            first.GetComponent<Square>().MergeCheck = false;
            first.GetComponent<Square>().IsMerging = false;
            first.GetComponent<Square>().IsSwooping = false;
            MergeInProgress = false;

            //add score of tmpsSquare

            scores += tmpScore;
            ProgressUpdate(tmpScore);

            if (scores >= highscores)
            {

                highscores = scores;
                highScoreText.gameObject.SetActive(false);
                //highScoreText.gameObject.GetComponent<Image>().sprite = relaxIcon;

                if (SceneManager.GetActiveScene().name == "Relax")
                {

                    PlayerPrefs.SetInt("HighscoreRelax", scores);
                }
                else if (SceneManager.GetActiveScene().name == "Main")
                {

                    PlayerPrefs.SetInt("HighscoreTimed", scores);
                }
                else if (SceneManager.GetActiveScene().name == "Dzen")
                {
                    PlayerPrefs.SetInt("HighscoreDzen", scores);
                }



                if (!highscoreSoundBool)
                {
                    highscoreSoundBool = true;
                    AudioManager.Instance.PlaySound("highscore");

                }
            }
            else
            {
                highscoreSoundBool = false;
            }


            scoreText.text = scores.ToString();

            first.GetComponent<Square>().DoublingPriority = false;
        }

        // Tell square that it is 256
        if (first.GetComponent<Square>().Score >= 256)
        {

            first.GetComponent<Square>().IsTop = true;
        }
    }




    //Checkrow and pop coroutine
    public IEnumerator Turn()
    {
        checkRowIndex = 0;
        // List<List<GameObject>> tmppopObjs = popObjs;
        //Debug.Log(" TURN " + checkObjs.Count);
        //if there's something in the queue
        while (turnCheckObjs.Count > 0)
        {
            //Grab first element
            GameObject tmpObj = turnCheckObjs.Pop();

            int tmpDist = 99;

            //if there's still something in there
            if (turnCheckObjs.Count > 1)
            {
                foreach (GameObject chObject in turnCheckObjs)
                {
                    //Check if object is still there
                    if (chObject == null)
                        continue;
                    //if tmpObj destroyed - move on
                    if (tmpObj == null)
                        break;
                    //check distance between (including passing through 0)
                    tmpDist = Mathf.Abs(int.Parse(tmpObj.transform.parent.name) - int.Parse(chObject.transform.parent.name));
                    if (Mathf.Abs(nBottom - tmpDist) <= tmpDist)
                        tmpDist = Mathf.Abs(nBottom - tmpDist);

                    //if chObject in list is furthering and same score -> this is tmpScore
                    if (chObject.GetComponent<Square>().Score == tmpObj.GetComponent<Square>().Score)
                    {
                        //If it's close enough and same score and nextobj is further and same index
                        if (tmpDist <= 3 && tmpObj.GetComponent<Square>().Score == chObject.GetComponent<Square>().Score
                        && chObject.GetComponent<Square>().Further /*&& tmpObj.GetComponent<Square>().RowObjIndex == chObject.GetComponent<Square>().RowObjIndex*/)
                        {
                            tmpObj = chObject;
                            continue;
                        }
                    }
                }

            }

            //if square reached 256 - ignore
            if (tmpObj != null && tmpObj.GetComponent<Square>().Score >= 256)
            {
                //reset 
                TurnInProgress = false;
                continue;
            }

            //If doesnt pop with further and same score - check this one
            if (tmpObj != null)
            {
                if (!tmpObj.transform.parent.CompareTag("outer") && !tmpObj.GetComponent<Square>().IsMerging)
                    CheckRow(int.Parse(tmpObj.transform.parent.name), tmpObj.transform.GetSiblingIndex(), tmpObj.GetComponent<Square>().Score, tmpObj);

            }
            else
            {
                //Debug.Log("DADADDA");
                TurnInProgress = false;
                continue;
            }

            yield return new WaitForSeconds(0.01f);

            //Continue with pop
            yield return StartCoroutine(StopPop(popObjs, tmpSquares, wheel));
            //reset 
            TurnInProgress = false;
            //Reset checkRowIndex for next Turn;
            checkRowIndex++;
        }

    }

    //Reset Expand Moves  (TIME EXPAND)
    public void ResetExpandTime()
    {
        Expand();
        //if(SceneManager.GetActiveScene().name == "Relax")
        //{
        sliderFill = (float)(expandMoves - fMoves) / expandMoves;
        StartCoroutine(FillStop());
        //}



    }


    //////////Reset Expand Moves Relax Mode
    public void ResetExpand(GameObject tmpSquare)
    {
        // expand moves++ if this happened by player
        if (tmpSquare.GetComponent<Square>().IsSpawn)
        {

            ExpandMoves();

            tmpSquare.GetComponent<Square>().IsSpawn = false;

            if (Moves > expandMoves - 1)
            {

                Expand();


                StartCoroutine(FillStop());
                sliderFill = (float)(expandMoves - Moves) / expandMoves;

                //expandMoves += expandMoves/2;
                //nextShrink.text = string.Format("256: {0}", expandMoves - Moves);
            }


        }
    }


    //Checks for 3 in a row
    public void CheckRow(int spotIndex, int squareIndex, int checkScore, GameObject tmpSquare)
    {
        if (tmpSquare != null)

        {

            tmpSquare.GetComponent<Square>().CheckPriority = true;
            CheckInProgress = true;
            //Debug.Log("(INIT) " + tmpSquare.transform.parent.name + " : " + tmpSquare.transform.GetSiblingIndex() + " >> " + tmpSquare.GetComponent<Square>().Score);



            if (!tmpSquare.GetComponent<Square>().Further)
            {

                //AudioManager.Instance.PlaySound("bump");
            }

            else
                tmpSquare.GetComponent<Square>().Further = false;


            //Debug.Log("ScheckRow"+spotIndex);
            List<GameObject> rowObjs = new List<GameObject>();

            //iterator for list
            int index = spotIndex;
            //noMoves = false;
            //index of start of rowObjs (outside nbottom numbers to be safe)
            int startIndex = nBottom + 10;
            int endIndex = nBottom + 11;
            int firstIndex = 0;
            int nextIndex = 0;

            //add placed square to rowOBjs

            rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
            spots[index].transform.GetChild(squareIndex).gameObject.GetComponent<Square>().RowObjIndex = checkRowIndex;

            do
            {
                //if there's no start yet
                if (startIndex > nBottom)
                {
                    //passing through 0
                    if (index - 1 < 0)
                    {
                        index = nBottom - 1;
                    }
                    else
                        index = index - 1;

                    //check next left one after getting index-1
                    if (index - 1 < 0)
                    {
                        firstIndex = nBottom - 1;
                    }
                    else
                        firstIndex = index - 1;


                    //if there's square with same squareIndex
                    if (spots[index].transform.childCount > squareIndex)
                    {
                        //if its score is the same
                        if (spots[index].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore
                            && !spots[index].transform.GetChild(squareIndex).GetComponent<Square>().IsMerging
                            )
                        {
                            //if there's nothing to the left
                            if (spots[firstIndex].transform.childCount < squareIndex + 1)
                            {

                                rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                                spots[index].transform.GetChild(squareIndex).gameObject.GetComponent<Square>().RowObjIndex = checkRowIndex;

                                //if never set up yet
                                if (startIndex > nBottom)
                                {
                                    // start pf a row
                                    startIndex = index;
                                    index = spotIndex;
                                    continue;
                                }

                            }
                            else
                            {
                                //or there's not that score
                                if (spots[firstIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score != checkScore)
                                {
                                    rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                                    spots[index].transform.GetChild(squareIndex).gameObject.GetComponent<Square>().RowObjIndex = checkRowIndex;

                                    //if never set up yet
                                    if (startIndex > nBottom)
                                    {
                                        startIndex = index;
                                        index = spotIndex;
                                        continue;
                                    }
                                }
                                //else if score is the same and not merging or checkrow
                                else if (spots[firstIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore
                                    && !spots[firstIndex].transform.GetChild(squareIndex).GetComponent<Square>().IsMerging
                                    && !spots[firstIndex].transform.GetChild(squareIndex).GetComponent<Square>().Further)
                                {
                                    rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                                    spots[index].transform.GetChild(squareIndex).gameObject.GetComponent<Square>().RowObjIndex = checkRowIndex;

                                    startIndex = nBottom + 10;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            //if score isnt same - move back to spotIndex and continue
                            startIndex = spotIndex;
                            index = spotIndex;
                            continue;
                        }
                    }
                    else
                    {
                        //if score isnt same - move back to spotIndex and continue
                        startIndex = spotIndex;
                        index = spotIndex;
                        continue;
                    }

                }
                //if we found left end
                else if (startIndex < nBottom)
                {
                    //passing through nBottom (0)
                    if (index + 1 > nBottom - 1)
                    {
                        index = 0;
                    }
                    else
                        index = index + 1;
                    //check next one after setting index+1
                    if (index + 1 > nBottom - 1)
                    {
                        nextIndex = 0;
                    }
                    else
                        nextIndex = index + 1;
                    //if there's square with same squareIndex
                    if (spots[index].transform.childCount > squareIndex)
                    {
                        //if its score is the same
                        if (spots[index].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore
                            && !spots[index].transform.GetChild(squareIndex).GetComponent<Square>().IsMerging
                            )
                        {
                            //if there's nothing to the right
                            if (spots[nextIndex].transform.childCount < squareIndex + 1)
                            {

                                rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                                spots[index].transform.GetChild(squareIndex).gameObject.GetComponent<Square>().RowObjIndex = checkRowIndex;
                                //if never set up yet
                                if (endIndex > nBottom)
                                {
                                    // start pf a row
                                    endIndex = index;
                                    break;
                                }

                            }
                            else
                            {
                                //or there's not that score
                                if (spots[nextIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score != checkScore)
                                {
                                    rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                                    spots[index].transform.GetChild(squareIndex).gameObject.GetComponent<Square>().RowObjIndex = checkRowIndex;

                                    //if never set up yet
                                    if (endIndex > nBottom)
                                    {
                                        endIndex = index;
                                        break;
                                    }
                                }
                                //else if score is the same and not merging or checkrow
                                else if (spots[nextIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score == checkScore
                                    && !spots[nextIndex].transform.GetChild(squareIndex).GetComponent<Square>().IsMerging
                                     && !spots[nextIndex].transform.GetChild(squareIndex).GetComponent<Square>().Further)

                                {
                                    rowObjs.Add(spots[index].transform.GetChild(squareIndex).gameObject);
                                    spots[index].transform.GetChild(squareIndex).gameObject.GetComponent<Square>().RowObjIndex = checkRowIndex;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            //if score isnt same - move back to spotIndex and continue
                            endIndex = spotIndex;
                            break;
                        }
                    }
                    else
                    {
                        //if score isnt same - move back to spotIndex and continue
                        endIndex = spotIndex;
                        break;
                    }
                }
            }
            while (endIndex > nBottom + 1);

            // nothing close row
            if (rowObjs.Count < 2)
            {


                //Check what's above
                // if (!tmpSquare.GetComponent<Square>().checkPriority)



            }
            else
            {

                //Get rid of the one we keep
                rowObjs.Remove(tmpSquare);

                //If there's no same rowObj in pop - add
                if (!popObjs.Contains(rowObjs))
                {
                    popObjs.Add(rowObjs);
                }

                //how many there's
                int fltScore = popObjs.Count;

                //add its tmpSquare to list
                if (!tmpSquares.Contains(tmpSquare))
                {

                    foreach (List<GameObject> checkRowObjs in popObjs)
                    {
                        //add only first tmpSquare if there's any in popObjs to pop
                        bool contained = false;
                        if (checkRowObjs.Contains(tmpSquare))
                        {
                            contained = true;
                        }


                        //foreach(GameObject go in checkRowObjs)
                        //{
                        //    //Debug.Log(" checkRowObjs: " + go.transform.parent.name);
                        //}


                        if (!contained)
                        {
                            tmpSquares.Add(tmpSquare);
                        }

                    }

                }

            }
        }




        CheckInProgress = false;
    }

    private IEnumerator FillStop()
    {
        yield return new WaitForSeconds(1f);
        Moves = 0;

        slider.fillAmount = 1;
    }



    //////////////////////////////////////////////////////////////////////////////////////CHECK FOR EACH IN COLUMN
    public void CheckAbove(int spotIndex, int squareIndex)
    {

        //if there's no start yet
        int index = spotIndex;
        int firstIndex;
        int nextIndex;

        //check next left one after getting index-1
        if (index - 1 < 0)
        {
            firstIndex = nBottom - 1;
        }
        else
            firstIndex = index - 1;

        //check next one after setting index+1
        if (index + 1 > nBottom - 1)
        {
            nextIndex = 0;
        }
        else
            nextIndex = index + 1;

        //Debug.Log(" I : " + index + " n " + nextIndex + " F " + firstIndex);

        if (spots[index].transform.childCount > squareIndex + 1)
        {
            //Debug.Log("CHECK ABOVE );
            //If same score above

            if (spots[index].transform.GetChild(squareIndex + 1).GetComponent<Square>().Score == spots[index].transform.GetChild(squareIndex).GetComponent<Square>().Score)
            {
                //Debug.Log("up " + spots[spotIndex].transform.GetChild(squareIndex).GetComponent<Square>().Score);
                if (!spots[index].transform.GetChild(squareIndex + 1).gameObject.GetComponent<Square>().IsMerging)
                {
                    //Debug.Log("MRG");
                    Merge(spots[index].transform.GetChild(squareIndex + 1).gameObject, null, spots[index].transform.GetChild(squareIndex).gameObject);
                }

                //spots[spotIndex].transform.GetChild(squareIndex + 1).localPosition += new Vector3(0f, +0.3f, 0f);
                // Destroy(spots[index].transform.GetChild(i).gameObject);
                return;
            }


            for (int i = squareIndex + 1; i < spots[index].transform.childCount; i++)
            {
                if (spots[index].transform.GetChild(i).GetComponent<Square>().ColumnPew)
                    return;


                ////CHECK SCORE to THE LEFT starting with row above
                if (spots[firstIndex].transform.childCount > i)
                {
                    //if its score is the same
                    if (spots[firstIndex].transform.GetChild(i).GetComponent<Square>().Score == spots[index].transform.GetChild(i).GetComponent<Square>().Score
                            && !spots[firstIndex].transform.GetChild(i).GetComponent<Square>().IsMerging
                                && !spots[firstIndex].transform.GetChild(i).GetComponent<Square>().Further)
                    {

                        //Debug.Log("left " + spots[index].transform.GetChild(i).GetComponent<Square>().Score);

                        //checkObjs.Enqueue(spots[index].transform.GetChild(i).gameObject);
                        //spots[index].transform.GetChild(i).localPosition += new Vector3(0f, 0.3f, 0f);
                        StartCoroutine(StopEnqueue(spots[index].transform.GetChild(i).gameObject));
                        spots[index].transform.GetChild(i).GetComponent<Square>().ColumnPew = true;
                        break;
                    }
                }
                //CHECK SCORE TO the RIGHT
                if (spots[nextIndex].transform.childCount > i)
                {
                    //if its score is the same
                    if (spots[nextIndex].transform.GetChild(i).GetComponent<Square>().Score == spots[index].transform.GetChild(i).GetComponent<Square>().Score
                            && !spots[nextIndex].transform.GetChild(i).GetComponent<Square>().IsMerging
                                && !spots[nextIndex].transform.GetChild(i).GetComponent<Square>().Further)
                    {
                        //Debug.Log("right " + spots[index].transform.GetChild(i).GetComponent<Square>().Score);

                        StartCoroutine(StopEnqueue(spots[index].transform.GetChild(i).gameObject));
                        //spots[index].transform.GetChild(i).localPosition += new Vector3( 0f, 0.3f, 0f);

                        spots[index].transform.GetChild(i).GetComponent<Square>().ColumnPew = true;
                        break;
                    }
                }

            }

        }


    }

    //Check Objs delay
    public IEnumerator StopEnqueue(GameObject checkObj)
    {

        yield return new WaitForSeconds(0.1f);
        checkObjs.Push(checkObj);
        //Debug.Log(checkObjs.Count);
    }



    //Pop coroutine
    IEnumerator StopPop(List<List<GameObject>> thisPopObjs, List<GameObject> tmpSquares, GameObject wheel)
    {

        int count = 0;

        yield return new WaitForSeconds(0.01f);

        foreach (List<GameObject> rowObjs in thisPopObjs)
        {

            //Debug.Log("STARTING COURUTINE   " + thisPopObjs.Count + " === " + rowObjs[0].GetComponent<Square>().Score);

            if (tmpSquares.Count > count && tmpSquares[count] != null)
            {
                if (tmpSquares[count].transform.parent.CompareTag("outer"))
                {
                    tmpSquares[count].GetComponent<Square>().CheckPriority = false;
                    //count++;
                    thisPopObjs.Remove(rowObjs);
                    continue;
                }

                Pop(rowObjs, tmpSquares[count]);
                tmpSquares[count].GetComponent<Square>().CheckPriority = false;

                //yield return new WaitForSeconds(0.2f);
                //tmpSquares.Clear();
                StartCoroutine(FurtherPops(tmpSquares[count]));
                count++;
            }
            //yield return new WaitForSeconds(0.2f);

        }



        //Clear pop objs list
        for (int i = 0; i < popObjs.Count; i++)
        {
            if (popObjs[i].Count == 0)
            {
                //Debug.Log(">>>>>>>>>>>>>>>>>>>>" + popObjs[i].Count);
                popObjs.RemoveAt(i);
                tmpSquares.RemoveAt(i);
            }
        }


    }


    //Kill all adjacent squares
    public void Pop(List<GameObject> rowObjs, GameObject tmpSquare = null)
    {

        //get a private tmpSquare ref
        GameObject tmpTmpSquare = tmpSquare;
        //Keep one that has fallen

        //Debug.Log("MRG");
        Merge(tmpTmpSquare, rowObjs);

        // Move others
        if (rowObjs.Count != 0)
        {
            foreach (GameObject rowObj in rowObjs)
            {
                //temp for reference
                GameObject tmprowObj = rowObj;
                //Update the score
                if (tmprowObj != null)
                {




                    if (!tmprowObj.transform.parent.CompareTag("outer"))
                    {
                        //rowObj.transform.position += new Vector3(0, 0, 10);

                        tmprowObj.transform.parent.GetComponent<SpriteRenderer>().color = leGreen;


                    }
                    if (tmprowObj.transform != null && tmpTmpSquare != null && tmpTmpSquare.GetComponent<Square>().Desto != tmpTmpSquare
                        && !tmprowObj.transform.parent.CompareTag("outer"))
                    {

                        tmprowObj.GetComponent<Square>().SquareTmpSquare = tmpTmpSquare.transform;
                        tmprowObj.GetComponent<Collider2D>().isTrigger = true;

                        FurtherProgress = true;
                        //furtherScore = tmprowObj.GetComponent<Square>().Score;
                        //for avoiding double collapses
                        if (!tmprowObj.GetComponent<Square>().IsMerging)
                            StartCoroutine(FurtherPops(tmprowObj));

                        //Detach this square from parent
                        tmprowObj.transform.parent = tmprowObj.transform.parent.parent.parent.GetChild(3);
                    }
                }



            }
            rowObjs.Clear();
        }

    }


    //Merge after pop coroutine
    public IEnumerator FurtherPops(GameObject tmpSquare)
    {
        GameObject furthertmpSquare = tmpSquare;
        yield return new WaitForSeconds(0.2f);
        //if something below -> move on
        if (tmpSquare != null && tmpSquare.transform.GetSiblingIndex() > 1)
        {
            //if same score below
            if (tmpSquare.transform.parent.GetChild(tmpSquare.transform.GetSiblingIndex() - 1).GetComponent<Square>().Score == tmpSquare.GetComponent<Square>().Score)
            {
                if (tmpSquare.GetComponent<Square>().IsMerging)
                {
                    //Debug.Log("SOMETHING BELOW " + tmpSquare.transform.parent.name);
                    yield break;
                }

                else
                {
                    yield return new WaitForSeconds(0.1f);
                    checkObjs.Push(tmpSquare);
                }
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                checkObjs.Push(tmpSquare);
            }
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            checkObjs.Push(tmpSquare);
        }
        //else if (tmpSquare.GetComponent<Square>().Further)
        //    checkObjs.Enqueue(tmpSquare);


        if (tmpSquare != null && tmpSquare.GetComponent<Square>().Score != 256)
        {
            tmpSquare.GetComponent<Square>().Further = true;
            //Debug.Log("FURTHER");
            if (tmpSquare.transform.GetSiblingIndex() > 0)
            {
                //if one below is same score - merge
                if (!tmpSquare.transform.parent.CompareTag("outer") && tmpSquare.transform.parent.GetChild(tmpSquare.transform.GetSiblingIndex() - 1).GetComponent<Square>().Score == tmpSquare.GetComponent<Square>().Score)
                {

                    Merge(tmpSquare, null, tmpSquare.transform.parent.GetChild(tmpSquare.transform.GetSiblingIndex() - 1).gameObject);
                    //Debug.Log("MRG BLW");    
                    //furthertmpSquare.transform.localPosition += new Vector3(0f, +0.3f, 0f);
                    //Debug.Log("PEW " + tmpSquare.transform.parent.name);
                }
            }



            if (tmpSquare != null && !tmpSquare.transform.parent.CompareTag("outer"))
                CheckAbove(int.Parse(furthertmpSquare.transform.parent.name), furthertmpSquare.transform.GetSiblingIndex());

            //if (tmpSquare.GetComponent<Collider2D>().isTrigger != true)
            //{
            furthertmpSquare.GetComponent<Square>().CheckPriority = false;

            //    tmpSquare.GetComponent<Square>().CheckAround = true;
            //   
            //}


        }



        FurtherProgress = false;


    }


    //Update moves
    public void ExpandMoves()
    {
        if (SceneManager.GetActiveScene().name == "Relax")
        {
            Moves++;
            sliderFill = (float)(expandMoves - Moves) / expandMoves;

            //NextShrink.text = string.Format("256: {0}", expandMoves - Moves);
        }

        StartCoroutine(SliderStop());
    }



    //Spawn randoms
    public void Expand()
    {
        //spawn only 2 when upper is 8
        if (scoreUpper == 8)
            randSpawnCount = tmpRands - 1;
        else
            randSpawnCount = tmpRands;

        RandValues tmp = new RandValues();
        rands = new List<RandValues>();

        //Rand Score checker
        List<int> randCheckTmp = new List<int>();

        //randSpawn is upper Power -1 always
        int upperPow = (int)Mathf.Log(scoreUpper, 2) - 1;
        List<int> randList = new List<int>();
        //get free spots

        //Add available spots for rand spawns to the list
        foreach (GameObject spot in spots)
        {
            if (spot.transform.childCount < 5 && spot.name != currentSpot.name)
            {
                randList.Add(int.Parse(spot.name));
            }
        }

        //if there's atleast 3 spots to spawn randoms
        if (randList.Count >= randSpawnCount)
        {
            rands.Clear();

            for (int i = 0; i < randSpawnCount; i++)
            {
                tmp.Rng = randList[Random.Range(0, randList.Count)];
                tmp.RandScore = 0;

                if (rands.Count >= 1 && ListContains(rands, tmp))
                {
                    while (ListContains(rands, tmp))
                    {
                        tmp.Rng = randList[Random.Range(0, randList.Count)];
                        tmp.RandScore = 0;
                    }
                }

                //Get scores for each randspawn
                do
                {
                    tmp.RandScore = (int)Mathf.Pow(2, Random.Range(1, upperPow + 1));
                }
                while (randCheckTmp.Contains(tmp.RandScore) || tmp.RandScore >= 128);
                randCheckTmp.Add(tmp.RandScore);

                rands.Add(tmp);
            }

            StartCoroutine(StopRandom(rands));
        }
        else
        {
            rands.Clear();
        }
    }

    //Spread random spawns
    private IEnumerator StopRandom(List<RandValues> rands)
    {
        randSpawning = true;
        //spawn all
        foreach (RandValues rand in rands)
        {
            yield return new WaitForSeconds(0.2f);
            SpawnRandom(rand.Rng, rand.RandScore);
        }
        randSpawning = false;
    }


    //Check if list of classes contains same rng
    private bool ListContains(List<RandValues> list, RandValues check)
    {
        foreach (RandValues n in list)
        {
            if (n.Rng == check.Rng)
            {

                return true;
            }

        }
        return false;
    }


    //Check if list of classes contains same rng
    private bool RandScoreContains(List<RandValues> list, RandValues check)
    {
        foreach (RandValues n in list)
        {
            if (n.RandScore == check.Rng)
            {
                return true;
            }
        }
        return false;
    }


    // Spawn randomSquare
    public void SpawnRandom(int rng, int randScore)
    {
        randSpawns = new List<GameObject>();
        //Debug.Log("SPAWN");
        //spawn a square at random spot with random score
        randSpawn = Instantiate(squarePrefab, spawns[rng].transform.position, Quaternion.identity);




        randSpawn.GetComponent<Square>().ExpandSpawn = true;
        randSpawn.GetComponent<Square>().Score = randScore;
        randSpawn.transform.SetParent(spots[rng].transform);
        randSpawn.name = randSpawn.transform.GetSiblingIndex().ToString();

        //Rotate spawns towards center
        Vector3 diff = randSpawn.transform.parent.position - randSpawn.transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        randSpawn.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);

        //make spot red if 6th child
        if (randSpawn.transform.parent.childCount == 5)
        {
            randSpawn.transform.parent.GetComponent<SpriteRenderer>().color = leRed;
        }




        randSpawns.Add(randSpawn);
    }






    //Check if gameOver
    public void GameOver(GameObject chk = null)
    {
        //Debug.Log(" game Over ");
        if (chk != null)
        {
            if (chk.transform.childCount >= 5 && !gameOverInProgress)
            {
                gameOverInProgress = true;
                //full spot colors red and opens another one
                chk.GetComponent<SpriteRenderer>().color = leRed;
                // 1 column gameover

            }
            else if (chk.transform.childCount == 4)
            {
                //alert of 4 column

                chk.GetComponent<SpriteRenderer>().color = leYellow;
            }


        }


        //for long game
        //StartCoroutine(StopGameOver());
    }




    //Game over short game
    public IEnumerator StopGameOverShort(GameObject chk = null)
    {

        yield return new WaitForSeconds(0.4f);
       

        if (chk.GetComponent<SpriteRenderer>().color == leRed)
        {
            if (chk.transform.GetChild(chk.transform.childCount - 1) != null)
            {
                noMoves = true;
                yield return new WaitForSeconds(2f);
                if (chk.GetComponent<SpriteRenderer>().color == leRed)
                {
                    AudioManager.Instance.PlaySound("end");
                    GameOverBool = true;
                    gameOverInProgress = false;
                    OpenMenu(true);
                    nextScore.text = "GAMEOVER";
                    //menu.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = string.Format("{0}\n Highscore\n{0}\n Top", scores, scoreUpper);
                }
                else
                {
                    gameOverInProgress = false;
                    noMoves = false;
                }
            }
        }

        
        Debug.Log("BACK");
        if (chk != null)
            chk.GetComponent<Spot>().GameOverCheckSpot = true;

    }





    ////Game over long game
    //private IEnumerator StopGameOver()
    //{

    //    int reds = 0;
    //    yield return new WaitForSeconds(0.4f);
    //    foreach (GameObject spot in spots)
    //    {
    //        if (spot.GetComponent<SpriteRenderer>().color == leRed)
    //        {
    //            if (spot.transform.GetChild(spot.transform.childCount - 1) != null)
    //            {
    //                reds++;

    //            }
    //        }

    //    }
    //    //Debug.Log("reds " + reds);
    //    if (reds == spots.Count)
    //    {
    //        noMoves = true;
    //        yield return new WaitForSeconds(1f);
    //        if (reds == spots.Count)
    //        {
    //            AudioManager.Instance.PlaySound("end");

    //            OpenMenu(true);
    //            nextScore.text = "GameOver";
    //            //menu.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = string.Format("{0}\n Highscore\n{1}\n Top", scores, scoreUpper);
    //        }
    //        else
    //            noMoves = false;
    //    }
    //}





    // ====================================================MENU FUNCTIONS




    //Toggle menu
    public void OpenMenu(bool gameOver = false)
    {
        if(PlayerPrefs.GetString("PlayerName","offlineUser") != "offlineUser")
            Highscores.Instance.AddNewHighscore(PlayerPrefs.GetString("PlayerName", "offlineUser"), scores, dbSceneIndex);
        Debug.Log("WRITTEN ");

        //scoreText
        menu.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = string.Format("{0}", scores);
        menu.transform.GetChild(0).GetChild(4).GetComponent<Text>().text = string.Format("{0}\nHIGHSCORE", highscores);

        //upperText
        menu.transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = string.Format("<color=white>{0}</color>", scoreUpper.ToString());

        if (gameOver)
        {
            //PlayGamesScript.AddScoreToLeaderBoard(Threesixty.leaderboard_leaderboards, scores);
            //UIScript.Instance.UpdatePointsText();
        }

        //if it is GAME OVER (endGameCheck for cooldown continue)
        if (gameOver && !endGameCheck)
        {
            menu.SetActive(true);
            ui.SetActive(!menu.gameObject.activeSelf);

            //GameOver
            menu.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            menu.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            //Cooldown for replay
            StartCoroutine(ContinueTime(menu.transform.GetChild(0).GetChild(0).GetChild(0).gameObject));

        }
        else if (gameOver && endGameCheck)
        { 
            //GameOver
            menu.SetActive(true);
            ui.SetActive(!menu.gameObject.activeSelf);

            menu.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            menu.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);

        }
        //If just Open menu mid game
        else
        {
            if (MenuUp && !AdInProgress && !GameOverBool)
            {

                CoinManager.Instance.MenuAd();
                AdInProgress = true;
            }
            else if (MenuUp && AdInProgress)
            {
                AdInProgress = false;
            }

            menu.SetActive(!menu.activeSelf);
            ui.SetActive(!menu.activeSelf);

            MenuUp = !MenuUp;
        }
    }


    //Restarts game
    public void Restart()
    {
        serializer.CreateNewGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        menu.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        AdInProgress = true;
        OpenMenu();
        //GameOverMenu.SetActive(false);
        //In case game was paused before
        Time.timeScale = 1;
    }

    //Quit
    public void Quit()
    {
        SaveGame();
        Application.Quit();
    }


    //On minimize - save, time = 0

    private void OnApplicationPause(bool value)
    {
        if (value)
        {
           

            if (!gameOverInProgress && !GameOverBool)
            {
                if (!MenuUp && !AdInProgress)
                {
                    OpenMenu();

                }
                else if (AdInProgress)
                {

                    OpenMenu();
                }
                SaveGame();
            }
            else
            {
                NewGame();
            }
        }


    }

    //private void OnApplicationFocus(bool focus)
    //{
    //    Debug.Log("EHEHEHEHEHEHE");
    //    if (gameOverInProgress && GameOverBool)
    //    {
    //        if (MenuUp && AdInProgress)
    //        {
    //            OpenMenu();
    //            AdInProgress = false;
    //        }
    //    }
    //}

    private IEnumerator ContinueTime(GameObject button)
    {
        if (noMoves == true)
        {
            button.GetComponentInChildren<Slider>().value = 1;
            float contTimer = 10f;
            button.GetComponent<Animation>().Play();
            while (contTimer > 0)
            {

                if (noMoves == false)
                    yield break;

                contTimer -= 0.01f;
                button.GetComponentInChildren<Slider>().value -= 0.001f;
                yield return new WaitForSeconds(0.01f);
            }

            button.SetActive(false);
        }
        else yield break;

    }



    //Button functions

    public void Continue()
    {
        OpenMenu();
        GameOverBool = false;
        noMoves = false;
        MenuUp = false;
        //hide button
        menu.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        menu.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);

        menu.transform.GetChild(0).GetChild(0).gameObject.GetComponentInChildren<Slider>().value = 1;
        //Hide Continue for 2nd run
        menu.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        nextScore.text = next_score.ToString();

        foreach (GameObject spot in spots)
        {
            if (spot.transform.childCount == 5)
            {
                spot.transform.DetachChildren();
                spot.GetComponent<SpriteRenderer>().color = leGreen;
            }
        }
        endGameCheck = true;
    }

    public void ChangeTheme(int index)
    {
        themeIndex = index;
        PlayerPrefs.SetInt("Theme", themeIndex);
        Restart();
        //InitializeTheme();


    }



    public void SaveGame()
    {
        currentBoard = new Board();
        currentBoard.pieces = new List<Piece>();
        currentBoard.highscore = new int();

        currentBoard.highscore = scores;


        for (int i = 0; i < wheel.transform.GetChild(0).childCount; i++)
        {
            for (int j = 0; j < wheel.transform.GetChild(0).GetChild(i).childCount; j++)
            {
                Piece spawnPiece = new Piece();
                GameObject tmp = wheel.transform.GetChild(0).GetChild(i).GetChild(j).gameObject;

                spawnPiece.score = tmp.GetComponent<Square>().Score;
                spawnPiece.position.x = int.Parse(tmp.transform.parent.name);
                spawnPiece.position.y = tmp.transform.GetSiblingIndex();

                currentBoard.pieces.Add(spawnPiece);

            }

        }

        if (!gameOverInProgress || !GameOverBool)
        {
            serializer.SaveGameBinary(currentBoard);

        }
        else if (gameOverInProgress || GameOverBool)
        {
            serializer.CreateNewGame();
            
        }
      

      

    }

    public void LoadGame(string fileName)
    {
        
        currentBoard = serializer.LoadGameBinary(fileName);

        
        //Update last score
        scores = currentBoard.highscore;
        scoreText.text = scores.ToString();

        if (currentBoard.pieces != null)
        {
            foreach(Piece p in currentBoard.pieces)
            {
                int lScore = p.score;
                int lSpot = (int)p.position.x;
                int lIndex = (int)p.position.y;

                //Debug.Log(p.position);
                GameObject lSpawn = Instantiate(squarePrefab, spawns[lSpot].transform.GetChild(lIndex).position, Quaternion.identity);




                lSpawn.GetComponent<Square>().ExpandSpawn = true;
                lSpawn.GetComponent<Square>().Score = lScore;
                lSpawn.transform.SetParent(spots[lSpot].transform);
                lSpawn.name = lSpawn.transform.GetSiblingIndex().ToString();

                //Rotate spawns towards center
                Vector3 diff = lSpawn.transform.parent.position - lSpawn.transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                lSpawn.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);


            }

        }
        
    }

    public void NewGame()
    {
        serializer.CreateNewGame();
    }

    //public void TUTORIALSAVER()
    //{
    //    currentBoard = new Board();
    //    currentBoard.pieces = new List<Piece>();
    //    currentBoard.highscore = new int();

    //    currentBoard.highscore = scores;


    //    for (int i = 0; i < wheel.transform.GetChild(0).childCount; i++)
    //    {
    //        for (int j = 0; j < wheel.transform.GetChild(0).GetChild(i).childCount; j++)
    //        {
    //            Piece spawnPiece = new Piece();
    //            GameObject tmp = wheel.transform.GetChild(0).GetChild(i).GetChild(j).gameObject;

    //            spawnPiece.score = tmp.GetComponent<Square>().Score;
    //            spawnPiece.position.x = int.Parse(tmp.transform.parent.name);
    //            spawnPiece.position.y = tmp.transform.GetSiblingIndex();

    //            currentBoard.pieces.Add(spawnPiece);

    //        }

    //    }

      
    //    serializer.TutorialGameBinary(currentBoard);
    //}

    public void TweakAngle(string value)
    {
        spawn__Angle = float.Parse(value);
    }

    public void TweakDelay(string value)
    {
        follow__Delay = float.Parse(value);
    }

    public void TweakFollow(string value)
    {
        follow__Angle = float.Parse(value);
    }

    public void TweakDiffer(string value)
    {
        rotationDuration = float.Parse(value);
    }




}



