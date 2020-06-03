using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum KeyBindsName
{
    Up = 0, Down, Left, Right, X, Y, Dodge, Jump, ActiveSkill, InventoryOpen, QuickSlotLeft, QuickSlotSelect, QuickSlotRight, WeaponSwap
}

public class KeyBindManager : MonoBehaviour
{
    public static KeyBindManager instance;
    public SystemUI_KeySettingMenu keySettingMenu;
    public bool isKeySettingOn;
    public Dictionary<string, KeyCode> KeyBinds { get; set; }

    private string bindName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log("Key bind manager awake");
        isKeySettingOn = false;
    }
    public void Init()
    {
        KeyBinds = new Dictionary<string, KeyCode>();

        BindKey("Up", KeyCode.UpArrow);
        BindKey("Down", KeyCode.DownArrow);
        BindKey("Left", KeyCode.LeftArrow);
        BindKey("Right", KeyCode.RightArrow);

        BindKey("X", KeyCode.Z);
        BindKey("Y", KeyCode.X);
        BindKey("Dodge", KeyCode.Space);
        BindKey("Jump", KeyCode.C);
        BindKey("ActiveSkill", KeyCode.S);

        BindKey("QuickSlotLeft", KeyCode.Q);
        BindKey("InventoryOpen", KeyCode.I);
        BindKey("QuickSlotSelect", KeyCode.W);
        BindKey("QuickSlotRight", KeyCode.E);

        Debug.Log("Key Init");
    }

    public void Init(Dictionary<string, KeyCode> _SavedDictionary)
    {
        for (int i = 0; i < KeyBinds.Count; ++i)
        {
            if (!KeyBinds.ContainsKey(((KeyBindsName)i).ToString()))
            {
                Init();
                Debug.Log("Key setting Init");
                return;
            }
        }
        KeyBinds = _SavedDictionary;
        Debug.Log("Key setting");
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary;

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

        KeyBinds = currentDictionary;
    }

    public void KeyBindSelect(string bindName, SystemUI_KeySettingMenu _KeySettingMenu)
    {
        this.bindName = bindName;
        keySettingMenu = _KeySettingMenu;
        Invoke("KeyInputDelay", 0.2f);
    }

    public void KeyInputDelay()
    {
        isKeySettingOn = true;
    }

    public void OnGUI()
    {
        if (!isKeySettingOn) return;

        if (bindName != string.Empty)
        {
            // UnityGUI 에서 발생한 이벤트
            Event e = Event.current;

            if (e != null)
            {
                Debug.Log("keyInput + " + e.keyCode);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isKeySettingOn = false;
                    keySettingMenu.KeySettingEnd();

                    Debug.Log("setting cancel");
                }
                // 발생한 이벤트가 키 입력이라면.
                if (e.isKey)
                {
                    BindKey(bindName, e.keyCode);
                    isKeySettingOn = false;
                    keySettingMenu.KeySettingEnd();

                    Debug.Log(bindName + " setting " + e.keyCode);
                }
            }
        }
    }
}
