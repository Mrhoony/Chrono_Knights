using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontSetting : MonoBehaviour
{
    public Font[] font;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < font.Length; ++i)
        {
            font[i].material.mainTexture.filterMode = FilterMode.Point;
        }
    }
}
