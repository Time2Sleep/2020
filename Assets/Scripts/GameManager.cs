using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int[] cells = new int[100]; // 0 - пусто, 1 - занято
    [SerializeField] private Text scoreText;
    private int score = 0;
    [SerializeField] private float secondsleft = 30f;
    [SerializeField] private Text secondsText;
    [SerializeField] private GameObject gameOver;
    private SpawnShape[] spawners;

    private void Start()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = 0;
        }

        spawners = FindObjectsOfType<SpawnShape>();
    }

    public bool checkCells(Vector2[] shapeCells, int dropCellIndex)
    {
        bool canPlace = true;
        try
        {
            foreach (Vector2 shapeCell in shapeCells)
            {
                int index = (int) shapeCell.y * (-10) + (int) shapeCell.x + dropCellIndex;
                if (index < 0)
                {
                    return false;
                }

                if (cells[index] == 1
                ) //можно за пределы массива уйти
                {
                    canPlace = false;
                }

                if (index % 10 == 9 && shapeCell.x < 0)
                {
                    canPlace = false;
                }
            }
        }
        catch (Exception e)
        {
            return false;
        }


        return canPlace;
    }

    public void placeShape(Vector2[] shapeCells, int dropCellIndex, Color color)
    {
        foreach (Vector2 shapeCell in shapeCells)
        {
            int cellIndex = (int) shapeCell.y * (-10) + (int) shapeCell.x + dropCellIndex;
            cells[cellIndex] = 1;
            transform.GetChild(cellIndex).GetComponentInChildren<CellUI>().changeColor(color);

            score++;
            scoreText.text = score.ToString();
            addTime(0.1f);

        }

        checkForBurn();
    }

    private void checkForGameOver()
    {
        foreach (SpawnShape spawnShape in spawners)
        {
            Vector2[] currentShape = spawnShape.getCurrentShape();

            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] == 0)
                {
                    bool canBePlaced = checkCells(currentShape, i);
                    if (canBePlaced)
                    {
                        return;
                    }
                }
            }
        }

        stopGame();
    }

    void checkForBurn()
    {
        List<int> listToDelete = new List<int>();
        int counter = 0;
        for (int i = 0; i < 100; i++) //horizontal
        {
            if (i % 10 == 0) counter = 0;
            if (cells[i] == 1) counter++;
            if (counter == 10)
            {
                for (int j = 0; j < 10; j++)
                {
                    listToDelete.Add(i - j);
                }
            }
        }

        for (int i = 0; i < 10; i++) //Vertical
        {
            counter = 0;
            for (int j = 0; j < 10; j++)
            {
                if (cells[j * 10 + i] == 1) counter++;
            }

            if (counter == 10)
            {
                for (int j = 0; j < 10; j++)
                {
                    listToDelete.Add(i + j * 10);
                }
            }
        }

        for (int i = 0; i < listToDelete.Count; i++)
        {
            cells[listToDelete[i]] = 0;
            transform.GetChild(listToDelete[i]).GetComponentInChildren<CellUI>().fadeOut();
            score++;
            addTime(0.5f);
            scoreText.text = score.ToString();
        }

        listToDelete.Clear();
        checkForGameOver();
    }

    private void addTime(float seconds)
    {
        secondsleft += seconds;
    }

    private void FixedUpdate()
    {
        secondsleft -= 0.02f;
        secondsText.text = secondsleft.ToString("0.0") + "s";

        if (secondsleft <= 0)
        {
            stopGame();
        }
    }

    private void stopGame()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }
}