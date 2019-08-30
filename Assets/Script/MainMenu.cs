using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject SettingsMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("Town"); // 다음 씬 입력
    }

    public void ExitGame()
    {
        Debug.Log("Quit"); // Application.Quit()은 에디터 상에서 작동x로 Debug.log로 동작 확인, 빌드시 삭제
        Application.Quit();
    }

    public void OpenSettings()
    {
        SettingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        SettingsMenu.SetActive(false);
    }
}
