using UnityEngine;
using System.Collections;

public class Tile
{
    public int ID = 0;
    public GameObject gObject = null;
}

public class WorldGenerator : MonoBehaviour {

    public int WIDTH, HEIGHT;
    public int TILESIZE;

    public bool button;
    public int getX, getY;

    public GameObject waterTile;
    public GameObject rockTile;

    public GameObject posObject;
    private Tile[,] tileMap;
    //private int[,] tileMap;

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
        //tileMap = new int[WIDTH, HEIGHT];

        GenerateMap();
	}

    void GenerateMap()
    {
        for(int i = 0; i < WIDTH * 0.5f; i++)
        {
            for(int j = 0; j < HEIGHT; j++)
            {
                int tileID = 0;

                if(i >= 10 && i <= 20 || j > 2 && j < 7)
                {
                    if (Random.Range(0, 100) < 2)
                        tileID = 1;
                }
                else if (Random.Range(0, 100) < 50)
                    tileID = 1;


                for (int k = 0; k < 2; k++)
                {
                    int tileIndex = (k == 1 ? (WIDTH - 1) - i : i);

                    tileMap[tileIndex, j].ID = tileID;
                    //tileMap[tileIndex, j] = tileID;
                    //
                    switch (tileID)
                    {
                        case 0:
                            //Instantiate(waterTile, new Vector3(tileIndex, 0, j), Quaternion.identity);
                            tileMap[tileIndex, j].gObject = Instantiate(waterTile, new Vector3(tileIndex, 0, j), Quaternion.identity) as GameObject;
                            tileMap[tileIndex, j].gObject.transform.SetParent(transform);
                            break;
                        case 1:
                            //Instantiate(rockTile, new Vector3(tileIndex, 0, j), Quaternion.identity);
                            tileMap[tileIndex, j].gObject = Instantiate(rockTile, new Vector3(tileIndex, 0, j), Quaternion.identity) as GameObject;
                            tileMap[tileIndex, j].gObject.transform.SetParent(transform);
                            break;
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if(button)
        {
            button = false;
            posObject.transform.position = new Vector3(getX * TILESIZE, 0, getY * TILESIZE);
        }
	}
}
