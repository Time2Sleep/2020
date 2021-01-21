using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StyleManager : MonoBehaviour
{
    [SerializeField] private Color bgColorLight; 
    [SerializeField] private Color bgColorDark;
    private string skin = "box";
    private string flatness = "flat";
    private bool isDark = true;

    public UnityEvent changeSkin;

    private int currentSpriteId = 1;

    public void changeStyle()
    {
        changeSkin.Invoke();
    }

    public Sprite getStyle()
    {
        return Resources.Load<Sprite>("Sprites/"+skin+"_"+flatness);
    }

    public void setFlatness(string Path)
    {
        flatness = Path;
    }
    
    public void setSkin(string Path)
    {
        skin = Path;
    }
    

    public void changeTheme()
    {
        isDark = !isDark;
        if (isDark)
        {
            Camera.main.backgroundColor = bgColorDark;
        }
        else
        {
            Camera.main.backgroundColor = bgColorLight;
        }

        GameObject.Find("Pause Overlay").GetComponent<Image>().color = Camera.main.backgroundColor;
        GameObject.Find("Theme Overlay").GetComponent<Image>().color = Camera.main.backgroundColor;
    }
        
}
