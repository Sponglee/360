using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/** Swipe direction */
public enum SwipeDirection
{
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,
}



public class SwipeManager : Singleton<SwipeManager>
{


    public SwipeDirection Direction { set; get; }


    private Vector3 touchPosition;
    private Vector3 screenTouch;
    private Vector3 endTouch;


    private float swipeResistanceX = 25f;
    private float swipeResistanceY = 25f;

    public bool SwipeC = true;
    public bool swipeValue = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        Direction = SwipeDirection.None;
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
            screenTouch = Camera.main.ScreenToViewportPoint(touchPosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 deltaSwipe = touchPosition - Input.mousePosition;

            endTouch = Camera.main.ScreenToViewportPoint(Input.mousePosition);



            if (Mathf.Abs(deltaSwipe.x) > swipeResistanceX)
            {
                if (screenTouch.y >= 0.5)
                {
                    Direction |= (deltaSwipe.x < 0) ? SwipeDirection.Right : SwipeDirection.Left;
                }
                else
                    Direction |= (deltaSwipe.x < 0) ? SwipeDirection.Left : SwipeDirection.Right;
            }
            else if (Mathf.Abs(deltaSwipe.y) > swipeResistanceY)
            {
                if (screenTouch.x >= 0.5)
                {
                    Direction |= (deltaSwipe.y < 0) ? SwipeDirection.Left : SwipeDirection.Right;
                }
                else
                    Direction |= (deltaSwipe.y < 0) ? SwipeDirection.Right : SwipeDirection.Left;
            }
            else
            {

                Direction |= SwipeDirection.None;
            }
            Debug.Log(Direction);
        }

    }
    public bool IsSwiping(SwipeDirection dir)
    {

        return (Direction & dir) == dir;
    }


    public void SwipeChange()
    {
        swipeValue = !swipeValue;

    }
}