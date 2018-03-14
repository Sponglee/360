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



public class SwipeManager : Singleton<SwipeManager> {


    public SwipeDirection Direction { set; get; }


    private Vector3 touchPosition;

    private float swipeResistanceX = 50f;
    private float swipeResistanceY = 100f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


            Direction = SwipeDirection.None;
	        if (Input.GetMouseButtonDown(0))
            {
                touchPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 deltaSwipe = touchPosition - Input.mousePosition;

                if(Mathf.Abs(deltaSwipe.x) > swipeResistanceX)
                {
                    Direction |= (deltaSwipe.x < 0) ? SwipeDirection.Right : SwipeDirection.Left;
                }
                if (Mathf.Abs(deltaSwipe.y) > swipeResistanceY)
                {
                    // SWIPE Y AXIS
                    Direction |= (deltaSwipe.y < 0) ? SwipeDirection.Up : SwipeDirection.Down;
                }
            }
	}
    public bool IsSwiping (SwipeDirection dir)
    {
        return (Direction & dir) == dir;
    }
}
