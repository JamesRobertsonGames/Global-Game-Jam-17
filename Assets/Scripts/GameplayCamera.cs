using UnityEngine;
using System.Collections;

public class GameplayCamera : MonoBehaviour
{

    public Vector3 Offset;

    public int PlayerTurn = 0;
    public GameObject ShipOne_One;
    public GameObject ShipOne_Two;

    public RoundManager Manager;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerTurn = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerTurn = 2;
        }

        PlayerFollow();


    }

    void PlayerFollow()
    {
        if (PlayerTurn == 1)
        {
            Manager.camera.transform.position = ShipOne_One.transform.position + Offset;
        }
        if (PlayerTurn == 2)
        {
            Manager.camera.transform.position = ShipOne_Two.transform.position + Offset;
        }
    }

    void ExploreMode()
    {
        // Lookat Enemy
        // Pan
        // Reset
    }
}
