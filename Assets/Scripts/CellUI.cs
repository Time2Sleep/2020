using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image;
    private Color defaultColor = new Color(0, 0, 0, 0.254f);

    void Start()
    {
        image = transform.GetChild(0).GetComponentInChildren<Image>();
    }

    public void changeColor(Color color)
    {
        image.color = color;
    }

    public void fadeOut()
    {
        StartCoroutine(ChangeSize());
    }

    IEnumerator ChangeSize()
    {
        float startTime = Time.time;
        float overTime = 0.1f;
        while (Time.time < startTime + overTime)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 10);
            Debug.Log("computing");
            yield return null;
        }

        Debug.Log("calling to default");
        toDefault();
    }

    private void toDefault()
    {
        transform.localScale = new Vector3(1, 1, 1);
        image.color = defaultColor;
    }
}