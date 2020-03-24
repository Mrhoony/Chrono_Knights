using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObjectArrow : MonoBehaviour
{
    public bool isHit;
    public Rigidbody2D rb;
    public int damage;
    
    private void Update()
    {
        if (isHit) return;

        transform.localEulerAngles = new Vector3(0, 0, rb.velocity.y * 45f / rb.velocity.x);
    }

    public void arrowShooting(int _damage, float _distance, int _arrowDirection)
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        isHit = false;
        damage = _damage;

        Vector2 scale = transform.localScale;
        scale.x *= _arrowDirection;
        transform.localScale = scale;

        rb.gravityScale = _distance / 5f;

        rb.AddForce(new Vector2(_arrowDirection * 3f, 2f), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit) return;

        if(collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                isHit = true;
                collision.gameObject.GetComponent<IsDamageable>().Hit(damage, 0);
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
