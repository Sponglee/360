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


         
        if (Input.GetMouseButtonDown(0))
        {

        }

        if (Input.GetMouseButton(0))
            {
               // Vector2 deltaSwipe = touchPosition - Input.mousePosition;
                float deltaRot = Mathf.Abs(wheel.rotation.z*Mathf.Rad2Deg) - Mathf.Abs(touchRotation.z * Mathf.Rad2Deg);

            //if (Mathf.Abs(deltaRot) > rotResistance)
            //    {
                    
                        Direction |= (deltaRot < 0) ? SwipeDirection.Right : SwipeDirection.Left;
                //}

           // Debug.Log("wheel rotation " + touchRotation.z*Mathf.Rad2Deg + "wheel current " + wheel.rotation.z*Mathf.Rad2Deg + "dir " + Direction);
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
