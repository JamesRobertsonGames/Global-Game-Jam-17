using UnityEngine;
using System.Collections;

public class Tile
{
    public int ID = 0;
    public GameObject gObject = null;
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

    public GameObject testObject;
    private Tile[,] tileMap;
    private TileWave[] waveMap;
    private int waveWidth;
    
    /*
    int array[3][5] // C++
    int array[3,5]  // C#
    */

	// Use this for initialization
	void Start () {
        tileMap = new Tile[WIDTH, HEIGHT];
        for(int i = 0; i < WIDTH; i++)
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
                            tileMap[tileIndex, j].gObject = Instantiate(waterTile, new Vector3(tileIndex, 0, j), Quaternion.identity) as GameObject;
                            tileMap[tileIndex, j].gObject.transform.SetParent(transform);
                            break;
                        case 1:
                            tileMap[tileIndex, j].gObject = Instantiate(rockTile, new Vector3(tileIndex, Random.Range(0, 40) * 0.01f, j), Quaternion.identity) as GameObject;
                            tileMap[tileIndex, j].gObject.transform.SetParent(transform);
                            break;
                        case 2:
                            tileMap[tileIndex, j].gObject = Instantiate(landTile, new Vector3(tileIndex, 0, j), Quaternion.identity) as GameObject;
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
            StartCoroutine(DoWave());
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

    public float waveStrength;
    public float waveSpeed;
    IEnumerator DoWave()
    {
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

                    tileMap[(int)v.x, (int)v.y].gObject.transform.position = new Vector3(v.x, 0, v.y) + new Vector3(0, height);
                }
            yield return null;
        }        

        yield return null;
    }
}
