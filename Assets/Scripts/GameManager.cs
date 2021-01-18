using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int[] cells = new int[100]; // 0 - пусто, 1 - занято
    [SerializeField] private Text scoreText;
    private int score = 0;
    private void Start()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = 0;
        }
    }

    public bool checkCells(Vector2[] shapeCells, int dropCellIndex)
    {
        bool canPlace = true;
        foreach (Vector2 shapeCell in shapeCells)
        {
            if (cells[(int)shapeCell.y*(-10) + (int)shapeCell.x + dropCellIndex] == 1) //можно за пределы массива уйти
            {
                canPlace = false;
                Debug.Log("Can't place here");
            }

            if (((int)shapeCell.y*(-10) + (int)shapeCell.x + dropCellIndex) % 10 == 9 && shapeCell.x < 0)
            {
                canPlace = false;
                Debug.Log("i'm here");
            }
        }
        return canPlace;
    }

    public void placeShape(Vector2[] shapeCells, int dropCellIndex, Color color)
    {
        foreach (Vector2 shapeCell in shapeCells)
        {
            int cellIndex = (int) shapeCell.y * (-10) + (int) shapeCell.x + dropCellIndex;
            cells[cellIndex] = 1;
            transform.GetChild(cellIndex).GetComponent<Image>().color = color;
            
            score+=5;
            scoreText.text = score.ToString();
                
            Debug.Log("placed to " + cellIndex);
        }
    }
}
