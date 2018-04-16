using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    private Vector3 finger;
    private Transform myTrans, camTrans;


	// Use this for initialization
	void Start ()
    {
        myTrans = this.transform;
        camTrans = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void LookAtFinger()
    {
        Vector3 tempTouch = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camTrans.position.y-myTrans.position.y);
        finger = Camera.main.ScreenToWorldPoint(tempTouch);


        myTrans.LookAt(finger);
    }
}
