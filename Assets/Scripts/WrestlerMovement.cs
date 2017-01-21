using UnityEngine;
using System.Collections;

public class WrestlerMovement : MonoBehaviour {

    public GameObject WrestlerGroup;
    public float Speed;

    public GameObject Rope1;


    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Add force in all directions on keypress
        if (Input.GetKey(KeyCode.W))
        {
            WrestlerGroup.transform.position =  new Vector3 (WrestlerGroup.transform.position.x, WrestlerGroup.transform.position.y, WrestlerGroup.transform.position.z + Speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            WrestlerGroup.transform.position = new Vector3(WrestlerGroup.transform.position.x - Speed, WrestlerGroup.transform.position.y, WrestlerGroup.transform.position.z);
        }

        if (Input.GetKey(KeyCode.S))
        {
            WrestlerGroup.transform.position = new Vector3(WrestlerGroup.transform.position.x, WrestlerGroup.transform.position.y, WrestlerGroup.transform.position.z - Speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            WrestlerGroup.transform.position = new Vector3(WrestlerGroup.transform.position.x + Speed, WrestlerGroup.transform.position.y, WrestlerGroup.transform.position.z);
        }
    }
}
