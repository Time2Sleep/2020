using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int[] cells = new int[100]; // 0 - пусто, 1 - занято
    [SerializeField] private Text scoreText;
    private int score = 0;
    [SerializeField] private float secondsleft = 30f;
    [SerializeField] private Slider secondsText;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject addTimerPopup;
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
               /*if (index < 0)
                {
                    return false;
                }*/

                if (cells[index] == 1) //можно за пределы массива уйти
                {
                    canPlace = false;
                }
                if (index % 10 == 9 && shapeCell.x < 0)
                {
                    canPlace = false;
                }

                if (index % 10 == 0 && shapeCell.x >= 1)
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
        
        GameObject popup = Instantiate(addTimerPopup, GameObject.Find("Canvas").transform);
        popup.transform.position = Input.mousePosition + new Vector3(0, 200f, 0);
        popup.GetComponent<Text>().text = "+" + 0.1f * shapeCells.Length + "s";
        
        checkForBurn();
    }

    public void checkForGameOver()
    {
        foreach (SpawnShape spawnShape in spawners)
        {
            Vector2[] currentShape = spawnShape.getCurrentShape().shapeCells;
            for (int i = 0; i < cells.Length; i++)
            {
                bool canBePlaced = checkCells(currentShape, i);
                    if (canBePlaced)
                    {
                       return;
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

        for (int i = 0; i < listToDelete.Count; i++) //Пробегаемся по всем и удаляем
        {
            cells[listToDelete[i]] = 0;
            transform.GetChild(listToDelete[i]).GetComponentInChildren<CellUI>().fadeOut();
            score++;
            addTime(0.5f);
            scoreText.text = score.ToString();
        }

        listToDelete.Clear();
    }

    private void addTime(float seconds)
    {
        secondsleft += seconds;
        if (secondsleft > 60)
        {
            secondsleft = 60;
        }
    }

    private void FixedUpdate()
    {
        secondsleft -= 0.02f;
        secondsText.value = secondsleft/60;

        if (secondsleft <= 0)
        {
            //stopGame();
        }
    }

    private void stopGame()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }

    public void applyStyle()
    {
        CellUI[] cellUis = GetComponentsInChildren<CellUI>();
        foreach (CellUI cellUi in cellUis)
        {
            cellUi.applyStyle();
        }
        foreach (SpawnShape spawnShape in spawners)
        {
            spawnShape.getCurrentShape().applyStyle();
        }
    }
}