using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Quaternion touchRotation;


    private float swipeResistanceX = 25f;
    private float swipeResistanceY = 25f;
    private float rotResistance = 0f;



    public bool SwipeC = true;
    public bool swipeValue = false;

    float timer = 0f;
    private bool startTouch = false;
    public Text SwipeDirect;


    public Transform wheel;




    // Use this for initialization
    void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {


           Direction = SwipeDirection.None;
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
            touchRotation = wheel.rotation;
            timer = Time.deltaTime;
            startTouch = true;
        }

        if (Input.GetMouseButtonUp(0))
            {
                Vector2 deltaSwipe = touchPosition - Input.mousePosition;
                float deltaRot = touchRotation.z - wheel.rotation.z;

            if (Mathf.Abs(deltaRot) > rotResistance)
                {
                    if (!swipeValue)
                    {
                        Direction |= (deltaRot< 0) ? SwipeDirection.Left : SwipeDirection.Right;
                    }
                    else
                        Direction |= (deltaRot < 0) ? SwipeDirection.Right : SwipeDirection.Left;
            }
            else
                {
                
                    Direction |= SwipeDirection.None;
                }
           
            //if (Mathf.Abs(deltaSwipe.y) > swipeResistanceY)
            //{
            //    if (!swipeValue || Input.mousePosition.x >= 0.5f)
            //    {
            //        Direction |= (deltaSwipe.y < 0) ? SwipeDirection.Left : SwipeDirection.Right;
            //    }
            //    else
            //        Direction |= (deltaSwipe.y < 0) ? SwipeDirection.Right : SwipeDirection.Left;
            //}
            //else
            //{
                     
            //            Direction |= SwipeDirection.None;
            //}
            //Debug.Log(Direction);
        }
       
    }
    public bool IsSwiping (SwipeDirection dir)
    {
      
        return (Direction & dir) == dir;
    }


    public void SwipeChange()
    {
        string swipeArrow;
        if (swipeValue == true)
            swipeArrow = "<--";
        else
            swipeArrow = "-->";
        swipeValue = !swipeValue;
        SwipeDirect.text = string.Format("Swipe Direction {0}", swipeArrow);

    }
}
