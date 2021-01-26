using UnityEngine;

[CreateAssetMenu(fileName = "ColorScheme", menuName = "Styles/ColorScheme", order = 1)]
public class ColorSchemeSO : ScriptableObject, ColorScheme
{
    public string name;
    public Color backgroundColor;
    public Color[] itemsColor;
    public Color textColor;
    public Color highScoreColor;
    public Sprite sprite;

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