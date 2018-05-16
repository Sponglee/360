﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

 

    // Use this for initialization
    void Start () {
       
	}
	

    public void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("line") && gameObject.CompareTag("spot"))
        {
            
            GameManager.Instance.currentSpot = gameObject;
            AudioManager.Instance.PlaySound("rotationClick");
            //GameManager.Instance.centerAnim.SetTrigger("tilt");
            //Debug.Log(GameManager.Instance.currentSpot);
        }
        else if (other.CompareTag("line") && gameObject.CompareTag("spawn"))
        {
            GameManager.Instance.currentSpawn = gameObject;
        }
    }
}
