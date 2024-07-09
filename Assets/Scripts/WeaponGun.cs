using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
public enum GunType
{
    None,
    Rifle,
    Shotgun,
    Sniper,
    Pistol,
}
public class WeaponGun : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("槍枝設定")]
    public GunType gunType;
    public Transform firePos;
    public Transform fireRay;
    [SerializeField] GameObject bullet;
    [SerializeField] int maxBullet;
    [SerializeField] LayerMask layerMask;
    int currentBullet;


    [SerializeField] float fireDelay;
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxRayDistance;

    [Header("雜項設定")]
    public LineRenderer lineRenderer;
    public GameObject shootEffect;
    public GameObject bulletHole;
    public GameObject GunUI;
    public TextMeshProUGUI ammoText;

    Quaternion rotate;

    [Header("槍枝音效")]
    public AudioClip shootClip;
    public AudioClip emptyShot;
    AudioSource audioSource;

    [Header("Debug")]

    public bool useLineRay;

    public bool isHolding;
    float timer;

    [HideInInspector]
    public Vector3 bh_Point;               //給計算彈孔的方向用
    [HideInInspector]
    public Vector3 hitPoint;





    void Start()
    {
        isHolding = false;
        useLineRay = false;
        currentBullet = maxBullet;
        timer = fireDelay; //讓使用者剛按下去時就能馬上開槍
        lineRenderer.enabled = false;
        GunUI.SetActive(false);
        UpdateUiText();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            ShowGunRay();
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (isHolding && gunType == GunType.Rifle)  //只有連射武器會需要用到，如果是一般單發射擊不用用到。
        {
            if (other.gameObject.tag == "Hand")
            {

                float triggerInput = other.GetComponent<HandInput>().HandTrigger.action.ReadValue<float>();
                if (triggerInput >= 1)
                {
                    ContinueShoot();
                }
            }
        }
        if (other.gameObject.tag == "Clip")     //裝彈藥123
        {
            if (currentBullet <= 0)
            {
                currentBullet = maxBullet;
                UpdateUiText();
                Destroy(other.gameObject);
            }
        }
    }
    void ShowGunRay()
    {
        if (useLineRay)
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }

        Ray ray = new Ray(fireRay.position, fireRay.forward);        //槍枝雷射用的
        RaycastHit hit;
        lineRenderer.SetPosition(0, fireRay.position);
        if (Physics.Raycast(ray, out hit, maxRayDistance, layerMask))
        {
            lineRenderer.SetPosition(1, hit.point);
            bh_Point = hit.normal;              //給計算彈孔的方向用
            hitPoint = hit.point;                 //紀錄hit的點
        }
        else
        {
            lineRenderer.SetPosition(1, fireRay.position + fireRay.forward * maxRayDistance);
            hitPoint = new Vector3(999, 999, 999);                                                  //如果射線沒有打到東西就設定這個值，代表沒有\打到東西
            //hitPoint=fireRay.position+fireRay.forward * maxRayDistance;
        }

    }
    void UpdateUiText()
    {
        ammoText.text = $"{currentBullet}/{maxBullet}";

    }

    void ContinueShoot()    //連射武器用的，例如步槍
    {
        if (currentBullet > 0)
        {
            timer += Time.deltaTime;
            if (timer > fireDelay)
            {
                timer = 0;
                GameObject bb = Instantiate(bullet, firePos.position, Quaternion.identity);
                bb.GetComponent<Rigidbody>().AddForce(firePos.forward * bulletSpeed, ForceMode.Impulse);
                Destroy(bb, 2f);

                GameObject effect = Instantiate(shootEffect, firePos.position, Quaternion.identity);
                effect.transform.SetParent(firePos);
                effect.transform.localEulerAngles = new Vector3(0, -90, 0);
                audioSource.PlayOneShot(shootClip);
                Destroy(effect, 0.5f);


                if (hitPoint != new Vector3(999, 999, 999))
                {
                    rotate = bulletHoleRotate(bh_Point);
                    GameObject bh = Instantiate(bulletHole, hitPoint, rotate);
                    Destroy(bh, 10f);
                }


                currentBullet--;
                UpdateUiText();
            }
        }
        else if (currentBullet <= 0)
        {
            timer += Time.deltaTime;
            if (timer > fireDelay)
            {
                timer = 0;
                audioSource.PlayOneShot(emptyShot);
            }

        }

    }

    public void PistolShoot()         //手槍用
    {
        if (currentBullet > 0)
        {
            GameObject bb = Instantiate(bullet, firePos.position, Quaternion.identity);                                     //產生子彈
            bb.GetComponent<Rigidbody>().AddForce(firePos.forward * bulletSpeed, ForceMode.Impulse);
            Destroy(bb, 2f);

            GameObject effect = Instantiate(shootEffect, firePos.position, Quaternion.identity);                            //產生開火特效和音效
            effect.transform.SetParent(firePos);
            effect.transform.localEulerAngles = new Vector3(0, -90, 0);
            audioSource.PlayOneShot(shootClip);
            Destroy(effect, 0.5f);

            if (hitPoint != new Vector3(999, 999, 999))
            {
                rotate = bulletHoleRotate(bh_Point);

                GameObject bh = Instantiate(bulletHole, hitPoint, rotate);
                Destroy(bh, 10f);
            }

            currentBullet--;
            UpdateUiText();
        }
        else if (currentBullet <= 0)
        {
            audioSource.PlayOneShot(emptyShot);

        }

    }
    public void ShotGunShoot()        //霰彈槍射擊用
    {
        if (currentBullet > 0)
        {
            for (int i = 0; i < 10; i++)                                                //這邊會跑一個for迴圈，這樣可以一次射出10發子彈
            {
                Vector3 randomSpread = UnityEngine.Random.insideUnitSphere * 0.1f;     //Random.insideUnitSphere 可以隨機產生一個方向偏移量
                Vector3 dir = firePos.forward + randomSpread;

                Ray sgRay = new Ray(firePos.position, dir);                            //這邊會用射線來顯示彈孔的生成位置
                RaycastHit sgHit;
                Vector3 sHit;       //紀錄normal
                Vector3 sHit2;      //紀錄點
                if (Physics.Raycast(sgRay, out sgHit, maxRayDistance, layerMask))
                {
                    sHit = sgHit.normal;                                                //射線命中的向量點
                    sHit2 = sgHit.point;                                                //設限命中的點
                    rotate = bulletHoleRotate(sHit);                                    //會去使用這個含式來看彈孔要怎麼顯示(旋轉)到正確位置
                    GameObject bh = Instantiate(bulletHole, sHit2, rotate);             //生成彈孔在雷射打到的位置上
                    Destroy(bh, 10f);                                                   //設定彈孔殘留時間
                }


                GameObject bb = Instantiate(bullet, firePos.position, Quaternion.identity); //生成子彈
                bb.GetComponent<Rigidbody>().AddForce(dir * bulletSpeed, ForceMode.Impulse);//給子彈推力，方向為上面的隨機dir方向
                Destroy(bb, 2f);

            }

            GameObject effect = Instantiate(shootEffect, firePos.position, Quaternion.identity);                            //產生開火特效和音效
            effect.transform.SetParent(firePos);
            effect.transform.localEulerAngles = new Vector3(0, -90, 0);
            audioSource.PlayOneShot(shootClip);
            Destroy(effect, 0.5f);


            currentBullet--;
            UpdateUiText();
        }
        else if (currentBullet <= 0)
        {
            audioSource.PlayOneShot(emptyShot);
        }

    }
    public void Holding()
    {
        isHolding = true;
        useLineRay = true;
        GunUI.SetActive(true);

    }
    public void Reset()
    {
        isHolding = false;
        useLineRay = false;
        lineRenderer.enabled = false;
        GunUI.SetActive(false);
    }



    Quaternion bulletHoleRotate(Vector3 t)        //子彈彈孔的方向設定
    {
        if (t == Vector3.up || t == Vector3.down)
        {
            return Quaternion.Euler(90, 0, 0);

        }
        else if (t == Vector3.left || t == Vector3.right)
        {
            return Quaternion.Euler(0, 90, 0);
        }
        else if (t == Vector3.forward || t == Vector3.back)
        {
            return Quaternion.Euler(0, 0, 0);
        }
        else
        {
            return quaternion.identity;
        }

    }
}
