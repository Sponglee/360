using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPStargetFrameRate : MonoBehaviour {

	private void Aware()
    {
        Application.targetFrameRate = 60;
    }
}
