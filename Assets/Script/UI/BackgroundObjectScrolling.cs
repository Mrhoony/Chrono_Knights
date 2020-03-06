using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObjectScrolling : MonoBehaviour
{
    Vector2 spriteSize;
    GameObject backGround;
    Collider2D backGroundCollider;
    float randomMoveSpeed;
    public bool updown;
    public float updownCount;

    private void OnEnable()
    {
        spriteSize = GetComponent<SpriteRenderer>().sprite.rect.size;
        backGround = GameObject.Find("BackGround");
        backGroundCollider = backGround.GetComponent<Collider2D>();
        randomMoveSpeed = Random.Range(0.001f, 0.002f);
        updownCount = 0.001f;
        StartCoroutine(updownRandom());
    }

    // Update is called once per frame
    void Update()
    {
        if (updown)
        {
            transform.Translate(new Vector3(0f, updownCount, 0f));
        }
        else
        {
            transform.Translate(new Vector3(randomMoveSpeed, 0f, 0f));
            if (transform.position.x > (backGroundCollider.bounds.size.x * 0.5f) + (spriteSize.x * 0.01f))
            {
                transform.position *= new Vector2(-1f, 1f);
            }
        }
    }

    IEnumerator updownRandom()
    {
        yield return new WaitForSeconds(3f);

        updownCount *= -1f;
        StartCoroutine(updownRandom());
    }
}
