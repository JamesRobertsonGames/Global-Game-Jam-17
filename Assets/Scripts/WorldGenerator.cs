using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Tile ID
// 0 = water
// 1 = rock
// 2 = land

public class Tile
{
    public int ID = 0;
    public GameObject gObject = null;

    public GameObject occupant;
}
public class ReachableTile
{
    public Tile tile;
    public bool move;
    public bool attack;
    public GameObject gObject = null;

    public Vector2 pos;
    public int depth;
}

public class TileWave
{
    public int waveStart;
    public int length;
    public int[] waveLength;
}

public class WorldGenerator : MonoBehaviour {

    public int WIDTH, HEIGHT;
    public int TILESIZE;

    public bool button;
    public bool otherButton;
    public int getX, getY;

    public GameObject waterTile;
    public GameObject rockTile;
    public GameObject landTile;
    public GameObject[] moveTiles;

    public GameObject testObject;
    private Tile[,] tileMap;
    private TileWave[] waveMap;
    private List<ReachableTile> reachableTiles;
    private int waveWidth;

    static WorldGenerator instance;
    public static WorldGenerator GetInstance()
    {
        if (instance == null)
            instance = FindObjectOfType<WorldGenerator>();
        return instance;
    }

    /*
    int array[3][5] // C++
    int array[3,5]  // C#
    */

    // Use this for initialization
    void Start () {
        tileMap = new Tile[WIDTH, HEIGHT];
        reachableTiles = new List<ReachableTile>();

        for (int i = 0; i < WIDTH; i++)
            for(int j = 0; j < HEIGHT; j++)
            {
                tileMap[i, j] = new Tile();
            }

        GenerateMap();
	}

    void GenerateMap()
    {
        int MAXx = (int)(WIDTH * 0.5f);

        // actually create tilemap
        for (int i = 0; i < WIDTH * 0.5f; i++)
        {
            bool t = false;
            for(int j = 0; j < HEIGHT; j++)
            {
                int tileID = 0;

                if(i >= MAXx - 5 && i <= WIDTH - (MAXx - 5) || j > 1 && j < HEIGHT - 1)
                {
                    if (i > MAXx - 5)
                    {
                        if (Random.Range(0, 100) < 10 && !t)
                        {
                            if (Random.Range(0, 100) < 20)
                                t = true;

                            if (j > 0)
                                tileID = 1;
                        }
                    }
                    else if (Random.Range(0, 100) < 10 && j > 0)
                        tileID = 1;
                }
                else if (Random.Range(0, 20 + (j * 10)) < 30)
                    tileID = 1;

                tileMap[i, j].ID = tileID;
            }
        }

        tileMap[MAXx, 0].ID = 0;


        /////////////////////////////////////////////////////////////////////////////////// generate rock mass
        int landX = 0;
        int landY = 0;
        int rand = 50;
        do
        {
            tileMap[landX, landY].ID = 1;
            if (Random.Range(0, 50) < rand)
            {
                if (landY < 3)
                    landY++;

                rand -= 10;
            }
            else if (landY > 0)
            {
                for (int i = 0; i < landY; i++)
                    tileMap[landX, i].ID = 1;

                tileMap[landX, landY + 1].ID = 0;

                rand += 5;
                landX++;
                landY--;
            }
            else
            {
                break;
            }

        } while (true);

        /////////////////////////////////////////////////////////////////////////////////// generate rock split
        landX = MAXx;
        landY = HEIGHT - 1;
        rand = 50;
        do
        {
            tileMap[landX, landY].ID = 1;
            if (Random.Range(0, 40) < rand)
            {
                if (landY != 6)
                    landY--;

                rand -= 10;
            }
            else if (landY < HEIGHT - 1)
            {
                for (int i = landY; i < HEIGHT; i++)
                    tileMap[landX, i].ID = 1;

                tileMap[landX, landY - 1].ID = 0;

                rand += 5;
                landX--;
                landY++;
            }
            else
            {
                break;
            }
        } while (true);


        /////////////////////////////////////////////////////////////////////////////////// generate land mass
        landX = 0;
        landY = HEIGHT - 1;
        rand = 70;
        do
        {
            tileMap[landX, landY].ID = 2;
            if (Random.Range(0, 70) < rand)
            {
                if (landY != 6)
                    landY--;

                rand -= 10;
            }
            else if (landY < HEIGHT - 1)
            {
                for (int i = landY; i < HEIGHT; i++)
                    tileMap[landX, i].ID = 2;

                tileMap[landX, landY - 1].ID = 0;

                rand += 5;
                landX++;
                landY++;
            }
            else
            {
                tileMap[landX, landY - 1].ID = 0;
                tileMap[landX + 1, landY].ID = 0;
                tileMap[landX + 1, landY - 1].ID = 0;
                break;
            }
        } while (true);

        //////////////////////////////////////////////////// fill tiles
        for (int i = 0; i < WIDTH * 0.5f; i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                // final pass
                for (int k = 0; k < 2; k++)
                {
                    int tileIndex = (k == 1 ? (WIDTH - 1) - i : i);
                    tileMap[tileIndex, j].ID = tileMap[i, j].ID;

                    switch (tileMap[i, j].ID)
                    {
                        case 0:
                            tileMap[tileIndex, j].gObject = Instantiate(waterTile, new Vector3(tileIndex * TILESIZE, 0, j * TILESIZE), Quaternion.identity) as GameObject;
                            tileMap[tileIndex, j].gObject.transform.SetParent(transform);
                            break;
                        case 1:
                            tileMap[tileIndex, j].gObject = Instantiate(rockTile, new Vector3(tileIndex * TILESIZE, Random.Range(-10, 10) * 0.1f, j * TILESIZE), Quaternion.identity) as GameObject;
                            tileMap[tileIndex, j].gObject.transform.SetParent(transform);
                            break;
                        case 2:
                            tileMap[tileIndex, j].gObject = Instantiate(landTile, new Vector3(tileIndex * TILESIZE, 0, j * TILESIZE), Quaternion.identity) as GameObject;
                            tileMap[tileIndex, j].gObject.transform.SetParent(transform);
                            break;
                    }
                }
            }
        }

