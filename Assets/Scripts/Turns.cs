using UnityEngine;
using System.Collections;

public class Turns : MonoBehaviour
{
    public int PlayerTurnSelect;

    public GameObject PlayerOne;
    public GameObject PlayerTwo;
    public GameObject PlayerThree;
    public GameObject PlayerFour;


    // Use this for initialization
    void Start ()
    {

	    
	}
	
	// Update is called once per frame
	void Update ()
    {

	
	}

    void TurnSelect ()
    {
        switch(PlayerTurnSelect)
        {
            case 1:
                // Temp - put player rounds in here
                TurnLogic(1);
                break;
        }
    }

    GameObject GetPlayerObject(int PlayerIndex)
    {
        switch(PlayerIndex)
        {
            case 1:
                return PlayerOne;
            case 2:
                return PlayerTwo;
            case 3:
                return PlayerThree;
            case 4:
                return PlayerFour;
        }
        return PlayerOne;
    }

    void TurnLogic (int Player)
    {
        GameObject PlayerLogicBoard = GetPlayerObject(Player);
        
    }
}
