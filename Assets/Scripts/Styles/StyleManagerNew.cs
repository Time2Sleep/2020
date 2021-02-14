using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;

public class StyleManagerNew : MonoBehaviour
{
    private ColorSchemeSO currentColorScheme;
    [SerializeField] private ColorSchemeSO[] colorSchemes;
    private Slider timerSlider;
    private Text scoreText;
    private Text HighScoreText;
    private CellUI[] cells;
    private SpawnShape spawner;
    [SerializeField] private Transform ThemeOverlayPanel;
    [SerializeField] private Transform PauseOverlayPanel;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spawner = FindObjectOfType<SpawnShape>();
        Debug.Log(spawner);
        cells = gameManager.GETCellUis();
        scoreText = gameManager.getScoreText();
        HighScoreText = gameManager.getHighScoreText();
        timerSlider = gameManager.getSlider();
        currentColorScheme = colorSchemes[PlayerPrefs.GetInt("style", 0)];
        
        for(int j=0; j<colorSchemes.Length; j++)
        {
            GameObject themePanel = Instantiate(Resources.Load<GameObject>("Prefabs/theme"), ThemeOverlayPanel);
            themePanel.GetComponent<Image>().color = colorSchemes[j].backgroundColor;
            themePanel.GetComponentInChildren<Text>().color = colorSchemes[j].textColor;
            themePanel.GetComponentInChildren<Text>().text = colorSchemes[j].name;
            int id = j;
            themePanel.GetComponent<Button>().onClick.AddListener(delegate { changeColorScheme(id); });
            
            for (int i = 0; i < themePanel.transform.GetChild(0).childCount; i++)
            {
                themePanel.transform.GetChild(0).GetChild(i).GetComponent<Image>().color = colorSchemes[j].itemsColor[Random.Range(0, colorSchemes[j].itemsColor.Length)];
                themePanel.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = colorSchemes[j].sprite;
            }
        }
        
        updateStaticContentStyle();
        
        gameManager.loadValues();
        loadCells();

       
    }

    public void changeColorScheme(int colorSchemeId)
    {
        currentColorScheme = colorSchemes[colorSchemeId];
        PlayerPrefs.SetInt("style", colorSchemeId);
        updateStaticContentStyle();
        ThemeOverlayPanel.transform.parent.transform.parent.gameObject.SetActive(false);
    }

    public ColorSchemeSO getCurrentColorScheme()
    {
        return currentColorScheme;
    }

    private void updateStaticContentStyle()
    {
        scoreText.color = getCurrentColorScheme().textColor;
        GameObject.Find("PauseButton").GetComponentInChildren<Text>().color = getCurrentColorScheme().textColor;
        GameObject.Find("Logo").GetComponent<Image>().color = getCurrentColorScheme().textColor;
        HighScoreText.color = getCurrentColorScheme().highScoreColor;
        Camera.main.backgroundColor = getCurrentColorScheme().backgroundColor;
        PauseOverlayPanel.GetComponent<Image>().color = getCurrentColorScheme().backgroundColor;
        ThemeOverlayPanel.transform.parent.gameObject.GetComponent<Image>().color = getCurrentColorScheme().backgroundColor;
        updateCells();
    }

    private void updateAlreadySpawnedShapes()
    {
        foreach (Shape shape in spawner.getCurrentShapes())
        {
            shape.setUpStyles();
        }
    }

    private void updateCells()
    {
        var sprite = getCurrentColorScheme().sprite;
        foreach (CellUI cellUi in cells)
        {
            var color = getCurrentColorScheme().itemsColor[Random.Range(0, getCurrentColorScheme().itemsColor.Length)];

            Color cellColor = cellUi.transform.GetChild(1).GetComponent<Image>().color;
            if (cellColor.a > 0f)
            {
                cellUi.transform.GetChild(1).GetComponent<Image>().color = color;
            }
            cellUi.transform.GetChild(1).GetComponent<Image>().sprite = sprite;
        }

        updateAlreadySpawnedShapes();
    }
    
    private void loadCells()
    {
        var sprite = getCurrentColorScheme().sprite;

        for (int i = 0; i < gameManager.getCells().Length; i++)
        {
            int index = Random.Range(0, getCurrentColorScheme().itemsColor.Length);
            if (gameManager.getCells()[i] == 1)
            {
                cells[i].transform.GetChild(1).GetComponent<Image>().color = getCurrentColorScheme().itemsColor[index];
            }
            cells[i].transform.GetChild(1).GetComponent<Image>().sprite = sprite;
        }

        updateAlreadySpawnedShapes();
    }
}