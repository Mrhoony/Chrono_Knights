using UnityEngine;

public static class SpriteSet
{
    public static readonly Sprite[] itemSprite = Resources.LoadAll<Sprite>("Graphic/Item/ui_itemset");
    public static readonly Sprite[] skillSprite;
    public static readonly Sprite[] markerSprite = Resources.LoadAll<Sprite>("Graphic/UI/ui_mark");
    public static readonly Sprite[] keyItemBorderSprite = Resources.LoadAll<Sprite>("Graphic/UI/Inventory_Set");
    public static readonly Sprite[] shopItemBorderSprite = Resources.LoadAll<Sprite>("Graphic/UI/ui_shop_set");
    public static readonly Sprite[] enchantSlotImage = Resources.LoadAll<Sprite>("Graphic/UI/ui_enchant_set");
    public static readonly Sprite[] upgradeSlotImage = Resources.LoadAll<Sprite>("Graphic/UI/ui_upgrade_set");
    public static readonly Sprite[] quickSlotImage = Resources.LoadAll<Sprite>("Graphic/UI/ui_quickSlot");
}
