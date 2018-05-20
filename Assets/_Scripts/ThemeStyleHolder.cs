
using UnityEngine;
using UnityEngine.UI;






// class for style options 
[System.Serializable]
public class ThemeStyle
{
    public GameObject wheelPref;
    public GameObject squarePref;
    public GameObject spotPref;

    public GameObject backPref;
    public GameObject buttonPref;
    public GameObject fltTextPref;
    public GameObject styleHolerPref;

    public Font fontPref;
 
  
}


public class ThemeStyleHolder : Singleton<ThemeStyleHolder> {



    public  ThemeStyle[] ThemeStyles;



    private void Awake()
    {
      
    }


}
