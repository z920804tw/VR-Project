using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectSetting : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("物件設定")]
    public int objectHp;
    public Transform dmgUITransform;
    public GameObject dmgUI;
    public GameObject aa;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (objectHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }




    public void takeDmg(int minDmg, int maxDmg)         //給攻擊動畫用的
    {
        int rndDmg = Random.Range(minDmg, maxDmg);

        Vector3 randomSpread = Random.insideUnitSphere * 0.5f;
        Vector3 pos = dmgUITransform.position + randomSpread;

        GameObject dmg = Instantiate(dmgUI, pos, Quaternion.identity);
        dmg.transform.SetParent(dmgUITransform);
        dmgUI.GetComponentInChildren<TextMeshProUGUI>().text = $"-{rndDmg}";

        dmg.GetComponent<TargetDmgUI>().upSpeed = 0.01f;
        dmgUI.GetComponent<TargetDmgUI>().upTimeLimit = 2;

        objectHp -= rndDmg;
    }
}
