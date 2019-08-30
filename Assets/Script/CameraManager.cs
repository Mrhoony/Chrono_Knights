using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
            Destroy(this);
    }

    void Start()
    {
        bound = GameObject.Find("BackGround").GetComponent<BoxCollider2D>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        camera = GetComponent<Camera>();
        halfHeight = camera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void FixedUpdate()
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

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bound = GameObject.Find("BackGround").GetComponent<BoxCollider2D>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        camera = GetComponent<Camera>();
        halfHeight = camera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    public void OnEnable()
    { SceneManager.sceneLoaded += OnSceneLoaded; }
    public void OnDisable()
    { SceneManager.sceneLoaded -= OnSceneLoaded; }
}
