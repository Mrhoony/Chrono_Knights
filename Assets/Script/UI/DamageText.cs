using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    Color fadeColor;
    public float liveTime = 0f;
    public Text damageText;

    public void Update()
    {
        transform.position += Vector3.up * Time.deltaTime;

        liveTime += Time.deltaTime;
        if (liveTime > 1.5f)
            Destroy(gameObject);

        fadeColor.a -= Time.deltaTime;
        if (fadeColor.a > 1f) fadeColor.a = 1f;
        GetComponent<Text>().color = fadeColor;
    }

    public void SetDamage(int _Damage)
    {
        fadeColor = GetComponent<Text>().color;
        
        damageText.text = _Damage.ToString();
    }
}
