using UnityEngine;
using System.Collections;

public class GameplayCamera : MonoBehaviour
{

    public Vector3 Offset;
    public GameObject Ship;
    public RoundManager Manager;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void PlayerFollow()
    {
        Manager.Camera.transform.position = Ship.transform.position + Offset;
    }

    void ExploreMode()
    {
        // Lookat Enemy
        // Pan
        // Reset
    }
}
