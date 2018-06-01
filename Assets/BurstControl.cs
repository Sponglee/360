using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstControl : MonoBehaviour {

    public ParticleSystem particle;

    [SerializeField]
    private float turnDelay = 4f;

    

 
    // Update is called once per frame
    void Update () {
        turnDelay -= Time.deltaTime;

        if (turnDelay <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}

