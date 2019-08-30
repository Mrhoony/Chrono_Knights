using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEffector : MonoBehaviour
{
    public PlatformEffector2D platform;
    public float waitTime = 1f;
    public bool isChange = false;

    // Start is called before the first frame update
    void Start()
    {
        platform = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow) && PlayerControl.instance.isDownJump)
        {
            waitTime = 0.15f;
            isChange = true;
        }
        if(isChange)
        {
            if (waitTime > 0)
            {
                platform.rotationalOffset = 180f;
                waitTime -= Time.deltaTime;
            }
            else
            {
                platform.rotationalOffset = 0f;
                isChange = false;
            }
        }
    }
}
