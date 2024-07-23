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
    public TMP_Dropdown tD;


    [Header("持續選轉設定")]
    public GameObject turnSpeedUI;
    public TMP_Text turnSpeedText;
    public Slider turnSpeedSlider;
    [Header("定點選轉設定")]
    public GameObject turnAngleUI;
    public TMP_Text turnAngelText;
    public Slider turnAngleSlider;

    int currentMode;


    private void Start()    //預設當前旋轉模式為最後設定的模式。
    {
        SettingTurn(PlayerPrefs.GetInt("TurnMode"));
        tD.value = PlayerPrefs.GetInt("TurnMode");

    }
    public void SettingTurn(int i)      //設定旋轉選單
    {
        if (i == 0)                     //當傳入的值=0時，代表持續旋轉。會將持續選轉的UI打開，角度選轉的UI關閉。
        {
            turnSpeedUI.SetActive(true);
            turnAngleUI.SetActive(false);

            currentMode = 0;            //設定當前模式為持續旋轉。
            ChangeTurnValue(PlayerPrefs.GetInt("ContinuousTurnValue"));
        }
        else if (i == 1)                //當傳入的值=1時，代表角度旋轉。會將角度旋轉的UI打開，持續旋轉的UI關閉
        {
            turnSpeedUI.SetActive(false);
            turnAngleUI.SetActive(true);
            currentMode = 1;
            ChangeTurnValue(PlayerPrefs.GetInt("SnapTurnValue"));
        }
    }

    public void ChangeTurnValue(float value)        //改變旋轉Slider值時，就會執行以下功能，並儲存數值
    {
        if (currentMode == 0)
        {
            continuousTurn.turnSpeed = value;
            turnSpeedSlider.value = value;
            turnSpeedText.text = value.ToString();

            snapTurn.turnAmount = 0;

            PlayerPrefs.SetInt("ContinuousTurnValue", (int)value);
        }
        else if (currentMode == 1)
        {
            snapTurn.turnAmount = value;
            turnAngleSlider.value = value;
            turnAngelText.text = value.ToString();

            continuousTurn.turnSpeed = 0;

            PlayerPrefs.SetInt("SnapTurnValue", (int)value);

        }
        PlayerPrefs.SetInt("TurnMode", currentMode);
        PlayerPrefs.Save();
    }
}