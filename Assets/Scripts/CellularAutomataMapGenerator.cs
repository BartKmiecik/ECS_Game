using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomataMapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    private int maxDepth = 2;

    public string seed;
    public bool useRandom;

    public int[] randomFillPercent;

    public int smoothinNeighbour;
    public int smoothinNeighbour2ndLevel;
    public bool invert;
    public bool invert2;
    int[,,] map;
    GameObject[,,] cubes;

    public List<Material> levelMaterials;

    private void Start()
    {
        map = new int[width, height, maxDepth];
        cubes = new GameObject[width, height, maxDepth];
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
                for(int d = 0; d < maxDepth; d++)
                {
                    cubes[w, h, d] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubes[w, h, d].transform.position = new Vector3(w, h, d);
                    cubes[w, h, d].GetComponent<MeshRenderer>().material = levelMaterials[d];
                }
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
                for (int d = 0; d < maxDepth; d++)
                {
                    if (map[w, h, d] == 1)
                    {
                        cubes[w, h, d].SetActive(shuldActive);
                    }
                    else
                    {
                        cubes[w, h, d].SetActive(!shuldActive);
                    }
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
                for (int d = 0; d < maxDepth; d++)
                {
                    if (w == 0 || w == width - 1 || h == 0 || h == height - 1)
                    {
                        map[w, h, d] = 1;
                    }
                    else
                    {
                        map[w, h, d] = rand.Next(0, 100) < randomFillPercent[d] ? 1 : 0;
                    }
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
                for (int d = 0; d < maxDepth; d++)
                {
                    int nearbyWalls = GetSurrounderWallCount(w, h, d);

                    if (d == 1)
                    {
                        if (map[w, h, 0] == 0)
                        {
                            map[w, h, d] = 0;
                        }
                        else
                        {
                            if (nearbyWalls > smoothinNeighbour)
                            {
                                map[w, h, d] = 1;
                            }
                            else if (nearbyWalls < smoothinNeighbour)
                            {
                                map[w, h, d] = 0;
                            }
                        }
                    }
                    else
                    {
                        if (nearbyWalls > smoothinNeighbour2ndLevel)
                        {
                            map[w, h, d] = 1;
                        }
                        else if (nearbyWalls < smoothinNeighbour2ndLevel)
                        {
                            map[w, h, d] = 0;
                        }
                    }

                }    
            }
        }
    }

    private int GetSurrounderWallCount(int gridX, int gridY, int gridD)
    {
        int wallCount = 0;
        for(int x = gridX -1; x <= gridX + 1; x++)
        {
            for(int y = gridY -1; y <= gridY + 1; y++)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (x != gridX || y != gridY)
                    {
                        wallCount += map[x, y, gridD];
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
