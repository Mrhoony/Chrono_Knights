using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    public Font[] font;

    static public CameraManager instance;
    public GameObject target;
    private BoxCollider2D bound;

    private Vector3 targetPosition;
    private Vector2 minBound;
    private Vector2 maxBound;

    private int Height;
    private int Width;
    private float halfHeight;
    private float halfWidth;

    private Camera mainCamera;
    private PixelPerfectCamera perfectCamera;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Height = 640;
        Width = 360;

        Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        mainCamera = GetComponent<Camera>();
        perfectCamera = GetComponent<PixelPerfectCamera>();

        GameStartScreenSet();
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
        if (target != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, transform.position.z);
            
            transform.position = Vector3.Lerp(transform.position, targetPosition, target.GetComponent<SubCamera>().moveSpeed * 2f * Time.deltaTime);
            float clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y + halfHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
        
        //targetPosition.Set(target.transform.position.x, target.transform.position.y, transform.position.z);
        //transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }

    public void SetHeiWid(int hei, int wid)
    {
        Height = hei;
        Width = wid;
        GameStartScreenSet();
    }

    public void SetCameraBound(BoxCollider2D box)
    {
        bound = box;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = mainCamera.orthographicSize;
        halfWidth = halfHeight * 1280 / 720;
    }

    public void GameStartScreenSet()
    {
        perfectCamera.refResolutionX = Height;
        perfectCamera.refResolutionY = Width;
    }
}
