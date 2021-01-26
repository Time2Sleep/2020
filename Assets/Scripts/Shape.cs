using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shape : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPos;

    public Color shapeColor;
    public Vector2[] shapeCells;
    private Coroutine currentCoroutine;
    [SerializeField] private bool isEven;

    private float offset = 0;

    private void Start()
    {
        startPos = GetComponent<RectTransform>().position;

        foreach (Vector2 vector2 in shapeCells)
        {
            if (vector2.y > offset) offset = vector2.y;
        }

        setUpStyles();
    }

    public void setUpStyles()
    {
        var colorScheme = FindObjectOfType<StyleManagerNew>().getCurrentColorScheme();
        var color = colorScheme.itemsColor[Random.Range(0, colorScheme.itemsColor.Length)];
        var sprite = colorScheme.sprite;
        shapeColor = color;
        foreach (Transform imageCell in transform)
        {
            var componentInChildren = imageCell.GetComponentInChildren<Image>();
            componentInChildren.color = color;
            componentInChildren.sprite = sprite;
        }
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
        GetComponent<Image>().raycastTarget = false;
        currentCoroutine = StartCoroutine(ChangeSize(new Vector3(0.95f, 0.95f, 1)));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        stopCoroutine();
        GetComponent<Image>().raycastTarget = true;
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
        GetComponent<RectTransform>().position =
            Input.mousePosition + new Vector3(isEven ? 50 : 0, 100f + offset * 50f, 0);
    }

   
}