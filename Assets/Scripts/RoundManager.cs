using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour
{

    public FormationManager BoatsFormationsPlayerOne;
    public FormationManager BoatsFormationsPlayerTwo;

    public SimpleSailing PlayerMovement;

    public GameObject Camera;
    public GameObject SetupCameraPositionPlayerOne;
    public GameObject SetupCameraPositionPlayerTwo;

    bool RoundComplete = false;
    int hack = 0;

    public bool PlayModeEnabled = false;

    // Use this for initialization
    void Start ()
    {
  
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!PlayModeEnabled)
        {
            CreateFormation();
        }

        if (PlayModeEnabled)
        {
            hack++;
            if (hack > 30)
                if (!RoundComplete)
                    RoundComplete = PlayerMovement.StartRound(BoatsFormationsPlayerOne, Camera);
            
        }
    }

    void CreateFormation()
    {
        if (!BoatsFormationsPlayerOne.PlacementRoundOver)
        {
            BoatsFormationsPlayerOne.PlayerRoutine();
            Camera.transform.position = SetupCameraPositionPlayerOne.transform.position;
            if (BoatsFormationsPlayerOne.BoatThreePlaced)
            {
                BoatsFormationsPlayerOne.PlacementRoundOver = true;
            }
            return;
        }

        if (!BoatsFormationsPlayerTwo.PlacementRoundOver)
        {
            BoatsFormationsPlayerTwo.PlayerRoutine();
            Camera.transform.position = SetupCameraPositionPlayerTwo.transform.position;
            if (BoatsFormationsPlayerTwo.BoatThreePlaced)
            {
                BoatsFormationsPlayerTwo.PlacementRoundOver = true;
                PlayModeEnabled = true;
            }
            return;
        }
    }
}
