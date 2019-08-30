using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCamera : MonoBehaviour
{
    static public SubCamera instance;
    public GameObject target;
    public BoxCollider2D boxCollider2D;
    public float moveSpeed;
    public Vector3 targetPosition;

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
        moveSpeed = target.gameObject.GetComponent<PlayerControl>().pStat.moveSpeed;
    }

    public void FixedUpdate()
    {
        if (target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, transform.position.z);

            if (target.transform.position.x > transform.position.x + boxCollider2D.size.x / 4
                || target.transform.position.x < transform.position.x - boxCollider2D.size.x / 4
                || target.transform.position.y > transform.position.y + boxCollider2D.size.y / 4
                || target.transform.position.y < transform.position.y - boxCollider2D.size.y / 4)
                transform.position = Vector3.Lerp(transform.position, targetPosition,
                    (Mathf.Abs(transform.position.x - target.transform.position.x) * 2
                    + Mathf.Abs(transform.position.y - target.transform.position.y)) * Time.deltaTime);
        }
    }
}
