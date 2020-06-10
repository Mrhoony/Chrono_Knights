using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    private Transform cameraTrasform;
    public float[] ParalaxSpeedX;
    public float[] ParalaxSpeedY;

    public Transform[] layers;
    public int layerCount;

    public float lastCameraX;
    public float lastCameraY;
    
    private void Awake()
    {
        layers = new Transform[transform.childCount];

        cameraTrasform = Camera.main.transform;
        lastCameraX = cameraTrasform.position.x;

        for (int i = 0; i < transform.childCount; ++i)
        {
            layers[i] = transform.GetChild(i);
            layers[i].transform.position = new Vector2(0f, layers[i].transform.position.y);
        }
        layerCount = layers.Length;
    }

    public void SetBackGroundPosition(Vector2 _entrance, int currentStage)
    {
        if (-1 == currentStage) return;
        lastCameraX = _entrance.x;
        lastCameraY = _entrance.y;
        for (int i = 0; i < layerCount; ++i)
        {
            layers[i].transform.position = new Vector2(_entrance.x + Random.Range(-0.5f, 0.5f), _entrance.y + Random.Range(-1f, 1f));
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (CameraManager.instance.mainScenarioOn) return;

        float deltaX = cameraTrasform.position.x - lastCameraX;
        float deltaY = cameraTrasform.position.y - lastCameraY;
        
        for(int i = 0; i < layerCount; ++i)
        {
            layers[i].transform.position = new Vector2(layers[i].transform.position.x + deltaX * ParalaxSpeedX[i]
                , layers[i].transform.position.y + deltaY * ParalaxSpeedY[i] * 2f);
        }

        lastCameraX = cameraTrasform.position.x;
        lastCameraY = cameraTrasform.position.y;
    }
}
