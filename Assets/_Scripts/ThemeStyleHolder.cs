
using UnityEngine;
using UnityEngine.UI;






// class for style options 
[System.Serializable]
public class ThemeStyle
{
    public ColorBlock shareButton;
    public Color shareButtonText;

    public GameObject wheelPref;
    public GameObject squarePref;
    public GameObject spotPref;
    public GameObject gridPref;
    public GameObject spawnPref;

    public GameObject backPref;
    public GameObject fltTextPref;
    public GameObject styleHolderPref;

    public Color32 yellowPref;
    public Color32 redPref;
    public Font fontPref;

   
    public GameObject uiPref;

    public Color32 menuPref;
    public Color32 linePref;
    public Color32 sliderTextPref;
    public Color32 sliderBackPref;
}


public class ThemeStyleHolder : Singleton<ThemeStyleHolder> {



    public  ThemeStyle[] ThemeStyles;




}
