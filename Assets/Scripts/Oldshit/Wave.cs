using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {

    public bool button;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {	
        if(button)
        {
            button = false;
            gameObject.transform.position = Vector3.up;
        }
        else
        {
            gameObject.transform.position = Vector3.up * -10;
        }
	}
}
