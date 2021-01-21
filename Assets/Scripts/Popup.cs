using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    private Text popupText;
    // Start is called before the first frame update
    void Start()
    {
        popupText = GetComponent<Text>();
        StartCoroutine(FadeOut());
        StartCoroutine(destroyThis());
    }

    
    IEnumerator FadeOut()
    {
        while (popupText.color.a > 0)
        {
            popupText.color = Color.Lerp(popupText.color, new Color(255f, 255f, 255f, 0f), Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator destroyThis()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(0, 1f, 0);
    }
}
