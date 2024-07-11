using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnClipUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Dropdown tMP_Dropdown;
    public Transform spawnTransform;
    public GameObject rifleClip,smgClip,pistolClip,shotgunClip;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnClip()
    {
        string currnetSelect = tMP_Dropdown.options[tMP_Dropdown.value].text.ToString();
        switch (currnetSelect)
        {
            case "步槍彈藥":
                Instantiate(rifleClip, spawnTransform.position,Quaternion.identity);
            break;
            case "衝鋒槍彈藥":
                Instantiate(smgClip, spawnTransform.position, Quaternion.identity);
            break;
            case"手槍彈藥":
                Instantiate(pistolClip,spawnTransform.position, Quaternion.identity);
            break;
            case"霰彈槍彈藥":
                Instantiate(shotgunClip,spawnTransform.position, Quaternion.identity);
            break;
            default:
            Debug.Log("找不到");
            break;
        }
    }
}
