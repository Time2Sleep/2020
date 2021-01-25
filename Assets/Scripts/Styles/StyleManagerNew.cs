using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;

public class StyleManagerNew : MonoBehaviour
{
    public ColorSchemeSO currentColorScheme;
    public ItemSkinSO currentItemSkin;
    private Slider timerSlider;
    private Text scoreText;
    private CellUI[] cells;
    private SpawnShape spawner;

    private void Start()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        spawner = FindObjectOfType<SpawnShape>();


        Debug.Log(spawner);
        cells = gameManager.GETCellUis();
        scoreText = gameManager.getScoreText();
        timerSlider = gameManager.getSlider();
    }

    public void changeColorScheme(ColorSchemeSO colorScheme)
    {
        currentColorScheme = colorScheme;
        updateStaticContentStyle();
    }

    public void changeItemSkin(ItemSkinSO itemSkin)
    {
        currentItemSkin = itemSkin;
        updateCells();
    }

    private void updateStaticContentStyle()
    {
        scoreText.color = currentColorScheme.textColor;
        Camera.main.backgroundColor = currentColorScheme.backgroundColor;
        GameObject.Find("Pause Overlay").GetComponent<Image>().color = currentColorScheme.backgroundColor;
        GameObject.Find("Theme Overlay").GetComponent<Image>().color = currentColorScheme.backgroundColor;
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
        var color = currentColorScheme.itemsColor[Random.Range(0, currentColorScheme.itemsColor.Length)];
        foreach (CellUI cellUi in cells)
        {
            Color cellColor = cellUi.transform.GetChild(1).GetComponent<Image>().color;
            if (cellColor.a > 0f)
            {
                cellUi.transform.GetChild(1).GetComponent<Image>().color = color;
            }
        }

        updateAlreadySpawnedShapes();
    }
}