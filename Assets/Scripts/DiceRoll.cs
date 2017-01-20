using UnityEngine;
using System.Collections;

public class DiceRoll : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void Roll()
    {
        Random.Range(1, 6);
    }
}
