using UnityEngine;

[CreateAssetMenu(fileName = "ItemSkin", menuName = "Styles/ItemSkin", order = 1)]
public class ItemSkinSO : ScriptableObject, ItemSkin
{
    public Sprite skinSprite;

    public Sprite getItemSprite()
    {
        return skinSprite;
    }
}