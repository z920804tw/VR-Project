using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Program_Initialization : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("initialization") == false)
        {
            PlayerPrefs.SetInt("initialization", 1);
            TurnControlSetting();
            MoveControlSetting();


        }
    }

    void TurnControlSetting()
    {
         PlayerPrefs.SetInt("TurnMode", 0);
        PlayerPrefs.SetInt("ContinuousTurnValue", 50);
        PlayerPrefs.SetInt("SnapTurnValue", 35);
    }
    void MoveControlSetting()
    {
        PlayerPrefs.SetInt("MoveMode", 0);
        PlayerPrefs.SetInt("ContinuousMoveValue", 4);
    }
}
