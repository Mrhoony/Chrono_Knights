using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    public Font[] font;

    static public CameraManager instance;
    public GameObject target;
    public BoxCollider2D bound;

    public Vector3 targetPosition;

    public Vector2 minBound;
    public Vector2 maxBound;
    
    public float halfWidth;
    public float halfHeight;

    public new Camera camera;
    public PixelPerfectCamera perfectCamera;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        camera = GetComponent<Camera>();

        perfectCamera = GetComponent<PixelPerfectCamera>();
    }

    void Start()
    {
        for (int i = 0; i < font.Length; ++i)
        {
            font[i].material.mainTexture.filterMode = FilterMode.Point;
        }

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
        halfHeight = camera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    public void GameStartScreenSet()
    {
        perfectCamera.refResolutionX = 1280;
        perfectCamera.refResolutionY = 720;
    }
}
