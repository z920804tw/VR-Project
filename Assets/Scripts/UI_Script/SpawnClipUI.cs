using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnClipUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Dropdown tMP_Dropdown;
    public Transform spawnTransform;
    public GameObject[] ammoClip;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void BuyClip()
    {
        string currnetSelect = tMP_Dropdown.options[tMP_Dropdown.value].text.ToString();
        switch (currnetSelect)
        {
            case "手槍彈藥":
                Instantiate(ammoClip[0], spawnTransform.position,Quaternion.identity);
            break;
            case "衝鋒槍彈藥":
                Instantiate(ammoClip[1], spawnTransform.position, Quaternion.identity);
            break;
            case"步槍彈藥":
                Instantiate(ammoClip[2],spawnTransform.position, Quaternion.identity);
            break;
            case"霰彈槍彈藥":
                Instantiate(ammoClip[3],spawnTransform.position, Quaternion.identity);
            break;
            default:
            Debug.Log("找不到");
            break;
        }
    }
}
