using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GroundGeneration : MonoBehaviour
{

    public GameObject EndLevel;
    string END_LEVEL_TAG = "EndLevel";


    public Tilemap tileMap;
    public TileBase tileBase;

    public int width;
    public int height;

    private float seed = 0.11f;

    private int[,] mapGround;
    private int[,] mapPlatform;

    private LevelSelection levelManager;

    // Start is called before the first frame update
    private void Start()
    {
        // If we have a levelManager, we choose a random level.
        levelManager = GameObject.FindObjectOfType<LevelSelection>();
        if (levelManager != null)
        {
            levelManager.ChooseLevel();
            tileBase = levelManager.GetTileBase();
        }
        
        EndLevel = GameObject.FindGameObjectWithTag(END_LEVEL_TAG);
        // tileMap = GetComponent<Tilemap>();
        mapGround = GenerateArray(width, height, empty: true);
        mapPlatform = GenerateArray(width, height, empty: true);

        seed = Random.Range(0.1f, 0.4f);
        mapGround = RandomWalkTopSmoothed(mapGround, seed, 3);
        
        // TODO Fix error RandomPlatform Array out of index
        try
        {
            mapPlatform = RandomPlatform(mapPlatform ,mapGround, seed);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        RenderMap(mapGround, tileMap, tileBase);
        UpdateMapPlatform(mapPlatform, tileMap, tileBase, height);
        Debug.Log("FinalWalk");
        FinalWalkPlatform(tileMap, tileBase, mapGround, EndLevel);

    }

    private static void FinalWalkPlatform(Tilemap tileMap, TileBase tileBase, int[,] mapGround, GameObject EndLevel) {

        int WALKLENGHTPLATFORM = 50;

        int finalPosition = mapGround.GetUpperBound(0);
        for (var x = finalPosition; x < finalPosition + WALKLENGHTPLATFORM; x++)
        {
            tileMap.SetTile(new Vector3Int(x, 0, 0), tileBase);
            if (x == ((int) WALKLENGHTPLATFORM/2 + finalPosition))
            {
                EndLevel.transform.position = new Vector3(x, 0, 0);
            }
        }
    }

    private static int[,] RandomPlatform(int[,] mapPlatform, int[,] mapGround, float seed)
    {
        // Iterate in x axis.
        for (var x = 0; x < mapPlatform.GetUpperBound(0); x++)
        {
            // See if it's possible create a platform in the x-axis
            var platformExist = false;
            for (var y = 0; y < mapPlatform.GetUpperBound(1); y++)
            {
                if (mapPlatform[x, y] == 1) platformExist = true;
            }

            // Throw a money if 1 and there isn't a platform, then create a platform.
            float create = Random.Range(0f, 1.0f);
            if (!platformExist && (create < 0.2f))
            {
                // Choose length of platform
                int platformLenght = Random.Range(2, 4);
                // Choose the height of platform
                int platformHeight = Random.Range(0, mapPlatform.GetUpperBound(1));
                // Put the platform in the map
                for (int x1 = x; x1 < x + platformLenght; x1++)
                {
                    mapPlatform[x1, platformHeight] = 1;
                }
            }
        }


        return mapPlatform;
    }

    private static void UpdateMapPlatform(int[,] map, Tilemap tilemap, TileBase tile, int groundHeight) //Takes in our map and tilemap, setting null tiles where needed
    {
        for (var x = 0; x < map.GetUpperBound(0); x++)
        {
            for (var y = 0; y < map.GetUpperBound(1); y++)
            {
                //We are only going to update the map, rather than rendering again
                //This is because it uses less resources to update tiles to null
                //As opposed to re-drawing every single tile (and collision data)
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y+groundHeight, 0), tile);
                }
            }
        }
    }


    private static int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x <  map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }
        return map;
    }

    private static void RenderMap(int[,] map, Tilemap tilemap, TileBase tile)
    {
        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < map.GetUpperBound(0) ; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }

    public static void UpdateMap(int[,] map, Tilemap tilemap, TileBase tile) //Takes in our map and tilemap, setting null tiles where needed
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                //We are only going to update the map, rather than rendering again
                //This is because it uses less resources to update tiles to null
                //As opposed to re-drawing every single tile (and collision data)
                if (map[x, y] == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }


    private static int[,] RandomWalkTopSmoothed(int[,] map, float seed, int minSectionWidth)
    {
        //Seed our random
        System.Random rand = new System.Random(seed.GetHashCode());

        //Determine the start position
        int lastHeight = Random.Range(0, map.GetUpperBound(1));

        //Used to determine which direction to go
        int nextMove = 0;
        //Used to keep track of the current sections width
        int sectionWidth = 0;

        //Work through the array width
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            //Determine the next move
            nextMove = rand.Next(2);

            //Only change the height if we have used the current height more than the minimum required section width
            if (nextMove == 0 && lastHeight > 0 && sectionWidth > minSectionWidth)
            {
                lastHeight--;
                sectionWidth = 0;
            }
            else if (nextMove == 1 && lastHeight < map.GetUpperBound(1) && sectionWidth > minSectionWidth)
            {
                lastHeight++;
                sectionWidth = 0;
            }
            //Increment the section width
            sectionWidth++;

            //Work our way from the height down to 0
            for (int y = lastHeight; y >= 0; y--)
            {
                map[x, y] = 1;
                // if (Random.Range(0, 1) < 0.2f) map[x, y + 2] = 1;
            }
        }

        //Return the modified map
        return map;
    }
}

