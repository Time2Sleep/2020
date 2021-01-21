using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StyleManager : MonoBehaviour
{
    [SerializeField]private Sprite[] sprites;

    public UnityEvent changeSkin;

    private int currentSpriteId = 1;

    public void changeStyle(int id)
    {
        currentSpriteId = id;
        changeSkin.Invoke();
    }

    public Sprite getStyle()
    {
        return sprites[currentSpriteId];
    }
}
