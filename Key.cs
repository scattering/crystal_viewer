using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Key : MonoBehaviour {
    public Text atomDisplay;
    public Text KeyA;
    public Text KeyB;
    public Text KeyX;
    // Use this for initialization
    void Start () {
       
        KeyA.text = string.Concat(atomDisplay.text[3],atomDisplay.text[4]," (A)");
        KeyB.text = string.Concat(atomDisplay.text[1], atomDisplay.text[2]," (B)");
        KeyX.text = string.Concat(atomDisplay.text[5], " (X)");
        
    }
	
}
