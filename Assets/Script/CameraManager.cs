using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;
    public GameObject target;
    public BoxCollider2D bound;

    public Vector3 targetPosition;

    public Vector2 minBound;
    public Vector2 maxBound;
    
    public float halfWidth;
    public float halfHeight;

    public new Camera camera;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1280, 720, true);
    }

    void Start()
    {
        bound = GameObject.Find("BackGround").GetComponent<BoxCollider2D>();
        SetCameraBound(bound);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, transform.position.z);

            transform.position = targetPosition;
            transform.position = Vector3.Lerp(transform.position, targetPosition, target.GetComponent<SubCamera>().moveSpeed/2 * Time.deltaTime);
            float clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y + halfHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }

    public void SetCameraBound(BoxCollider2D box)
    {
        bound = box;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        camera = GetComponent<Camera>();
        halfHeight = camera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }
}
