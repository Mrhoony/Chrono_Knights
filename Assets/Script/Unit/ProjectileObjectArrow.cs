using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObjectArrow : MonoBehaviour
{
    public bool isHit;
    public Rigidbody2D rb;
    public GameObject parentObject;

    public void Init(GameObject _parentObject)
    {
        isHit = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        parentObject = _parentObject;
    }

    private void Update()
    {
        if (isHit) return;

        transform.localEulerAngles = new Vector3(0, 0, rb.velocity.y * 10f / rb.velocity.x);
    }

    public void arrowShooting(float distance)
    {
        isHit = false;
        rb.gravityScale = 1;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(distance * 2.5f, 4f), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit) return;

        if(collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                isHit = true;
                collision.gameObject.GetComponent<IsDamageable>().PlayerHit(parentObject.GetComponentInParent<EnemyStatus>().GetAttack());
                gameObject.SetActive(false);
            }
            else if (collision.CompareTag("Ground"))
            {
                isHit = true;
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
        }
    }
}
