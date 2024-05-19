using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomataMapGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public string seed;
    public bool useRandom;

    [Range(0, 100)]
    public int randomFillPercent;

    public int smoothinNeighbour;
    public bool invert;
    int[,] map;
    GameObject[,] cubes;

    private void Start()
    {
        map = new int[width, height];
        cubes = new GameObject[width, height];
        SpawnCubes();
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SmoothMap();
            UpdateCubeStatus();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GenerateMap();
            SmoothMap();
            UpdateCubeStatus();
        }
    }

    private void SpawnCubes()
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                cubes[w, h] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubes[w, h].transform.position = new Vector3(w, h, 0);
            }
        }
    }

    private void UpdateCubeStatus()
    {
        bool shuldActive = invert ? false : true;
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                if (map[w, h] == 1) {
                    cubes[w,h].SetActive(shuldActive);
                }
                else
                {
                    cubes[w, h].SetActive(!shuldActive);
                }
            }
        }
    }

    public void GenerateMap()
    {
        RandomFillMap();
        UpdateCubeStatus();
    }

    public void RandomFillMap()
    {
        if (useRandom)
        {
            seed = Time.time.ToString();
        }
        System.Random rand = new System.Random(seed.GetHashCode());

        for(int w = 0; w < width; w++)
        {
            for(int h = 0; h < height; h++)
            {
                if(w == 0 || w == width -1 || h == 0 || h == height -1)
                {
                    map[w, h] = 1;
                }
                else
                {
                    map[w, h] = rand.Next(0, 100) < randomFillPercent ? 1 : 0;
                }
            }
        }
    }
    
    public void SmoothMap()
    {
        for (int w = 0;w < width; w++)
        {
            for(int h = 0; h < height; h++)
            {
                int nearbyWalls = GetSurrounderWallCount(w, h);

                if(nearbyWalls > smoothinNeighbour)
                {
                    map[w, h] = 1;
                }
                else if (nearbyWalls < smoothinNeighbour)
                {
                    map[w,h] = 0;
                }
            }
        }
    }

    private int GetSurrounderWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for(int x = gridX -1; x <= gridX + 1; x++)
        {
            for(int y = gridY -1; y <= gridY + 1; y++)
            {
                if(x >= 0 && x < width && y >= 0 && y < height)
                {
                    if(x != gridX || y != gridY)
                    {
                        wallCount += map[x, y];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }
}