        // get the tiledata for the wave

        // get the 'middle' tile
        int waveStart = (int)(WIDTH * 0.5f);
        int waveEnd = waveStart;
        // search backwards to find the start of the wave
        while (tileMap[waveStart, 0].ID == 0)
        {
            waveStart--;

            if (waveStart < 0)
                break;
        }
        waveStart++;

        // search forwards to find the end of the wave
        while (tileMap[waveEnd, 0].ID == 0)
        {
            waveEnd++;

            if (waveEnd >= WIDTH)
                break;
        }
        waveEnd--;

        waveWidth = (waveEnd - waveStart) + 1;
        waveMap = new TileWave[waveWidth];

        // fuuuuuuuuuuuuuuuuuuuuuuuuuu
        for(int i = 0; i < waveWidth; i++)
        {
            waveMap[i] = new TileWave();
            
            int waveLength = 0;
            while (tileMap[waveStart + i, waveLength].ID == 0)
            {
                waveLength++;
                if (waveLength == 10)
                {
                    break;
                }
            }

            waveMap[i].waveStart = waveStart + i;

            waveMap[i].waveLength = new int[waveLength];
            waveMap[i].length = waveLength;
            for(int j = 0; j < waveLength; j++)
            {
                waveMap[i].waveLength[j] = j;
            }
        }

