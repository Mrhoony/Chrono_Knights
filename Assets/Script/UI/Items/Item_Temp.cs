using UnityEngine;

public enum ItemType
{
    Key
}

public class Item_Temp : MonoBehaviour
{
    public ItemType Type;
    public string Name;
    public int Value;
    public string Description;

    public Item_Temp(ItemType type)
    {
        switch (type)
        {
            case ItemType.Key:
                {
                    Name = "키";
                    Value = Random.Range(0, 5);
                    Description = "'" + Value.ToString() + "' 라 쓰인 키이다.";
                }
                break;
        }        
    }
}