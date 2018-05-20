using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBoardShadow : MonoBehaviour {


    [SerializeField]
    private Vector3 offset;

    private Quaternion rotationFix;

    private void Awake()
    {
        rotationFix = transform.rotation;
        
    }

    // Update is called once per frame
    void LateUpdate () {

        //gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        gameObject.transform.rotation = rotationFix;

        gameObject.transform.position = gameObject.transform.parent.position + offset;

    }
}