        for(int i = 0; i < waveWidth; i++)
        {
            for (int j = 0; j < waveMap[i].length; j++)
            {
                //Instantiate(testObject, new Vector3(waveMap[i].waveStart, 0, waveMap[i].waveLength[j]), Quaternion.identity);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if(button)
        {
            button = false;
            SendWave();
        }

        if(otherButton)
        {
            otherButton = false;

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            GenerateMap();
        }
	}

    private bool wave;
    public void ActivateWave(bool b)
    {
        wave = !b;
    }
    public void SendWave()
    {
        StartCoroutine(DoWave());
    }

    public GameObject GetTileObject(Vector3 v)
    {
        int x = (int)(v.x * 0.1f);
        int y = (int)(v.z * 0.1f);

        if(x < WIDTH && x >= 0
        && y < HEIGHT && y >= 0)
        {
            return tileMap[x, y].gObject;
        }

        return null;
    }
    public Tile GetTile(Vector3 v)
    {
        int x = (int)(v.x * 0.1f);
        int y = (int)(v.z * 0.1f);

        if (x < WIDTH && x >= 0
        && y < HEIGHT && y >= 0)
        {
            return tileMap[x, y];
        }

        return null;
    }

    private int teamStorage;
    private Vector2 posStorage;
    public void FindReachableTiles(Vector3 pos, int moveRange, int attackRange, int myTeam)
    {
        Tile startTile = GetTile(pos);

        teamStorage = myTeam;

        if (startTile.ID != 0)
            moveRange = 1;

        int tileX = (int)(startTile.gObject.transform.position.x / TILESIZE);
        int tileY = (int)(startTile.gObject.transform.position.z / TILESIZE);

        posStorage.x = tileX;
        posStorage.y = tileY;
        CheckNeighbors(tileX, tileY, 0, moveRange, attackRange);

        for(int i = 0; i < reachableTiles.Count; i++) // something fuckey here
        {
            if (reachableTiles[i].move)
            {
                reachableTiles[i].gObject = Instantiate(moveTiles[0], GetTileWorldPos(reachableTiles[i].tile.gObject.transform.position) + Vector3.up, Quaternion.identity) as GameObject;
                reachableTiles[i].gObject.transform.SetParent(transform);
            }
            else if(CanAttack())
            {
                reachableTiles[i].gObject = Instantiate(moveTiles[1], GetTileWorldPos(reachableTiles[i].tile.gObject.transform.position) + Vector3.up, Quaternion.identity) as GameObject;
                reachableTiles[i].gObject.transform.SetParent(transform);
            }
        }
    }

    public void ClearReachableTiles()
    {
        for (int i = 0; i < reachableTiles.Count; i++)
        {
            Destroy(reachableTiles[i].gObject);
        }
        reachableTiles.Clear();
    }

    public bool CheckMoveTile(Tile t)
    {
        for (int i = 0; i < reachableTiles.Count; i++)
        {
            if (t == reachableTiles[i].tile && reachableTiles[i].move)
                return true;
        }

        return false;
    }
    
    void CheckNeighbors(int x, int y, int depth, int maxMove, int maxAttack)
    {
        if (depth >= maxMove + maxAttack)
            return;

        depth++;
        
        for (int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if(x + i >= 0 && x + i < WIDTH && y + j >= 0 && y + j < HEIGHT)
                {
                    if (x + i == posStorage.x && y + j == posStorage.y)
                        continue;

                    if (i * i == 1 && j * j == 1)
                        continue;

                    bool sameTeam = false;
                    bool hasEnemy = false;
                    if (tileMap[x + i, y + j].occupant != null)
                    {
                        sameTeam = true;
                        if (tileMap[x + i, y + j].occupant.GetComponent<Ship>().team != teamStorage)
                        {
                            hasEnemy = true;
                            sameTeam = false;
                        }
                    }
                    
                    if (tileMap[x + i, y + j].ID == 0 || !sameTeam)
                    {
                        ReachableTile newT = new ReachableTile();
                        newT.pos = new Vector2(x + i, y + j);
                        newT.tile = tileMap[x + i, y + j];

                        if (!sameTeam)
                        {
                            newT.attack = true;

                            if (depth <= maxMove)
                                newT.move = true;
                        }
                        else
                            continue;

                        bool hasTile = false;
                        for (int k = 0; k < reachableTiles.Count; k++)
                        {
                            if (newT.pos == reachableTiles[k].pos)
                            {
                                if (depth < reachableTiles[k].depth)
                                {
                                    reachableTiles[k] = newT;
                                    CheckNeighbors(x + i, y + j, depth, maxMove, maxAttack);
                                }

                                hasTile = true;
                            }
                        }

                        if (tileMap[x + i, y + j].ID != 0 && !hasEnemy) hasTile = true;

                        if (!hasTile)
                        {
                            newT.depth = depth;
                            reachableTiles.Add(newT);
                            CheckNeighbors(x + i, y + j, depth, maxMove, maxAttack);
                        }
                    }
                }
            }
        }
    }

    public bool CanAttack()
    {
        for (int i = 0; i < reachableTiles.Count; i++)
        {
            if (reachableTiles[i].attack)
                if (reachableTiles[i].tile.occupant)
                    if (reachableTiles[i].tile.occupant.GetComponent<Ship>().team != teamStorage)
                        return true;
        }

        return false;
    }

    public Vector3 GetTileWorldPos(Vector3 v)
    {
        int x = (int)(v.x * 0.1f) * TILESIZE;
        float y = 5;
        int z = (int)(v.z * 0.1f) * TILESIZE;

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(x, 20, z), Vector3.down, out hit, 20))
        {
            y = hit.point.y;
        }

        return new Vector3(x, y, z);
    }

    public void PlaceOnTile(Vector3 pos, GameObject obj)
    {
        GetTile(pos).occupant = obj;
    }
    
    public float waveStrength;
    public float waveSpeed;
    IEnumerator DoWave()
    {
        if (!wave)
        {
            wave = true;
            float timer = 0;

            while (timer < 15)
            {
                timer += Time.deltaTime * waveSpeed;

                for (int i = 0; i < waveWidth; i++)
                    for (int j = 0; j < waveMap[i].length; j++)
                    {
                        Vector2 v = new Vector2(waveMap[i].waveStart, waveMap[i].waveLength[j]);
                        Vector3 pos = tileMap[(int)v.x, (int)v.y].gObject.transform.position;

                        float height = pos.y;
                        if ((int)timer < j + 1 && (int)timer > j - 1)
                            height += Time.deltaTime * waveStrength;
                        else
                            height -= Time.deltaTime * waveStrength;

                        if (height < 0)
                            height = 0;

                        tileMap[(int)v.x, (int)v.y].gObject.transform.position = new Vector3(v.x * TILESIZE, 0, v.y * TILESIZE) + new Vector3(0, height);
                    }
                yield return null;
            }
        }
        
        yield return null;
    }
}
