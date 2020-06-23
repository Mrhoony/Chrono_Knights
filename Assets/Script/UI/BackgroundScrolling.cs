using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    public Transform cameraTrasform;
    public float[] ParalaxSpeedX;
    public float[] ParalaxSpeedY;

    public Transform[] layers;
    public int layerCount;

    public float lastCameraX;
    public float lastCameraY;
    
    private void Awake()
    {
        cameraTrasform = Camera.main.transform;
        gameObject.transform.position = new Vector3(cameraTrasform.position.x, cameraTrasform.position.y + 1f, transform.position.z);

        /*
        layers = new Transform[transform.childCount];
        
        lastCameraX = cameraTrasform.position.x;

        for (int i = 0; i < transform.childCount; ++i)
        {
            layers[i] = transform.GetChild(i);
            layers[i].transform.position = new Vector2(gameObject.transform.position.x, layers[i].transform.position.y);
        }
        layerCount = layers.Length;
        */
    }

    public void SetBackGroundPosition(Vector3 _entrance, int currentStage)
    {
        transform.position = new Vector3(cameraTrasform.position.x, cameraTrasform.position.y + 1f, transform.position.z);
        return;
        /*
        lastCameraX = _entrance.x;
        lastCameraY = _entrance.y;
        
        for (int i = 0; i < layerCount; ++i)
        {
            layers[i].transform.position = new Vector2(_entrance.x + Random.Range(-0.5f, 0.5f), _entrance.y + Random.Range(-1f, 1f));
        }
        */
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(cameraTrasform.position.x, cameraTrasform.position.y + 1f, transform.position.z);
    }
}
