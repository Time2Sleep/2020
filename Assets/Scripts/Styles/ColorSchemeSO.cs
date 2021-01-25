using UnityEngine;

[CreateAssetMenu(fileName = "ColorScheme", menuName = "Styles/ColorScheme", order = 1)]
public class ColorSchemeSO : ScriptableObject, ColorScheme
{
    public Color backgroundColor;
    public Color[] itemsColor;
    public Color textColor;

    public Color getBackgroundSkin()
    {
        return backgroundColor;
    }

    public Color[] getItemColors()
    {
        return itemsColor;
    }

    public Color getTextColor()
    {
        return textColor;
    }
}