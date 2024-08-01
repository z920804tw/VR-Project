using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObstacleSpawnUI : MonoBehaviour
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

    public void BuyObstacle()
    {
        string obstacleName = tMP_Dropdown.options[tMP_Dropdown.value].text;
        switch (obstacleName)
        {
            case "柵欄(小)":
                Instantiate(spawnPrefabs[0], spawnTransform.position, Quaternion.identity);
                break;
            case "柵欄(大)":
                Instantiate(spawnPrefabs[1], spawnTransform.position, Quaternion.identity);
                break;
            case "木箱":
                Instantiate(spawnPrefabs[2], spawnTransform.position, Quaternion.identity);
                break;

            default:
            break;
        }
    }
}
