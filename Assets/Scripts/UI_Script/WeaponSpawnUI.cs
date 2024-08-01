using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class WeaponSpawnUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Dropdown tMP_Dropdown;
    public TMP_Text moneyText;
    public Transform spawnTransform;
    public GameObject[] spawnPrefabs;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void BuyWeapon()
    {
        string gunName = tMP_Dropdown.options[tMP_Dropdown.value].text;
        switch (gunName)
        {
            case "手槍":
                Instantiate(spawnPrefabs[0], spawnTransform.position, Quaternion.identity);
                break;
            case "衝鋒槍":
                Instantiate(spawnPrefabs[1], spawnTransform.position, Quaternion.identity);
                break;
            case "步槍":
                Instantiate(spawnPrefabs[2], spawnTransform.position, Quaternion.identity);
                break;
            case "霰彈槍":
                Instantiate(spawnPrefabs[3], spawnTransform.position, Quaternion.identity);
                break;

            case "刀":
                Instantiate(spawnPrefabs[4], spawnTransform.position, quaternion.identity);
                break;
            default:
                break;
        }
    }
}
