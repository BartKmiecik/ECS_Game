using System;
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
    private GameObject mapParent;
    private GameObject[] mapObjects;
    public GameObject primitivePrefab;

    private void Start()
    {
        mapObjects = new GameObject[2];
        map = new int[width, height, maxDepth];
        cubes = new GameObject[width, height, maxDepth];
        GenerateEmptyMapHolders();
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            mapObjects[1].GetComponent<MeshMerger>().MeshMerge();
            Array.Clear(cubes, 0, cubes.Length);    
        }
    }

    private void GenerateEmptyMapHolders()
    {
        mapParent = new GameObject("MapParent"); 
        mapObjects[0] = new GameObject("MapDestructable");
        mapObjects[0].transform.parent = mapParent.transform;
        mapObjects[1] = new GameObject("MapBase");
        mapObjects[1].transform.parent = mapParent.transform;
        mapObjects[1].AddComponent<MeshMerger>();
    }

    private void SpawnCubes()
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                for(int d = 0; d < maxDepth; d++)
                {
                    cubes[w, h, d] = Instantiate(primitivePrefab);
                    cubes[w, h, d].transform.position = new Vector3(w, h, d);
                    cubes[w, h, d].transform.parent = mapObjects[d].transform;
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
