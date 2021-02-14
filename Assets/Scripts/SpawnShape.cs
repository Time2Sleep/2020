using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnShape : MonoBehaviour
{
    [SerializeField] private GameObject[] shapes;
    [SerializeField] private GameObject[] SpawnPoints;
    private GameManager gameManager;

    void spawnRandomShape(Transform target)
    {
        int random = Random.Range(0, shapes.Length);
        GameObject shape = Instantiate(shapes[random], target);
        shape.name = shapes[random].name;
        shape.transform.position = target.transform.position;
        PlayerPrefs.SetString("Shape"+target.GetSiblingIndex(), shape.name);
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        foreach (GameObject spawnPoint in SpawnPoints)
        {
            if (PlayerPrefs.HasKey("Shape2"))
            {
                Debug.Log(PlayerPrefs.GetString("Shape"+spawnPoint.transform.GetSiblingIndex()));
                GameObject shape = Instantiate(Resources.Load<GameObject>("Prefabs/"+PlayerPrefs.GetString("Shape"+spawnPoint.transform.GetSiblingIndex())), spawnPoint.transform);
                shape.transform.position = spawnPoint.transform.position;
            }
            else
            {
                spawnRandomShape(spawnPoint.transform);
            }
        }
    }

    public Shape[] getCurrentShapes()
    {
        return GetComponentsInChildren<Shape>();
    }

    private void Update()
    {
        foreach (GameObject spawnPoint in SpawnPoints)
        {
            if (spawnPoint.transform.childCount == 0)
            {
                spawnRandomShape(spawnPoint.transform);
                gameManager.checkForGameOver();
            }
        }
    }
}