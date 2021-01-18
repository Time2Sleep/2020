using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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
            
            score+=3;
            scoreText.text = score.ToString();
            
            Debug.Log("placed to " + cellIndex);
        }
        
        checkForBurn();
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
                    listToDelete.Add(i-j);
                }
            }
        }
        
        counter = 0;
        for (int i = 0; i < 10; i++) //Vertical
        {
            counter = 0;
            for (int j = 0; j < 10; j++)
            {
                if (cells[j*10+i] == 1) counter++;
            }
           
            if (counter == 10)
            {
                for (int j = 0; j < 10; j++)
                {
                    listToDelete.Add(i+j*10);
                }
            }
        }
        
        for (int i = 0; i < listToDelete.Count; i++)
        {
            cells[listToDelete[i]] = 0; 
            transform.GetChild(listToDelete[i]).GetComponent<Image>().color = new Color(0, 0, 0, 0.254f);
            score+=5;
            scoreText.text = score.ToString();
        }
        listToDelete.Clear();
    }
}
