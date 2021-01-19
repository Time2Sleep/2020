using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShape : MonoBehaviour
{
    [SerializeField] private GameObject[] shapes;
    private GameManager gameManager;

    void spawnRandomShape()
    {
        int random = Random.Range(0, shapes.Length);
        GameObject shape = Instantiate(shapes[random], transform);
        shape.transform.position = transform.position;
    }

    private void Start()
    {
        spawnRandomShape();
    }

    public Vector2[] getCurrentShape()
    {
        return GetComponentInChildren<Shape>().shapeCells;
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            spawnRandomShape();
        }
    }
}