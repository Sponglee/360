using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : Singleton<Rotator> {


    public bool DoRotate = false;

    public float angle = 0f;
    private int spot;

    private float duration = 0.2f;
    private Vector3 axis = Vector3.forward;





    private float differ;


    Quaternion from;
    Quaternion to;
    // Maximum turn rate in degrees per second.
    public float turningRate = 7f;
    private Quaternion _targetRotation = Quaternion.identity;

    // Update is called once per frame
    void FixedUpdate ()
    {
		if (DoRotate)
        {

           

            
                
           
            
        }
	}

    public void DoRotation(float rotAngle, int rotSpot)
    {
        spot = rotSpot;
        angle = rotAngle;
        GameManager.Instance.RotationProgress = true;
        _targetRotation *= Quaternion.Euler(0,0,angle);

        DoRotate = true;
    }
   
}
