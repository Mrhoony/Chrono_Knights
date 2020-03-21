using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    private Transform cameraTrasform;
    public float[] ParalaxSpeedX;
    public float[] ParalaxSpeedY;
    
    public GameObject mapBase;
    public GameObject spawnerSet;
    public GameObject[] spawner;
    public GameObject teleporter;

    private Transform[] layers;
    private int layerCount;

    private float lastCameraX;
    private float lastCameraY;
    float deltaX;
    float deltaY;

    private void Awake()
    {
        layers = new Transform[transform.childCount - 1];
        Init();
    }

    public void Init()
    {
        cameraTrasform = Camera.main.transform;
        lastCameraX = cameraTrasform.position.x;

        for (int i = 0; i < transform.childCount - 1; ++i)
        {
            layers[i] = transform.GetChild(i);
            layers[i].transform.position = new Vector2(0f, layers[i].transform.position.y);
        }
        layerCount = layers.Length;
    }

    public void SetBackGroundPosition(int currentStage)
    {
        if (-1 == currentStage) return;
        layers[0].transform.position = new Vector2(cameraTrasform.position.x, cameraTrasform.position.y + Random.Range(-2.5f, 2.5f));
       
        if(currentStage < 1)
        {
            for (int i = 1; i < layerCount; ++i)
            {
                layers[i].transform.position = new Vector2(cameraTrasform.position.x, layers[i].transform.position.y + 2f);
            }
        }
        else
        {
            for (int i = 1; i < layerCount; ++i)
            {
                layers[i].transform.position = new Vector2(cameraTrasform.position.x + Random.Range(-0.5f, 0.5f)
                    , layers[i].transform.position.y + Random.Range(-1f, 1f));
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        deltaX = cameraTrasform.position.x - lastCameraX;
        deltaY = cameraTrasform.position.y - lastCameraY;
        
        for(int i = 0; i < layerCount; ++i)
        {
            layers[i].transform.position = new Vector2(layers[i].transform.position.x + deltaX * ParalaxSpeedX[i]
                , layers[i].transform.position.y + deltaY * ParalaxSpeedY[i] * 2f);
        }

        lastCameraX = cameraTrasform.position.x;
        lastCameraY = cameraTrasform.position.y;
    }
}
