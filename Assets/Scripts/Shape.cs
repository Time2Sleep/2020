using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler,
    IPointerUpHandler
{
    private GameManager gameManager;
    private Vector3 startPos;

    public Color shapeColor;
    public Vector2[] shapeCells;
    private Coroutine currentCoroutine;

    private void Start()
    {
        startPos = GetComponent<RectTransform>().position;
        gameManager = FindObjectOfType<GameManager>();
    }


    IEnumerator ChangeSize(Vector3 targetSize)
    {
        float startTime = Time.time;
        float overTime = 0.5f;
        while (Time.time < startTime + overTime)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetSize, (Time.time - startTime) / overTime);
            yield return null;
        }

        transform.localScale = targetSize;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        stopCoroutine();
        currentCoroutine = StartCoroutine(ChangeSize(new Vector3(0.95f, 0.95f, 1)));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        stopCoroutine();
        currentCoroutine = StartCoroutine(ChangeSize(new Vector3(0.6f, 0.6f, 1)));
        GetComponent<RectTransform>().position = startPos;
    }

    private void stopCoroutine()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().position = Input.mousePosition + new Vector3(0, 90f, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnBeginDrag(eventData);
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnEndDrag(eventData);
    }
}