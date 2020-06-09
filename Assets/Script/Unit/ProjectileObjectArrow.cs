using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObjectArrow : MonoBehaviour
{
    public bool isHit;
    public Rigidbody2D rb;
    public int damage;
    public float angle;
    
    private void Update()
    {
        if (isHit) return;

        angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void arrowShooting(int _Damage, float _DistanceX, float _DistanceY, int _ArrowDirection)
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        isHit = false;
        damage = _Damage;

        Vector2 scale = transform.localScale;
        scale.x *= _ArrowDirection;
        transform.localScale = scale;
        if(_DistanceY != 0)
        {
            rb.AddForce(new Vector2(_ArrowDirection * _DistanceX, 1/ _DistanceY), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(new Vector2(_ArrowDirection * _DistanceX, 4f), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit) return;

        if(collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                isHit = true;
                collision.gameObject.GetComponent<PlayerControl>().Hit(damage);
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
