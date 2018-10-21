using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** Swipe direction */
public enum MenuSwipeDirection
{
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,
}

public class MenuSwiper : MonoBehaviour {

    public MenuSwipeDirection Direction { set; get; }


    private Vector3 touchPosition;

    private float swipeResistanceX = 125f;
    private float swipeResistanceY = 125f;

    public bool SwipeC = true;
    public bool swipeValue = false;

    public Button[] buttons;
    public int currentTable = 0;
    //public FunctionHandler funcHandler;

    // Update is called once per frame
    void Update () {


        //SwipeManager part

        Direction = MenuSwipeDirection.None;
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 deltaSwipe = touchPosition - Input.mousePosition;

            if (Mathf.Abs(deltaSwipe.x) > swipeResistanceX)
            {
                if (!swipeValue)
                {
                    Direction |= (deltaSwipe.x < 0) ? MenuSwipeDirection.Left : MenuSwipeDirection.Right;
                }
                else
                    Direction |= (deltaSwipe.x < 0) ? MenuSwipeDirection.Right : MenuSwipeDirection.Left;
            }
            else
            {

                Direction |= MenuSwipeDirection.None;
            }


            if (Mathf.Abs(deltaSwipe.y) > swipeResistanceY)
            {
                // SWIPE Y AXIS
                Direction |= (deltaSwipe.y < 0) ? MenuSwipeDirection.Up : MenuSwipeDirection.Down;
            }
            else
            {

                Direction |= MenuSwipeDirection.None;
            }

        }


        //Cycle through tables
        if (IsSwiping(MenuSwipeDirection.Left) && FunctionHandler.Instance.leaderBoardOpen)
        {
            currentTable += 1;
            if (currentTable > 2)
                currentTable = 0;
            //funcHandler.PickTable(currentTable);
            buttons[currentTable].onClick.Invoke();
            StartCoroutine(StopLeader());
        }
        //roll to the left
        else if (IsSwiping(MenuSwipeDirection.Right) && FunctionHandler.Instance.leaderBoardOpen)
        {
            currentTable -= 1;
            if (currentTable < 0)
                currentTable = 2;
            //funcHandler.PickTable(currentTable);
            buttons[currentTable].onClick.Invoke();
            StartCoroutine(StopLeader(true));
        }
    }

    //reference for menu container
    public GameObject menuCont;

    public IEnumerator StopLeader(bool left=false)
    {
        
        float timeOfTravel = 0.5f; //time after object reach a target place 
        float currentTime = 0; // actual floting time 
        float normalizedValue;


        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 
                                                          //Debug.Log("runnin +  " + timeOfTravel + "  :  " + currentTime);
                                                          
            if(left)
                menuCont.transform.position = Vector3.Lerp(menuCont.transform.position, menuCont.transform.position - new Vector3(50, 0), normalizedValue);
            else
                menuCont.transform.position = Vector3.Lerp(menuCont.transform.position, menuCont.transform.position + new Vector3(50,0), normalizedValue);
            yield return null;
        }

        menuCont.transform.position = menuCont.transform.parent.position;
    }


    public bool IsSwiping(MenuSwipeDirection dir)
    {

        return (Direction & dir) == dir;
    }


    public void SwipeChange()
    {
        swipeValue = !swipeValue;

    }
}








  
