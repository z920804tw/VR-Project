using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurretSpawnUI : MonoBehaviour
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

    public void BuyTurret()
    {
        string turretName = tMP_Dropdown.options[tMP_Dropdown.value].text;
        switch (turretName)
        {
            case "砲塔A":
                Instantiate(spawnPrefabs[0], spawnTransform.position, Quaternion.identity);
                break;
            default:
                break;
        }
    }
}
