using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
public class TurnControlSetting : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("旋轉控制相關")]
    public ActionBasedContinuousTurnProvider continuousTurn;
    public ActionBasedSnapTurnProvider snapTurn;


    [Header("持續選轉設定")]
    public GameObject turnSpeedUI;
    public TMP_Text turnSpeedText;
    public Slider turnSpeedSlider;
    [Header("定點選轉設定")]
    public GameObject turnAngleUI;
    public TMP_Text turnAngelText;
    public Slider turnAngleSlider;

    int currentMode;


    private void Start()
    {
        currentMode = 0;

    }
    public void SettingTurn(int i)
    {
        if (i == 0)
        {
            turnSpeedUI.SetActive(true);
            turnAngleUI.SetActive(false);

            continuousTurn.enabled = false;
            snapTurn.enabled = false;
            Invoke("enableTurnControl", 1f);
            currentMode = 0;

        }
        else if (i == 1)
        {
            turnSpeedUI.SetActive(false);
            turnAngleUI.SetActive(true);

            continuousTurn.enabled = false;
            snapTurn.enabled = true;

            currentMode = 1;
        }
    }

    public void ChangeTurnValue(float value)
    {
        if (currentMode == 0)
        {
            continuousTurn.turnSpeed = value;
            turnSpeedText.text = value.ToString();
        }
        else if (currentMode == 1)
        {
            snapTurn.turnAmount = value;
            turnAngelText.text = value.ToString();
        }
    }

    void enableTurnControl()
    {
        if (currentMode == 0)
        {
            continuousTurn.enabled = true;
            snapTurn.enabled = false;
        }
        else if (currentMode == 1)
        {
            continuousTurn.enabled=false;
            snapTurn.enabled=true;  
        }
    }

}
