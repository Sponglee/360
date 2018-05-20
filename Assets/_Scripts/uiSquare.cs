using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISquare : Singleton<UISquare> {



    [SerializeField]
    private Text SquareText;
    [SerializeField]
    private Image SquareColor;
    [SerializeField]
    public Text upperText;


    // Helps ApplyStyle to grab numbers/color
    void ApplyStyleFromHolder(int index)
    {
        //SquareText.text = SquareStyleHolder.Instance.SquareStyles[index].Number.ToString();
        //SquareText.color = SquareStyleHolder.Instance.SquareStyles[index].TextColor;
        //SquareColor.color = SquareStyleHolder.Instance.SquareStyles[index].SquareColor;
    }

    //Gets Values from style script for each square
    public void ApplyUiStyle(int num)
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
            case 0:
                ApplyStyleFromHolder(2);
                break;
            default:
                Debug.LogError("Check the number that u pass to ApplyStyle");
                break;
        }
    }

    // Use this for initialization
    void Start () {

        ApplyUiStyle(GameManager.Instance.scoreUpper);
        Debug.Log(GameManager.Instance.scoreUpper);
        
    }
	
	// Update is called once per frame
	void Update () {
        
       

    }
}
