using UnityEngine;
using System.Collections;

public class RotateRaycast_Script : MonoBehaviour
{
   
    public float range = 5.0f;
    public float angle = 0.0f;

    private Vector2 startPoint;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       // Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        

      
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

     
        Vector3 direction = screenPos - gameObject.transform.position;

        Physics2D.Raycast(gameObject.transform.parent.position, direction * range); // Shot ray.

        
        Debug.DrawRay(gameObject.transform.parent.position, direction*range, Color.red);

    }

    
}