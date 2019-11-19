using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public GameObject SettingsMenu;
    public GameObject LoadSlot;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OpenLoad()
    {
        LoadSlot.SetActive(true);
    }

    public void CloseLoad()
    {
        LoadSlot.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Quit"); // Application.Quit()은 에디터 상에서 작동x로 Debug.log로 동작 확인, 빌드시 삭제
        //Application.Quit();
    }
}
