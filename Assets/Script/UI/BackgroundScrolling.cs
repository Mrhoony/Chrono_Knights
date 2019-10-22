using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    private Transform cameraTrasform;
    public float ParalaxSpeed;

    private Transform[] layers;

    private float lastCameraX;
    private float lastCameraY;
    float deltaX;
    float deltaY;

    // Start is called before the first frame update
    void Start()
    {
        cameraTrasform = Camera.main.transform;
        lastCameraX = cameraTrasform.position.x;

        layers = new Transform[transform.childCount - 1];

        for(int i = 0; i < transform.childCount - 1; ++i)
        {
            layers[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        deltaX = cameraTrasform.position.x - lastCameraX;
        deltaY = cameraTrasform.position.y - lastCameraY;

        for(int i = 0; i < layers.Length; ++i)
        {
            layers[i].transform.position = new Vector2(layers[i].transform.position.x + deltaX * ParalaxSpeed * i, layers[i].transform.position.y + deltaY * ParalaxSpeed * i * 0.5f);
        }

        lastCameraX = cameraTrasform.position.x;
        lastCameraY = cameraTrasform.position.y;
    }
}
