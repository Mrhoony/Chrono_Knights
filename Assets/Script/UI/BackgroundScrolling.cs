using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    private Transform cameraTrasform;
    public float[] ParalaxSpeed;

    public GameObject backGroundImage;
    public GameObject mapBase;
    public GameObject spawnerSet;
    public GameObject[] spawner;
    public GameObject teleporter;

    private Transform[] layers;

    private float lastCameraX;
    private float lastCameraY;
    float deltaX;
    float deltaY;

    private void Awake()
    {
        layers = new Transform[transform.childCount - 1];
        spawner = new GameObject[spawnerSet.transform.childCount];
        for(int i = 0; i<spawnerSet.transform.childCount; ++i)
        {
            
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        cameraTrasform = Camera.main.transform;
        lastCameraX = cameraTrasform.position.x;

        for(int i = 0; i < transform.childCount - 1; ++i)
        {
            layers[i] = transform.GetChild(i);
            layers[i].transform.position = new Vector2(0f, layers[i].transform.position.y);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        deltaX = cameraTrasform.position.x - lastCameraX;
        deltaY = cameraTrasform.position.y - lastCameraY;

        for(int i = 0; i < layers.Length; ++i)
        {
            layers[i].transform.position = new Vector2(layers[i].transform.position.x + deltaX * ParalaxSpeed[i], layers[i].transform.position.y + deltaY * ParalaxSpeed[i] * 2f);
        }

        lastCameraX = cameraTrasform.position.x;
        lastCameraY = cameraTrasform.position.y;
    }
}
