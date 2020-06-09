using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestMonsterGuider : MonoBehaviour
{ 
    Vector3 targetPos;
    float angle;

    public Transform target;
    public GameObject playerCharacter;
    public float offset;
    public float v;

    public void SetTarget(Transform t) {
        target = t;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (true == target.gameObject.CompareTag("DeadBody")) Reset();
        
        targetPos.x = (target.position.x - playerCharacter.transform.position.x);
        targetPos.y = (target.position.y - playerCharacter.transform.position.y);
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
    }

    public void Reset()
    {
        target = null;
        gameObject.SetActive(false);
    }
}
