using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : MonoBehaviour
{
    public Font[] font;

    static public CameraManager instance;

    public GameObject target;
    public GameObject currentMap;

    private Vector3 targetPosition;
    private Vector2 minBound;
    private Vector2 maxBound;

    public bool mainScenarioOn;

    public bool cameraShakeOnOff;
    public bool cameraShaking;
    public float cameraZPosition = -50f;
    public float cameraShackForce;
    IEnumerator cameraShake;

    private int Height;
    private int Width;
    private float halfHeight;
    private float halfWidth;

    float clampedX;
    float clampedY;

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
        if (mainScenarioOn) return;
        if (target == null) return;
        
        if (cameraShaking)
        {
            transform.position += Random.insideUnitSphere * cameraShackForce;
        }
        else
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, cameraZPosition);

            transform.position = Vector3.Lerp(transform.position, targetPosition, target.GetComponent<SubCamera>().moveSpeed * 2f * Time.deltaTime);
            clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y + halfHeight);
            transform.position = new Vector3(clampedX, clampedY, cameraZPosition);
        }
    }
    
    public void MainScenarioStart()
    {
        mainScenarioOn = true;
    }

    public void CameraFocus(GameObject _FocusedObject)
    {
        transform.position = new Vector3(_FocusedObject.transform.position.x, _FocusedObject.transform.position.y, cameraZPosition);
        clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y + halfHeight);
        transform.position = new Vector3(clampedX, clampedY, cameraZPosition);

    }
    public void CameraFocus(GameObject _FocusedObject1, GameObject _FocusedObject2)
    {
        transform.position = new Vector3(
            (_FocusedObject1.transform.position.x + _FocusedObject2.transform.position.x) * 0.5f,
            (_FocusedObject1.transform.position.y + _FocusedObject2.transform.position.y) * 0.5f, 
            cameraZPosition);
        clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y + halfHeight);
        transform.position = new Vector3(clampedX, clampedY, cameraZPosition);
    }
    public void CameraFocusOff(float _DelayTime)
    {
        //target = GameObject.Find("SubCamera");
        Invoke("CameraFocusOffDelay", _DelayTime);
    }
    public void CameraFocusOffDelay()
    {
        mainScenarioOn = false;
    }

    public void CameraScroll()
    {
        mainScenarioOn = true;
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

        minBound = box.bounds.min;
        maxBound = box.bounds.max;
        halfHeight = mainCamera.orthographicSize;
        halfWidth = halfHeight * 1280 / 720;
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
