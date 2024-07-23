using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveControlSetting : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("旋轉控制相關")]
    public ActionBasedContinuousMoveProvider continuousMove;
    public TeleportationProvider snapMove;
    public TMP_Dropdown tD;
    public ActionBasedController actionBasedController;


    [Header("持續選轉設定")]
    public GameObject moveSpeedUI;
    public TMP_Text moveSpeedText;
    public Slider moveSpeedSlider;

    int currentMode;
    void Start()
    {
        SettingMove(PlayerPrefs.GetInt("MoveMode"));   //設定目前的移動模式是甚麼。
        tD.value=PlayerPrefs.GetInt("MoveMode");        
    }


    public void SettingMove(int i)   //下拉選單選擇模式
    {
        if (i == 0)
        {
            moveSpeedUI.SetActive(true);

            currentMode = 0;
            ChangeMoveValue(PlayerPrefs.GetInt("ContinuousMoveValue"));
        }
        else if (i == 1)
        {
            moveSpeedUI.SetActive(false);
            currentMode = 1;
            ChangeMoveValue(-1);
        }
    }
    public void ChangeMoveValue(float value)          //持續、定點移動的修改函式
    {
        if (currentMode == 0)                           //如果currentMode=0就代表是持續移動，那就設定持續移動的相關值。
        {
            continuousMove.moveSpeed = value;
            moveSpeedSlider.value = value;
            moveSpeedText.text = value.ToString();
            actionBasedController.enableInputActions=false;
            PlayerPrefs.SetInt("ContinuousMoveValue", (int)value);
        }
        else if (currentMode == 1)                      //如果==1就設定持續移動的值=0，並且打開移動控制那邊的enableInputActions來啟用定點移動功能。
        {
            continuousMove.moveSpeed = 0;
            actionBasedController.enableInputActions = true;
        }
        PlayerPrefs.SetInt("MoveMode", currentMode);
        PlayerPrefs.Save();
    }


}
