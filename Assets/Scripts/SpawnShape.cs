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
        shape.transform.position = target.transform.position;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        foreach (GameObject spawnPoint in SpawnPoints)
        {
            spawnRandomShape(spawnPoint.transform);
        }
    }

    public Shape getCurrentShape()
    {
        return GetComponentInChildren<Shape>();
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