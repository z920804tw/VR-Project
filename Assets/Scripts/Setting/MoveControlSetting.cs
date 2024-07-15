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


    [Header("持續選轉設定")]
    public GameObject moveSpeedUI;
    public TMP_Text moveSpeedText;
    public Slider moveSpeedSlider;

    int currentMode;
    void Start()
    {
        currentMode = 0;
    }


    public void SetCurrentMode(int i)   //下拉選單選擇模式
    {
        if (i == 0)
        {
            moveSpeedUI.SetActive(true);
            continuousMove.enabled = false;
            snapMove.enabled = false;

            currentMode = 0;
            Invoke("enableTurnControl", 1f);
        }
        else if (i == 1)
        {
            moveSpeedUI.SetActive(false);
            continuousMove.enabled = false;
            moveSpeedText.enabled = false;

            currentMode = 1;
            Invoke("enableTurnControl", 1f);
        }
    }
    public void ChangeValue(float i)          //持續移動修改移動速度
    {
        if (currentMode == 0)
        {
            continuousMove.moveSpeed= i;
            moveSpeedText.text= i.ToString();
        }
    }

    void enableTurnControl()
    {
        if (currentMode == 0)
        {
            continuousMove.enabled = true;
        }
        else if (currentMode == 1)
        {
            snapMove.enabled = true;
        }
    }
}
