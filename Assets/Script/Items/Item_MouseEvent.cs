using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_MouseEvent : MonoBehaviour
{
    GameObject[] slots;
    bool[] isFull;
    int slotCount;
    GameObject slot;
    public Image tempImage;
    Vector3 mousePosition;
    Camera _mainCamera;
    Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        sprites = Resources.LoadAll<Sprite>("UI/Inventory_Set");
        slots = GetComponent<Menu_Inventory>().slot;
        isFull = GetComponent<Menu_Inventory>().isFull;
        slotCount = slots.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MouseDown()
    {
        int i;
        for (i = 0; i<slotCount; ++i)
        {
            if (slot == slots[i])
                break;
        }
        if (isFull[i])
        {
            slot = GetClickedObject();
            tempImage.sprite = slot.GetComponent<Image>().sprite;
            slot.GetComponent<Image>().sprite = sprites[6];
        }
    }

    public void MouseDown_Up()
    {

    }

    private GameObject GetClickedObject()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        slot = null;
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if(slot.CompareTag("ItemSlot"))
                slot = hit.collider.gameObject;
        }
        return slot;
    }
}
