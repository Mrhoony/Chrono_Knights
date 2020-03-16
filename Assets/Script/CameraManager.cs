using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    public Font[] font;

    static public CameraManager instance;
    public GameObject target;
    public BoxCollider2D bound;

    private Vector3 targetPosition;
    private Vector2 minBound;
    private Vector2 maxBound;

    public bool cameraShakeOnOff;
    public bool cameraShaking;
    public float cameraZPosition = -50f;
    public float cameraShackForce;
    IEnumerator cameraShake;

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
        cameraShakeOnOff = true;
        cameraShaking = false;

        GameStartScreenSet();
    }

    public void CameraSetting(bool _cameraSetting)
    {
        cameraShakeOnOff = _cameraSetting;
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
            if (cameraShaking)
            {
                transform.position += Random.insideUnitSphere * cameraShackForce;
            }
            else
            {
                targetPosition.Set(target.transform.position.x, target.transform.position.y, cameraZPosition);

                transform.position = Vector3.Lerp(transform.position, targetPosition, target.GetComponent<SubCamera>().moveSpeed * 2f * Time.deltaTime);
                float clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
                float clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y + halfHeight);
                transform.position = new Vector3(clampedX, clampedY, cameraZPosition);
            }
        }
    }

    public void CameraShake(int _cameraShackForce)
    {
        if (cameraShakeOnOff)
        {
            if (_cameraShackForce > 3) _cameraShackForce = 3;
            cameraShackForce = _cameraShackForce * 0.03f;
            StopCoroutine("CameraShackTime");
            cameraShake = CameraShackTime(_cameraShackForce * 0.1f);
            StartCoroutine(cameraShake);
        }
    }
    public IEnumerator CameraShackTime(float _time)
    {
        cameraShaking = true;
        yield return new WaitForSeconds(_time);
        cameraShaking = false;
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
