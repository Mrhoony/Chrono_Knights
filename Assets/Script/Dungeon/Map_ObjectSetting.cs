using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_ObjectSetting : MonoBehaviour
{
    public GameObject[] activateObject;
    public GameObject lightObject;

    public void SaveGame()
    {
        foreach (GameObject _Object in activateObject)
        {
            _Object.SetActive(true);
        }
        lightObject.GetComponent<Light>().intensity = 0.5f;
    }
    public void LoadGame()
    {
        foreach (GameObject _Object in activateObject)
        {
            _Object.SetActive(false);
        }
        lightObject.GetComponent<Light>().intensity = 1.5f;
    }
    public void LightintensitySetting(float _Intensity)
    {
        lightObject.GetComponent<Light>().intensity = _Intensity;
    }
}
