using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RoundManager : MonoBehaviour
{
    public FormationManager playerOne;
    public FormationManager playerTwo;

    public GameObject camera;
    private Vector3 cameraDefaultPos;

    public Crosshair cursor;
    private bool playing;

    static RoundManager instance;
    public static RoundManager GetInstance()
    {
        if (instance == null)
            instance = FindObjectOfType<RoundManager>();
        return instance;
    }

    // Use this for initialization
    void Start ()
    {
        cameraDefaultPos = camera.transform.position;
        playing = true;
    }

    private float turnTimer;
    private bool wave;
	// Update is called once per frame
	void Update ()
    {
        /*
        if (!PlayModeEnabled)
        {
            CreateFormation();
        }
        
        if (PlayModeEnabled)
        {
            hack++;
            if (hack > 30)
                if (!RoundComplete)
                    RoundComplete = PlayerMovement.StartRound(playerOne, camera);
            
        }*/

        if(!wave)
        {
            cursor.gameObject.SetActive(true);
            wave = PlayerPhase();
        }
        else
        {
            cursor.gameObject.SetActive(false);
            turnTimer -= Time.deltaTime;

            if(turnTimer < 6)
                WorldGenerator.GetInstance().SendWave();

            if(turnTimer < 0)
            {
                playerOne.Reset();
                playerTwo.Reset();
                wave = false;
                WorldGenerator.GetInstance().ActivateWave(false);
            }
        }

        TileSelection();
    }

    void TileSelection()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500))
        {
            int tileID = WorldGenerator.GetInstance().GetTile(hit.collider.transform.position).ID;
            Vector3 tilePos = WorldGenerator.GetInstance().GetTileWorldPos(hit.collider.transform.position);

            cursor.transform.position = tilePos;
            cursor.SetActive(tileID == 0 || IsCurrentTileOccupied());
        }
    }
    
    public bool IsCurrentTileOccupied()
    {
        if (WorldGenerator.GetInstance().GetTile(cursor.transform.position).occupant == null)
            return false;

        return true;
    }

    public Tile GetCurrentTile()
    {
        return WorldGenerator.GetInstance().GetTile(cursor.transform.position);
    }
    /*
    void CreateFormation()
    {
        if (!playerOne.PlacementRoundOver)
        {
            playerOne.PlayerRoutine();
            camera.transform.position = playerOne.transform.position;
            if (playerOne.BoatThreePlaced)
            {
                playerOne.PlacementRoundOver = true;
            }
            return;
        }

        if (!playerTwo.PlacementRoundOver)
        {
            playerTwo.PlayerRoutine();
            camera.transform.position = playerTwo.transform.position;
            if (playerTwo.BoatThreePlaced)
            {
                playerTwo.PlacementRoundOver = true;
                PlayModeEnabled = true;
            }
            return;
        }
    }*/

    private bool oneMove, twoMove;
    private FormationManager currentPlayer;
    bool PlayerPhase()
    {
        playerOne.Check();
        playerTwo.Check();

        if (!playerOne.CompletedTurn())
        {
            if(!oneMove)
            {
                if (!playerOne.firstTurn)
                    playerOne.UpdateCamera();

                currentPlayer = playerOne;
                playerOne.Wake();
                camera.transform.DOMove(playerOne.transform.position, 3);
                oneMove = true;
            }
            playerOne.TakeTurn();

            return false;
        }

        if (!playerTwo.CompletedTurn())
        {
            if(!twoMove)
            {
                if (!playerTwo.firstTurn)
                    playerTwo.UpdateCamera();

                currentPlayer = playerTwo;
                playerTwo.Wake();
                camera.transform.DOMove(playerTwo.transform.position, 3);
                twoMove = true;
            }
            playerTwo.TakeTurn();

            return false;
        }

        currentPlayer = null;
        oneMove = twoMove = false;
        camera.transform.DOMove(cameraDefaultPos, 3);

        WorldGenerator.GetInstance().ActivateWave(true);
        turnTimer = 8;
        return true;
    }

    public void ResetCamera(int i)
    {
        switch(i)
        {
            case 0:
                camera.transform.DOMove(cameraDefaultPos, 3);
                break;
            case 1:
                camera.transform.DOMove(playerOne.transform.position, 3);
                break;
            case 2:
                camera.transform.DOMove(playerTwo.transform.position, 3);
                break;
        }
    }

    public void LostGame(int playerID)
    {
        playing = false;
    }

    public void SkipCurrentMovement()
    {
        if (!currentPlayer)
            return;

        currentPlayer.SkipMove();
    }
}
