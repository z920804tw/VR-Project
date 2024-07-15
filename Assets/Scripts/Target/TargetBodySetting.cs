using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Search;
using UnityEngine;

public enum BodyType
{
    None,
    Body,
    Head,
}
public class TargetBodySetting : MonoBehaviour
{
    // Start is called before the first frame update
    public BodyType bodyType;
    public Transform dmgUITransform;
    public float upTimeLimit;
    public float upSpeed;                       //建議不要太高大概0.01左右

    int rndDmg;

    GameObject currentHitBullet;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            currentHitBullet = other.gameObject;
            switch (other.gameObject.GetComponent<Bullet>().gunType)    //判斷子彈的類型是哪種
            {
                case GunType.Rifle:
                    RifleDmgSetting();
                    break;
                case GunType.Pistol:
                    PistolDmgSetting();
                    break;
                case GunType.SMG:
                    SmgDmgSetting();
                    break;
                case GunType.Shotgun:
                    ShotgunDmgSetting();
                    break;
                default:
                    break;
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Knife")
        {
            KnifeDmgSetting(other.gameObject);


        }
    }


    void takeDmg(int min, int max)
    {
        rndDmg = Random.Range(min, max);                                                    //抓要生成的文字框物件
        GameObject parent = transform.parent.GetComponent<TargetSetting>().dmg_UI;


        Vector3 randomSpread = Random.insideUnitSphere * 0.5f;                              //文字框的生成位置
        Vector3 pos = dmgUITransform.position + randomSpread;

        GameObject dmg = Instantiate(parent, pos, Quaternion.identity);                     //生成文字框出來
        dmg.transform.SetParent(this.transform);
        dmg.GetComponentInChildren<TextMeshProUGUI>().text = $"-{rndDmg}";                  //設定他的文字為受到的隨機傷害值

        transform.parent.GetComponent<TargetSetting>().TargetHP -= rndDmg;                  //扣除本體的總HP


    }

    void RifleDmgSetting()
    {
        int maxDmg = currentHitBullet.GetComponent<Bullet>().MaxDmg;
        int minDmg = currentHitBullet.GetComponent<Bullet>().MinDmg;
        if (this.bodyType == BodyType.Head)
        {
            takeDmg(minDmg + 30, maxDmg + 30);
        }
        else if (this.bodyType == BodyType.Body)
        {
            takeDmg(minDmg, maxDmg);
        }
    }

    void PistolDmgSetting()
    {
        int maxDmg = currentHitBullet.GetComponent<Bullet>().MaxDmg;
        int minDmg = currentHitBullet.GetComponent<Bullet>().MinDmg;
        if (this.bodyType == BodyType.Head)
        {
            takeDmg(minDmg + 30, maxDmg + 30);
        }
        else if (this.bodyType == BodyType.Body)
        {
            takeDmg(minDmg, maxDmg);
        }
    }
    void SmgDmgSetting()
    {
        int maxDmg = currentHitBullet.GetComponent<Bullet>().MaxDmg;
        int minDmg = currentHitBullet.GetComponent<Bullet>().MinDmg;
        if (this.bodyType == BodyType.Head)
        {
            takeDmg(minDmg + 20, maxDmg + 30);
        }
        else if (this.bodyType == BodyType.Body)
        {
            takeDmg(minDmg, maxDmg);
        }
    }
    void ShotgunDmgSetting()
    {
        int maxDmg = currentHitBullet.GetComponent<Bullet>().MaxDmg;
        int minDmg = currentHitBullet.GetComponent<Bullet>().MinDmg;
        if (this.bodyType == BodyType.Head)
        {
            takeDmg(minDmg + 40, maxDmg + 50);
        }
        else if (this.bodyType == BodyType.Body)
        {
            takeDmg(minDmg, maxDmg);
        }
    }

    void KnifeDmgSetting(GameObject other)
    {
        int maxDmg = other.GetComponent<WeaponKnife>().maxDmg;
        int minDmg = other.GetComponent<WeaponKnife>().minDmg;

        if (this.bodyType == BodyType.Head)
        {
            takeDmg(minDmg + 20, maxDmg + 30);
        }
        else if (this.bodyType == BodyType.Body)
        {
            takeDmg(minDmg,maxDmg);
        }
    }
}
