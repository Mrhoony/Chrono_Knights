using UnityEngine;
using UnityEngine.UI;

public class TownUI_Training : MonoBehaviour
{
    public TownUI townUI;
    public readonly CanvasManager canvasManager = CanvasManager.instance;
    public PlayerStatus playerStatus;
    public PlayerData playerData;
    public GameObject button;
    public GameObject[] traningButton;
    public Image[] gauge;

    public float[] limit_traning;
    public float[] traningStat;
    public int[] traning_count;
    public int focus;
    public bool isTraningPossible;
    public bool isTownMenuOn = false;
    
    public void Update()
    {
        if (!isTownMenuOn || canvasManager.DialogBoxOn()) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["X"]))
        {
            if(focus < 6)
            {
                if (!isTraningPossible) return;
                Traning(focus);
            }
            else
            {
                CloseTownUIMenu();
            }
        }
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Y"]))
        {
            CloseTownUIMenu();
        }

        if (!isTraningPossible) return;

        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Up"])) { FocusedSlot(-1); }
        if (Input.GetKeyDown(KeyBindManager.instance.KeyBinds["Down"])) { FocusedSlot(1); }
    }

    public void OpenTownUIMenu()
    {
        isTownMenuOn = true;

        playerStatus = GameObject.Find("PlayerCharacter").GetComponent<PlayerStatus>();
        playerData = playerStatus.playerData;
        limit_traning = playerData.GetLimitTraning();
        traningStat = playerData.GetTraningStat();
        traning_count = playerData.GetTraningCount();
        
        for (int i = 0; i < 6; ++i)
        {
            gauge[i].fillAmount = traningStat[i] / limit_traning[i];
        }

        isTraningPossible = DungeonManager.instance.GetTrainigPossible();
        if (isTraningPossible)
        {
            button.SetActive(true);
            for (int i = 0; i < 6; ++i)
            {
                if (traningStat[i] >= limit_traning[i])
                {
                    traningButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
                }
                else
                {
                    traningButton[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
            focus = 0;
            traningButton[focus].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
        }
        else
        {
            button.SetActive(false);
            focus = 6;
            traningButton[focus].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
        }
    }
    public void CloseTownUIMenu()
    {
        button.SetActive(false);
        isTownMenuOn = false;
        townUI.CloseTrainingMenu();
    }
    public void Traning(int stat)
    {
        if (traningStat[stat] >= limit_traning[stat]) return;

        if (limit_traning[stat] > 0)
        {
            switch (stat)
            {
                case 0:
                case 1:
                case 5:
                    traningStat[stat] += (1f - traning_count[stat] * 0.1f);
                    break;
                case 2:
                case 3:
                case 4:
                    traningStat[stat] += (0.1f - traning_count[stat] * 0.01f);
                    break;
            }
            if (traningStat[stat] > limit_traning[stat])
                traningStat[stat] = limit_traning[stat];

            ++traning_count[stat];
            if (traning_count[stat] > 5)
                traning_count[stat] = 5;

            gauge[stat].fillAmount = traningStat[stat] / limit_traning[stat];

            playerData.limitTraning = limit_traning;
            playerData.traningStat = traningStat;
            playerData.traning_count = traning_count;
            playerStatus.PlayerStatusUpdate();
            
            traningButton[focus].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            focus = 6;
            traningButton[focus].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);

            button.SetActive(false);
            isTraningPossible = false;
            DungeonManager.instance.setTrainigPossible(false);
        }
    }
    void FocusedSlot(int AdjustValue)
    {
        traningButton[focus].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

        if (focus + AdjustValue < 0) focus = 6;
        else if (focus + AdjustValue > 6) focus = 0;
        else focus += AdjustValue;
        traningButton[focus].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
    }
}
