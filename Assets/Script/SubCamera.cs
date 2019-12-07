using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCamera : MonoBehaviour
{
    static public SubCamera instance;
    public GameObject target;
    private BoxCollider2D boxCollider2D;
    public float moveSpeed;
    private Vector3 targetPosition;

    float tempX;
    float tempY;

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
    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.transform.position;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void LateUpdate()
    {
        if (target.gameObject != null)
        {
            moveSpeed = target.gameObject.GetComponent<PlayerControl>().playerStatus.moveSpeed;
            targetPosition.Set(target.transform.position.x, target.transform.position.y, transform.position.z);

            tempX = transform.position.x - target.transform.position.x;
            tempY = transform.position.y - target.transform.position.y;
            if (tempX < 0) tempX *= -1;
            if (tempY < 0) tempY *= -1;

            if (target.transform.position.x > transform.position.x + boxCollider2D.size.x / 4
                || target.transform.position.x < transform.position.x - boxCollider2D.size.x / 4
                || target.transform.position.y > transform.position.y + boxCollider2D.size.y / 4
                || target.transform.position.y < transform.position.y - boxCollider2D.size.y / 4)
                transform.position = Vector3.Lerp(transform.position, targetPosition, (tempX * 2 + tempY) * Time.deltaTime);
        }
    }
}
