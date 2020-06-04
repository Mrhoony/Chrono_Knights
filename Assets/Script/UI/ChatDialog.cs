using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatDialog : MonoBehaviour
{
    public Text dialogName;
    public Text dialogText;
    public string completeText;
    ScenarioManager scenarioManager;


    public IEnumerator TempCoroutine;

    public void SetDialogText(string _DialogName, string _DialogText)
    {
        completeText = _DialogText;
        TempCoroutine = OneByOneTextSetting(_DialogName, _DialogText);
        StartCoroutine(TempCoroutine);
    }

    public void SetDialogText(string _DialogName, string _DialogText, ScenarioManager _ScenarioManager)
    {
        scenarioManager = _ScenarioManager;
        completeText = _DialogText;
        TempCoroutine = OneByOneTextSetting(_DialogName, _DialogText);
        StartCoroutine(TempCoroutine);
    }

    public void OneByOneTextSkip()
    {
        StopCoroutine(TempCoroutine);
        dialogText.text = completeText;
    }

    public IEnumerator OneByOneTextSetting(string _NPCName, string _Content)
    {
        string _TempDialogText = "";
        int textCount = _Content.Length;
        dialogName.text = _NPCName;
        for (int i = 0; i < textCount; ++i)
        {
            _TempDialogText += _Content[i];
            dialogText.text = _TempDialogText;
            yield return new WaitForSeconds(0.1f);
        }
        scenarioManager.isOneByOneTextOn = false;
    }
}