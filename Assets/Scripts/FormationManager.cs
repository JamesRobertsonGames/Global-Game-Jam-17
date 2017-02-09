using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

public class FormationManager : MonoBehaviour
{

    [Range(1, 10)]
    public int shipcount;

    /*
    public GameObject BoatOne, BoatTwo, BoatThree;
    public bool BoatOnePlaced, BoatTwoPlaced, BoatThreePlaced, PlacementRoundOver = false;
    public float WaterHeight;
    bool firstTime = true;

    RaycastHit hit;

    Color MaterialStorage;
    GameObject PastTrace = null;

    public Tile SelectedTile;


    public void HoverRoutine()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000000))
        {
            if (PastTrace != hit.collider.gameObject)
            {
                if (firstTime)
                {
                    MaterialStorage = hit.collider.gameObject.GetComponent<Renderer>().material.color;
                    PastTrace = hit.collider.gameObject;
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                    firstTime = false;
                }
                else
                {
                    PastTrace.GetComponent<Renderer>().material.color = MaterialStorage;
                    Debug.Log("CHANGE");
                    firstTime = true;
                }

            }
        }
        else
        {
            if (!firstTime)
            {
                PastTrace.GetComponent<Renderer>().material.color = MaterialStorage;
                Debug.Log("CHANGE");
                firstTime = true;
            }
        }
    }

    public void PlayerRoutine()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000000))
        { 
            //HoverRoutine();

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
    */
    // MY STUFF
    public List<Ship> activeShips;
    public bool firstTurn;
    public Ship[] boats;
    public int playerID;

    // Use this for initialization
    void Start()
    {
        firstTurn = true;
        activeShips = new List<Ship>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool CompletedTurn()
    {
        if(firstTurn)
        {
            if (activeShips.Count < shipcount)
                return false;
            else
            {
                firstTurn = false;
                return true;
            }
        }
        else
        {
            for (int i = 0; i < activeShips.Count; i++)
            {
                if (!activeShips[i].FinishedTurn())
                    return false;
            }
        }

        return true;
    }

    Ship currentShip;
    Ship enemyShip;
    bool attackPhase;
    bool attacking;
    float attackTimer;
    public void TakeTurn()
    {
        if (firstTurn)
        {
            if (Input.GetMouseButtonDown(0) && RoundManager.GetInstance().cursor.active)
            {
                if (!RoundManager.GetInstance().IsCurrentTileOccupied())
                {
                    Ship newBoat = Instantiate(boats[Random.Range(0, 2) + 1], RoundManager.GetInstance().cursor.transform.position, Quaternion.Euler(0, (playerID == 1 ? 90 : 270), 0)) as Ship;
                    newBoat.team = playerID;
                    RoundManager.GetInstance().GetCurrentTile().occupant = newBoat.gameObject;
                    activeShips.Add(newBoat);
                }
            }
        }
        else
        {
            float h = Input.GetAxis("Horizontal");

            if (h != 0)
            {
                transform.position += new Vector3(h * 50 * Time.deltaTime, 0);
                RoundManager.GetInstance().camera.transform.position = transform.position;
            }

            if(activeShips.Count == 0)
                RoundManager.GetInstance().LostGame(playerID);

            if(attacking)
            {
                if (attackTimer == 3)
                {
                    currentShip.Select(false);
                    Camera.main.transform.DOMove(((currentShip.transform.position + enemyShip.transform.position) * 0.5f) - Camera.main.transform.forward * 15, 1);
                    currentShip.Attack(enemyShip);
                }
                attackTimer -= Time.deltaTime;
                

                if(attackTimer < 0)
                {
                    RoundManager.GetInstance().ResetCamera(playerID);
                    attacking = false;
                    currentShip.EndTurn();
                    currentShip = null;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if(currentShip)
                {
                    if(attackPhase)
                    {
                        if (RoundManager.GetInstance().IsCurrentTileOccupied())
                        {
                            GameObject o = RoundManager.GetInstance().GetCurrentTile().occupant;

                            if (o.GetComponent<Ship>().team != currentShip.team)
                            {
                                enemyShip = o.GetComponent<Ship>();
                                attackPhase = false;
                                attacking = true;
                                attackTimer = 3;
                            }
                        }
                    }
                    else
                    {
                        if (!RoundManager.GetInstance().IsCurrentTileOccupied() && RoundManager.GetInstance().cursor.active)
                        {
                            if (WorldGenerator.GetInstance().CheckMoveTile(RoundManager.GetInstance().GetCurrentTile()))
                            {
                                WorldGenerator.GetInstance().GetTile(currentShip.transform.position).occupant = null;
                                currentShip.transform.position = RoundManager.GetInstance().cursor.transform.position;
                                RoundManager.GetInstance().GetCurrentTile().occupant = currentShip.gameObject;

                                currentShip.ShowAttackRange();
                                if (WorldGenerator.GetInstance().CanAttack())
                                {
                                    attackPhase = true;
                                }
                                else
                                {
                                    currentShip.EndTurn();
                                    currentShip.Select(false);
                                    currentShip = null;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (RoundManager.GetInstance().IsCurrentTileOccupied())
                    {
                        GameObject o = RoundManager.GetInstance().GetCurrentTile().occupant;

                        for (int i = 0; i < activeShips.Count; i++)
                        {
                            if (activeShips[i].FinishedTurn())
                                continue;

                            if (o == activeShips[i].gameObject)
                            {
                                currentShip = activeShips[i];
                                currentShip.Select(true);
                            }                      
                        }
                    }   //////////////////////// EWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
                }   //////////////////////////// EWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
            }   //////////////////////////////// EWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
        }   //////////////////////////////////// EWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW  
    }   //////////////////////////////////////// EWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW

    public void Reset()
    {
        for(int i = 0; i < activeShips.Count; i++)
        {
            if(!activeShips[i].killFlag)
                activeShips[i].Restart();
        }
    }

    public void Wake()
    {
        for (int i = 0; i < activeShips.Count; i++)
        {
            if (!activeShips[i].killFlag)
                activeShips[i].Alert();
        }
    }

    public void Check()
    {
        for (int i = 0; i < activeShips.Count; i++)
        {
            if(activeShips[i].killFlag)
            {
                WorldGenerator.GetInstance().GetTile(activeShips[i].transform.position).occupant = null;
                //activeShips.RemoveAt(i);
                activeShips[i].Disable();
            }
        }
    }

    public void UpdateCamera()
    {
        float x = 0;
        for (int i = 0; i < activeShips.Count; i++)
        {
            x += activeShips[i].transform.position.x;
        }
        x /= activeShips.Count;

        Vector3 pos = transform.position;
        transform.position = new Vector3(x, pos.y, pos.z);
    }

    public void SkipMove()
    {
        if (!currentShip)
            return;

        if(attackPhase)
        {
            attacking = false;
            currentShip.EndTurn();
            currentShip.Select(false);
            currentShip = null;

            attackPhase = false;
        }
        else
        {
            currentShip.ShowAttackRange();
            if (WorldGenerator.GetInstance().CanAttack())
            {
                attackPhase = true;
            }
            else
            {
                currentShip.EndTurn();
                currentShip.Select(false);
                currentShip = null;
            }
        }
    }
}
