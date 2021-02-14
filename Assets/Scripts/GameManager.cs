using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int[] cells = new int[100]; // 0 - пусто, 1 - занято
    [SerializeField] private Text scoreText;
    private int score;
    private float secondsleft;
    [SerializeField] private Slider secondsText;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject addTimerPopup;
    [SerializeField] private Text highScore;
    private SpawnShape spawner;
    [SerializeField] private string gameId = "4012861";
    [SerializeField] private bool testMode = true;

    private void Start()
    {
        Advertisement.Initialize(gameId, testMode);

        if (PlayerPrefs.GetInt("isGameOver", 0) == 1)
        {
            stopGame();
        }

        score = PlayerPrefs.GetInt("score", 0);
        secondsleft = PlayerPrefs.GetFloat("time", 30f);
        scoreText.text = score.ToString();
        spawner = FindObjectOfType<SpawnShape>();
        highScore.text = PlayerPrefs.GetString("highScore", "0");
        InvokeRepeating(nameof(saveTime), 3f, 3f);

        if (score == 0)
        {
            FindObjectOfType<LevelManager>().pause();
        }
    }

    public void newGame()
    {
        Debug.Log("loading new game");

        for (int i = 0; i < cells.Length; i++)
        {
            PlayerPrefs.SetInt(i.ToString(), 0);
            cells[i] = 0;
        }

        score = 0;
        secondsleft = 30f;
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("isGameOver", 0);
        PlayerPrefs.SetFloat("time", 30f);
        PlayerPrefs.DeleteKey("Shape0");
        PlayerPrefs.DeleteKey("Shape1");
        PlayerPrefs.DeleteKey("Shape2");
        scoreText.text = score.ToString();
        gameOver.SetActive(false);

        clearCells();

        for (int i = 0; i < spawner.transform.childCount; i++)
        {
            Destroy(spawner.transform.GetChild(i).GetChild(0).gameObject);
        }
    }

    void clearCells()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = 0;
            PlayerPrefs.SetInt(i.ToString(), 0);
            transform.GetChild(i).GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
    }

    public int[] getCells()
    {
        return cells;
    }

    public void loadValues()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = PlayerPrefs.GetInt(i.ToString(), 0);
        }
    }

    public bool checkCells(Vector2[] shapeCells, int dropCellIndex)
    {
        bool canPlace = true;
        try
        {
            foreach (Vector2 shapeCell in shapeCells)
            {
                int index = (int) shapeCell.y * (-10) + (int) shapeCell.x + dropCellIndex;

                if (cells[index] == 1)
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
            PlayerPrefs.SetInt("score", score);
            addTime(0.1f);

            PlayerPrefs.SetInt(cellIndex.ToString(), 1);
        }

        GameObject popup = Instantiate(addTimerPopup, GameObject.Find("spawn_zone").transform);
        popup.transform.position = Input.mousePosition + new Vector3(0, 200f, 0);
        popup.GetComponent<Text>().text = "+" + 0.1f * shapeCells.Length + "s";

        checkForBurn();
    }

    public void checkForGameOver()
    {
        foreach (Shape spawnShape in spawner.getCurrentShapes())
        {
            Vector2[] currentShape = spawnShape.shapeCells;
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
            PlayerPrefs.SetInt(listToDelete[i].ToString(), 0);
            transform.GetChild(listToDelete[i]).GetComponentInChildren<CellUI>().fadeOut();
            score++;
            addTime(0.5f);
            scoreText.text = score.ToString();
            PlayerPrefs.SetInt("score", score);
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
        secondsText.value = secondsleft / 60;


        if (secondsleft <= 0)
        {
            stopGame();
        }
    }

    void saveTime()
    {
        PlayerPrefs.SetFloat("time", secondsleft);
    }

    private void showAds()
    {
        if (Advertisement.IsReady()) {
            Advertisement.Show();
        } 
        else {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }

    private void stopGame()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0;
        PlayerPrefs.SetInt("isGameOver", 1);
        if (score > Int32.Parse(highScore.text))
        {
            PlayerPrefs.SetString("highScore", score.ToString());
            highScore.text = score.ToString();
        }
        showAds();
    }

    public CellUI[] GETCellUis()
    {
        return GetComponentsInChildren<CellUI>();
    }

    public Slider getSlider()
    {
        return secondsText;
    }

    public Text getScoreText()
    {
        return scoreText;
    }

    public Text getHighScoreText()
    {
        return highScore;
    }
}