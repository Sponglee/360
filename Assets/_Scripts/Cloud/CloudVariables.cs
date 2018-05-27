using UnityEngine.UI;
using UnityEngine;

public class CloudVariables : MonoBehaviour {

    public static int Highscore{get;set;}
    public static int[] ImportantValues { get; set; }
  

    


private void Awake()
{
    ImportantValues = new int[3];
}


}
