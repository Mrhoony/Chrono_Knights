﻿using System.Collections;
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
        cameraShaking = false;
    }

    public void CameraInit()
    {
        mainCamera = GetComponent<Camera>();
        perfectCamera = GetComponent<PixelPerfectCamera>();

        for (int i = 0; i < font.Length; ++i)
        {
            font[i].material.mainTexture.filterMode = FilterMode.Point;
        }
    }

    public void Init()
    {
        CameraInit();

        CameraSizeSetting(1);
        cameraShakeOnOff = true;

        bound = GameObject.Find("BackGroundSet").GetComponent<BoxCollider2D>();
        SetCameraBound(bound);
    }

    public void Init(bool _CameraShakeOnOff, int _CameraSize)
    {
        CameraInit();

        CameraSizeSetting(_CameraSize);
        //Screen.SetResolution(Screen.width, (Screen.width * 9) / 16, false);

        cameraShakeOnOff = _CameraShakeOnOff;

        bound = GameObject.Find("BackGroundSet").GetComponent<BoxCollider2D>();
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
    public void SetCameraBound(BoxCollider2D box)
    {
        bound = box;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
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
