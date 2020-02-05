using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyBindManager : MonoBehaviour
{
    public static KeyBindManager instance;

    public Dictionary<string, KeyCode> KeyBinds { get; set; }
    Dictionary<string, KeyCode> currentDictionary;

    private string bindName;

    public void Start()
    {
        KeyBinds = new Dictionary<string, KeyCode>();
    }

    public void Init()
    {
        BindKey("UP", KeyCode.UpArrow);
        BindKey("DOWN", KeyCode.DownArrow);
        BindKey("LEFT", KeyCode.LeftArrow);
        BindKey("RIGHT", KeyCode.RightArrow);

        BindKey("Fire1", KeyCode.Z);
        BindKey("Fire2", KeyCode.X);
        BindKey("Fire3", KeyCode.V);
        BindKey("Jump", KeyCode.C);
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        currentDictionary = KeyBinds;

        if(keyBind != KeyCode.Escape)
        {
            // currentDictionary의 키에 key 가 없다면
            if (!currentDictionary.ContainsKey(key))
            {
                // 버튼을 추가한다.
                currentDictionary.Add(key, keyBind);

                // 버튼이 이름을 keybind Menu UI에 표시한다.
                //UIManager.MyInstance.UpdateKeyText(key, keyBind);
            }
            else if (currentDictionary.ContainsValue(keyBind))
            {
                // Dictionary의 배열에서 값으로 키를 찾는 방법이다.
                // 같은 값을 지닌 키는 배열 순서가 제일 앞인 녀석이 반환된다.
                string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

                // 변경하려는 키가 이미 사용중이라면
                // 이전에 사용 중이던 키(버튼)을 없앤다.
                currentDictionary[myKey] = KeyCode.None;
                //UIManager.MyInstance.UpdateKeyText(key, itemCode.None);
            }

            // 키를 등록시킨다.
            currentDictionary[key] = keyBind;
            //UIManager.MyInstance.UpdateKeyText(key, keyBind);
            bindName = string.Empty;
        }
    }

    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
        StartCoroutine(KeyInputWating());

    }
    IEnumerator KeyInputWating()
    {
        while (true)
        {
            if (bindName != string.Empty)
            {
                // UnityGUI 에서 발생한 이벤트
                Event e = Event.current;

                // 발생한 이벤트가 키 입력이라면.
                if (e.isKey)
                {
                    BindKey(bindName, e.keyCode);
                    break;
                }
            }
            yield return null;
        }
    }
}
