using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestMonsterGuideSet : MonoBehaviour
{
    public RestMonsterGuider[] guider;

    public void SetGuider(int counter, Transform target) {
        if (false == guider[counter].gameObject.activeSelf) guider[counter].gameObject.SetActive(true);
        guider[counter].SetTarget(target);
    }

    public void ResetGuider() {
        for (int i = 0; i < guider.Length; ++i) {
            if (false == guider[i].isActiveAndEnabled) continue;
            guider[i].Reset();
        }
    }
}
