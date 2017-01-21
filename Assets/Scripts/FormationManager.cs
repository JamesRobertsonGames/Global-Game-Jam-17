using UnityEngine;
using System.Collections;

public class FormationManager : MonoBehaviour
{

    public GameObject BoatOne, BoatTwo, BoatThree;
    public bool BoatOnePlaced, BoatTwoPlaced, BoatThreePlaced, PlacementRoundOver = false;
    public float WaterHeight;

    RaycastHit hit;

    public Tile SelectedTile;

	// Use this for initialization
	void Start ()
    {
        
	}

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerRoutine()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000000))
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlacePlayerinWater();
            }
        }
    }

    void PlacePlayerinWater()
    {
            if (!BoatOnePlaced)
            {
                BoatOne.transform.position = hit.collider.gameObject.transform.position;
                BoatOne.transform.position = new Vector3(BoatOne.transform.position.x, BoatOne.transform.position.y + WaterHeight, BoatOne.transform.position.z);
                BoatOnePlaced = true;
                return;
            }
            if (!BoatTwoPlaced)
            {
                BoatTwo.transform.position = hit.collider.gameObject.transform.position;
                BoatTwo.transform.position = new Vector3(BoatTwo.transform.position.x, BoatTwo.transform.position.y + WaterHeight, BoatTwo.transform.position.z);
                BoatTwoPlaced = true;
                return;
            }
            if (!BoatThreePlaced)
            {
                BoatThree.transform.position = hit.collider.gameObject.transform.position;
                BoatThree.transform.position = new Vector3(BoatThree.transform.position.x, BoatThree.transform.position.y + WaterHeight, BoatThree.transform.position.z);
                BoatThreePlaced = true;
                return;
            }

        if (BoatThreePlaced)
        {
            PlacementRoundOver = true;
        }
    }
}
