using UnityEngine;
public class Item_Looting : MonoBehaviour
{
    Menu_Inventory inventory;
    float speed;
    Item item;
    public GameObject eft_itemRoot;

    private void OnEnable()
    {
        inventory = GameObject.Find("UI/InGameMenu/Inventory").GetComponent<Menu_Inventory>();
        speed = 1f;
        item = Database_Game.instance.ItemSetting();
        GetComponent<SpriteRenderer>().sprite = item.sprite;
    }

    private void Update()
    {
        speed += Time.deltaTime * 2f;
        transform.position = new Vector2(transform.position.x, transform.position.y + Mathf.Sin(speed) * 0.002f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (inventory.PutInInventory(item, false))
            {
                Instantiate(eft_itemRoot, collision.transform.position, Quaternion.identity);
                gameObject.SetActive(false);
            }
        }
    }
}
