using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBoardShadow : MonoBehaviour {


    [SerializeField]
    private Vector3 offset;
	
	// Update is called once per frame
	void Update () {

        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        gameObject.transform.position = gameObject.transform.parent.position + offset;
    }
}
