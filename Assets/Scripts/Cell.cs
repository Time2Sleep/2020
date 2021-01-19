using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IDropHandler
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Shape shape = eventData.pointerDrag.GetComponent<Shape>();
        if (gameManager.checkCells(shape.shapeCells, transform.GetSiblingIndex() - 10))
        {
            gameManager.placeShape(shape.shapeCells, transform.GetSiblingIndex() - 10, shape.shapeColor);
            Destroy(eventData.pointerDrag.gameObject);
        }
    }
}