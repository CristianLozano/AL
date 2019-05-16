using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutomataGenerator : MonoBehaviour
{
    public int height;
    public int width;
    public int numberOfIterations;
    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;
    public bool gameOfLife;
    public GameObject panelPrefab;
    public TextMeshProUGUI counterText;

    private int[,] map;
    private int[,] copiedMap;
    private int iterationCounter = 0;
    private int antX, antY;
    private int antDirection = 0; // 0 -> Up, 1 -> Right, 2 -> Down, 3 -> Left
    private int previousTileValue;

    void Start()
    {
        counterText.text = "0";
        GenerateMap();
    }

    void Update()
    {
        if (iterationCounter < numberOfIterations)
        {
            DeleteMap();
            if (gameOfLife)
            {
                GameOfLifeIteration();
            }
            else
            {
                LangtonsAntIteration();
            }
            FillMap();
            iterationCounter++;
            counterText.text = iterationCounter.ToString();
        }
    }

    private void GenerateMap()
    {
        map = new int[height, width];

        // Instantiate background
        GameObject background = Instantiate(panelPrefab);
        background.transform.localScale = new Vector3(width/10f, 1, height/10f);
        background.transform.localPosition = new Vector3(width / 2 - 0.5f, -1, -(height / 2 - 0.5f));

        RandomFillMap();

        if (!gameOfLife)
        {
            int randomX = UnityEngine.Random.Range(0, height);
            int randomY = UnityEngine.Random.Range(0, width);

            // Ant is represented by number 2
            map[randomX, randomY] = 2;

            antX = randomX;
            antY = randomY;

            Debug.Log("Ant Start: " + antX + " " + antY);

            MoveAnt();
        }

        FillMap();
    }

    private void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
            }
        }
    }

    private void GameOfLifeIteration()
    {
        copiedMap = new int[height, width];
        Array.Copy(map, 0, copiedMap, 0, map.Length);
        
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                int neighbourWallTiles = GetNeighbourCount(x, y);

                if (neighbourWallTiles < 2)
                {
                    // Dies of loneliness
                    map[x, y] = 0;
                }
                else if (neighbourWallTiles == 2)
                {
                    if (copiedMap[x, y] == 0)
                    {
                        // Dies
                        map[x, y] = 0;
                    }
                    else
                    {
                        // Survives
                        map[x, y] = 1;
                    }
                }
                else if (neighbourWallTiles == 3)
                {
                    // Survives/Reproduce
                    map[x, y] = 1;
                }
                else if (neighbourWallTiles > 3)
                {
                    // Dies of overcrowding
                    map[x, y] = 0;
                }
            }
        }
    }

    private void LangtonsAntIteration()
    {
        // Ant in white space
        if (previousTileValue == 0)
        {
            RotateAnt(false);
            map[antX, antY] = 1;
        }
        else // Ant in black space
        {
            RotateAnt(true);
            map[antX, antY] = 0;
        }

        // Move ant forward
        MoveAnt();
    }

    private void MoveAnt()
    {
        switch (antDirection)
        {
            // Up
            case 0:
                antX--;
                break;

            // Right
            case 1:
                antY++;
                break;

            // Down
            case 2:
                antX++;
                break;

            // Right
            case 3:
                antY--;
                break;
        }

        if (antX < 0)
        {
            antX = height - 1;
        }
        if (antX > height - 1)
        {
            antX = 0;
        }
        if (antY < 0)
        {
            antY = width - 1;
        }
        if (antY > width - 1)
        {
            antY = 0;
        }

        previousTileValue = map[antX, antY];
        map[antX, antY] = 2;
    }

    private void RotateAnt(bool right)
    {
        if (right)
        {
            antDirection++;
            if (antDirection > 3)
            {
                antDirection = 0;
            }
        }
        else
        {
            antDirection--;
            if (antDirection < 0)
            {
                antDirection = 3;
            }
        }
    }

    private int GetNeighbourCount(int gridX, int gridY)
    {
        int wallCount = 0;

        if (gridX == 0 && gridY == 0)
        {
            wallCount += copiedMap[0, 1] + copiedMap[1, 1] + copiedMap[1, 0] + copiedMap[0, width - 1] +
                         copiedMap[1, width - 1] + copiedMap[height - 1, width - 1] + copiedMap[height - 1, 0] + copiedMap[height - 1, 1];
        }
        else if (gridX == 0 && gridY == width - 1)
        {
            wallCount += copiedMap[0, width - 2] + copiedMap[1, width - 2] + copiedMap[1, width - 1] + copiedMap[0, 0] +
                         copiedMap[1, 0] + copiedMap[height - 1, width - 2] + copiedMap[height - 1, width - 1] + copiedMap[height - 1, 0];
        }
        else if (gridX == height - 1 && gridY == 0)
        {
            wallCount += copiedMap[height - 2, 0] + copiedMap[height - 2, 1] + copiedMap[height - 1, 1] + copiedMap[height - 2, width - 1] +
                         copiedMap[height - 1, width - 1] + copiedMap[0, 0] + copiedMap[0, 1] + copiedMap[0, width - 1];
        }
        else if (gridX == height - 1 && gridY == width - 1)
        {
            wallCount += copiedMap[height - 1, width - 2] + copiedMap[height - 2, width - 2] + copiedMap[height - 2, width - 1] + copiedMap[height - 2, 0] +
                         copiedMap[height - 1, 0] + copiedMap[0, width - 2] + copiedMap[0, width - 1] + copiedMap[0, 0];
        }
        else if (gridX == 0)
        {
            wallCount += copiedMap[gridX, gridY - 1] + copiedMap[gridX + 1, gridY - 1] + copiedMap[gridX + 1, gridY] + copiedMap[gridX + 1, gridY + 1] +
                         copiedMap[gridX, gridY + 1] + copiedMap[height - 1, gridY - 1] + copiedMap[height - 1, gridY] + copiedMap[height - 1, gridY + 1];
        }
        else if (gridX == height - 1)
        {
            wallCount += copiedMap[gridX, gridY - 1] + copiedMap[gridX - 1, gridY - 1] + copiedMap[gridX - 1, gridY] + copiedMap[gridX - 1, gridY + 1] +
                         copiedMap[gridX, gridY + 1] + copiedMap[0, gridY - 1] + copiedMap[0, gridY] + copiedMap[0, gridY + 1];
        }
        else if (gridY == 0)
        {
            wallCount += copiedMap[gridX - 1, gridY] + copiedMap[gridX - 1, gridY + 1] + copiedMap[gridX, gridY + 1] + copiedMap[gridX + 1, gridY + 1] +
                         copiedMap[gridX + 1, gridY] + copiedMap[gridX - 1, width - 1] + copiedMap[gridX, width - 1] + copiedMap[gridX + 1, width - 1];
        }
        else if (gridY == width - 1)
        {
            wallCount += copiedMap[gridX - 1, gridY] + copiedMap[gridX - 1, gridY - 1] + copiedMap[gridX, gridY - 1] + copiedMap[gridX + 1, gridY - 1] +
                         copiedMap[gridX + 1, gridY] + copiedMap[gridX - 1, 0] + copiedMap[gridX, 0] + copiedMap[gridX + 1, 0];
        }
        else
        {
            // General case
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < height && neighbourY >= 0 && neighbourY < width)
                    {
                        // Not count current tile
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += copiedMap[neighbourX, neighbourY];
                        }
                    }
                }
            }
        }

        //Debug.Log("PosX: " + gridX + " PosY: " + gridY);
        //Debug.Log("Wall Count: " + wallCount);
        return wallCount;
    }


    private void FillMap()
    {
        if (map != null)
        {
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    // Instantiate only blacks to improve performance
                    if (map[x, y] == 1)
                    {
                        GameObject panel = Instantiate(panelPrefab, transform);
                        panel.GetComponent<Renderer>().material.color = Color.black;
                        panel.transform.localPosition = new Vector3(x, 0, y);
                    }

                    // Instantiate ant if it is used
                    if (map[x, y] == 2)
                    {
                        GameObject panel = Instantiate(panelPrefab, transform);
                        panel.GetComponent<Renderer>().material.color = Color.red;
                        panel.transform.localPosition = new Vector3(x, 0, y);
                    }
                }
            }
        }
    }

    private void DeleteMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}