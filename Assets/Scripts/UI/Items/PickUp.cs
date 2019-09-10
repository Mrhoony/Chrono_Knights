using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    Inventory _Inventory;

    private void Start()
    {
        _Inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            for(int i=0; i<_Inventory.Slot.Length; i++)
            {
                if(_Inventory.isFull[i] == false)
                {
                    _Inventory.isFull[i] = true;
                    _Inventory.Slot[i].GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}