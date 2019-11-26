using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    DungeonManager dm;
    CircleCollider2D cc;
    GameObject menu;

    bool isStand = false;

    private void Awake()
    {
        dm = DungeonManager.instance;
        cc = GetComponent<CircleCollider2D>();
        menu = CanvasManager.instance.UI_Menu;
    }

    private void OnTriggerEnter2D(Collider2D collision) // 플레이어 텔레포터 영역 진입하면 true
    {
        if (collision.CompareTag("Player"))
        {
            dm.isStanding(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  // 플레이어 텔레포터 영역 벗어나면 false
    {
        if (collision.CompareTag("Player"))
        {
            dm.isStanding(false);
        }
    }

    #region 미사용 코드
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!menu.GetComponent<Menu_InGame>().InventoryOn && !menu.GetComponent<Menu_InGame>().CancelOn)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (dm.GetDungeonClear())
                    {
                        dm.Teleport();
                    }
                }
            }
        }
    }
    */
    #endregion
}
