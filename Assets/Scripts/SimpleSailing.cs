using UnityEngine;
using System.Collections;

public class SimpleSailing : MonoBehaviour
{

    RaycastHit hit;
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    void CenterCameraOnMovementObject(FormationManager Player, GameObject Camera)
    {
        Camera.transform.position = new Vector3(Player.BoatOne.transform.position.x, Player.BoatOne.transform.position.y + 5, Player.BoatOne.transform.position.z - 4);
    }

    public bool StartRound(FormationManager Player, GameObject Camera)
    {
        Player.HoverRoutine();
        CenterCameraOnMovementObject(Player, Camera);

        if (Input.GetMouseButtonDown(0))
        {
            MovetoTile(Player);
            return true;
        }
        return false;
    }

    void MovetoTile(FormationManager Player)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000000))
        {
            Player.BoatOne.transform.position = hit.collider.gameObject.transform.position;
            Player.BoatOne.transform.position = new Vector3(Player.BoatOne.transform.position.x, Player.BoatOne.transform.position.y + 0.4f, Player.BoatOne.transform.position.z);
        }
    }
}
