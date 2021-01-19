using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnShape : MonoBehaviour
{
    [SerializeField] private GameObject[] shapes;

    void spawnRandomShape()
    {
        int random = Random.Range(0, shapes.Length);
        GameObject shape = Instantiate(shapes[random]);
        shape.transform.position = transform.position;
        shape.transform.SetParent(transform);
    }

    private void Start()
    {
        spawnRandomShape();
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            spawnRandomShape();
        }
    }
}
