using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState
{
    IsDead,
    NotMove,
    IsJump,
    IsJumpAttack,
    IsAtk,
    IsParrying,
    IsDodge,
    Idle
}
public enum State
{
    common, poison, bleeding, slow
}

public abstract class MovingObject : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;

    public GameObject hitTextBox;

    public SpriteRenderer spriteRenderer;
    public Material defaultMaterial;
    public Material whiteFlashMaterial;

    public string currentState;
    
    public ActionState actionState;

    public int overlap;
    
    public int arrowDirection;

    public void Flip()
    {
        ObjectFlip(gameObject);
        arrowDirection *= -1;
    }
    public void ObjectFlip(GameObject _Object)
    {
        Vector3 scale;
        scale = _Object.transform.localScale;
        scale.x *= -1;
        _Object.transform.localScale = scale;
    }

    public string StateInfo
    {
        get { return currentState; }
        set { currentState = value; }
    }

    public int GetArrowDirection()
    {
        return arrowDirection;
    }

    public ActionState GetActionState()
    {
        return actionState;
    }

    public void SetDamageText(int _Damage)
    {
        GameObject DamageText;
        DamageText = Instantiate(hitTextBox);
        
        DamageText.transform.position = new Vector3(transform.position.x, 
            transform.position.y + GetComponent<BoxCollider2D>().bounds.size.y,
            transform.position.z);

        DamageText.transform.SetParent(GameObject.Find("FloatingTextPool").transform);
        DamageText.GetComponent<DamageText>().SetDamage(_Damage);
        DamageText.SetActive(true);
    }
}
