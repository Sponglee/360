using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstControl : MonoBehaviour {

    public ParticleSystem particle;

    [SerializeField]
    private float turnDelay = 4f;

    private float turnTimer;

    private void Start()
    {

        turnTimer = Time.deltaTime + turnDelay;
    }
    // Update is called once per frame
    void Update () {
        if (Time.deltaTime > turnTimer)
        {
            Destroy(gameObject);
        }
    }
}

