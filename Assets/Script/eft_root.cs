using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eft_root : MonoBehaviour
{
    private float x;
    private float y;
    private float power_x;
    private float power_y;
    private bool isChase;
    private bool isXPositive;
    public float fast;
    public float destroyRange;

    public float randX;
    public float randY;

    CanvasManager CM;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (0 <= Random.Range(-1f, 1f))
        {
            x = Random.Range(-1 * randY, -1 * randX);
            isXPositive = false;
        }
        else {
            x = Random.Range(randX, randY);
            isXPositive = true;
        }

        if (0 <= Random.Range(-1f, 1f)) y = Random.Range(-1 * randY, -1 * randX);
        else y = Random.Range(randX, randY);
        
        power_x = -1 * x;
        power_y = -1 * y;
        isChase = false;

        player = GameObject.Find("PlayerCharacter");
        CM = GameObject.Find("UI").GetComponent<CanvasManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        shoot();
        chase();    
    }

    public void shoot() 
    {
        if (true == isChase) return;
        transform.Translate(x * Time.deltaTime, y * Time.deltaTime, 0);

        x += power_x * Time.deltaTime * fast;
        y += power_y * Time.deltaTime * fast;

        if (true == isXPositive && 0 >= x)
        {
            isChase = true;
            x = 0;
            y = 0;
        }
        else if (false == isXPositive && 0 <= x)
        {
            isChase = true;
            x = 0;
            y = 0;
        }
    }

    public void chase()
    {
        if (false == isChase) return;
        
        power_x = player.transform.position.x - transform.position.x;
        power_y = player.transform.position.y - transform.position.y;

        if ((power_x * power_x <= destroyRange * destroyRange) && (power_y * power_y <= destroyRange * destroyRange))
        {
            CM.RootBagUI();
            Destroy(this.gameObject);
        }
        transform.Translate((power_x * Time.deltaTime * fast), (power_y * Time.deltaTime * fast), 0);        
    }
}
