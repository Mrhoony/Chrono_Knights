using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    public Font[] font;

    static public CameraManager instance;

    public GameObject target;
    public float targetMoveSpeed;

    public bool cameraScrollOn;
    public GameObject scrollStart;
    public GameObject scrollEnd;

    public GameObject subCamera;
    public GameObject currentMap;

    private Vector3 targetPosition;

    public float cameraMinX;
    public float cameraMaxX;
    public float cameraMinY;
    public float cameraMaxY;

    public bool mainScenarioOn;

    public bool cameraShakeOnOff;
    public bool cameraShaking;
    public float cameraZPosition = -50f;
    public float cameraShackForce;
    IEnumerator cameraShake;

    private int Height;
    private int Width;

    public float clampedX;
    public float clampedY;

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
        mainScenarioOn = false;
        cameraShaking = false;

        mainCamera = GetComponent<Camera>();
        perfectCamera = GetComponent<PixelPerfectCamera>();

        for (int i = 0; i < font.Length; ++i)
        {
            font[i].material.mainTexture.filterMode = FilterMode.Point;
        }
    }

    public void Init()
    {
        CameraSizeSetting(1);
        cameraShakeOnOff = true;

        currentMap = GameObject.Find("Base");
        SetCameraBound(currentMap);
    }
    public void Init(bool _CameraShakeOnOff, int _CameraSize)
    {
        CameraSizeSetting(_CameraSize);
        //Screen.SetResolution(Screen.width, (Screen.width * 9) / 16, false);

        cameraShakeOnOff = _CameraShakeOnOff;

        currentMap = GameObject.Find("Base");
        SetCameraBound(currentMap);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (mainScenarioOn)
        {
            if (cameraScrollOn)
            {
                if (scrollStart == null || scrollEnd == null) return;

                targetPosition.Set(scrollEnd.transform.position.x, scrollEnd.transform.position.y, cameraZPosition);

                transform.position = Vector3.Lerp(transform.position, targetPosition, targetMoveSpeed * Time.deltaTime);
                clampedX = Mathf.Clamp(transform.position.x, cameraMinX, cameraMaxX);
                clampedY = Mathf.Clamp(transform.position.y, cameraMinY, cameraMaxY);
                transform.position = new Vector3(clampedX, clampedY, cameraZPosition);

            }
            else
            {
                if (target == null) return;

                targetPosition.Set(target.transform.position.x, target.transform.position.y, cameraZPosition);

                transform.position = Vector3.Lerp(transform.position, targetPosition, targetMoveSpeed * 2f * Time.deltaTime);
                clampedX = Mathf.Clamp(transform.position.x, cameraMinX, cameraMaxX);
                clampedY = Mathf.Clamp(transform.position.y, cameraMinY, cameraMaxY);
                transform.position = new Vector3(clampedX, clampedY, cameraZPosition);
            }
        }
        else
        {
            if (subCamera == null) return;

            if (cameraShaking)
            {
                transform.position += Random.insideUnitSphere * cameraShackForce;
            }
            else
            {
                targetPosition.Set(subCamera.transform.position.x, subCamera.transform.position.y, cameraZPosition);

                transform.position = Vector3.Lerp(transform.position, targetPosition, subCamera.GetComponent<SubCamera>().moveSpeed * 2f * Time.deltaTime);
                clampedX = Mathf.Clamp(transform.position.x, cameraMinX, cameraMaxX);
                clampedY = Mathf.Clamp(transform.position.y, cameraMinY, cameraMaxY);
                transform.position = new Vector3(clampedX, clampedY, cameraZPosition);
            }
        }
    }
    
    public void MainScenarioStart()
    {
        mainScenarioOn = true;
    }
    public void CameraFocus(GameObject _FocusedObject)
    {
        cameraScrollOn = false;
        targetMoveSpeed = 2f;
        target = _FocusedObject;
    }
    public void CameraScroll(GameObject _StartPoint, GameObject _EndPoint, float _ScrollMoveSpeed)
    {
        targetMoveSpeed = _ScrollMoveSpeed;
        target = _StartPoint;
        scrollStart = _StartPoint;
        scrollEnd = _EndPoint;
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, cameraZPosition);
        cameraScrollOn = true;
    }
    public void CameraFocusOff(float _DelayTime)
    {
        //target = GameObject.Find("SubCamera");
        cameraScrollOn = false;
        targetMoveSpeed = 2f;
        target = null;
        scrollStart = null;
        scrollEnd = null;
        Invoke("CameraFocusOffDelay", _DelayTime);
    }

    public void CameraFocusOffDelay()
    {
        mainScenarioOn = false;
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
    public void SetCameraBound(GameObject _CurrentMap)
    {
        currentMap = _CurrentMap;
        BoxCollider2D box = currentMap.GetComponent<BoxCollider2D>();
        Vector2 minBound;
        Vector2 maxBound;
        float halfHeight;
        float halfWidth;

        minBound = box.bounds.min;
        maxBound = box.bounds.max;

        halfHeight = mainCamera.orthographicSize;
        halfWidth = halfHeight * 1280 / 720;

        cameraMinX = minBound.x + halfWidth;
        cameraMaxX = maxBound.x - halfWidth;
        cameraMinY = minBound.y + halfHeight;
        cameraMaxY = maxBound.y - halfHeight;
    }
    public void SetCameraPosition(Vector3 _Position)
    {
        transform.position = new Vector3(_Position.x, _Position.y, cameraZPosition);
    }

    public void CameraSizeSetting(int _CameraSize)
    {
        switch (_CameraSize)
        {
            case 1:
                Height = 640;
                Width = 360;
                break;
            case 2:
                perfectCamera.assetsPPU = 100;
                Height = 1280;
                Width = 720;
                break;
            default:
                perfectCamera.assetsPPU = 100;
                Height = 1280;
                Width = 720;
                break;
        }
        GameStartScreenSet();
    }
    public void GameStartScreenSet()
    {
        perfectCamera.refResolutionX = Height;
        perfectCamera.refResolutionY = Width;
    }
}
