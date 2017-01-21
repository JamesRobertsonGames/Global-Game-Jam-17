using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour {

    public LayerMask water;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Ray ray = new Ray(transform.position + (Vector3.up * 2), Vector3.down);
        RaycastHit hit;

	    if(Physics.Raycast(ray, out hit, 5, water))
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.Euler(hit.normal);
        }
	}
}
