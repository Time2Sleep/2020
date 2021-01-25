using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ColorScheme
{
    public Color getBackgroundSkin();

    public Color[] getItemColors();

    public Color getTextColor();
}