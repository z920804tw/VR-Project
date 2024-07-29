
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    SMG,
    Turret,
}
public class WeaponGun : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("槍枝類型設定")]
    public GunType gunType;
    [Header("槍枝配件位置設定")]
    public Transform firePos;
    public Transform fireRay;
    public Transform clipTransform;
    [Header("槍枝子彈設定")]
    [SerializeField] GameObject bullet;
    [SerializeField] int maxBullet;
    [SerializeField] int maxDmg;
    [SerializeField] int minDmg;
    [Header("槍枝功能設定")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] float fireDelay;
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxRayDistance;
    [SerializeField] float spread;

    [Header("雜項設定")]
    public LineRenderer lineRenderer;
    public GameObject shootEffect;
    public GameObject fireLight;
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
    int currentBullet;


    void Start()
    {
        currentBullet = maxBullet;
        timer = fireDelay; //讓使用者剛按下去時就能馬上開槍

        isHolding = false;
        useLineRay = false;
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
        if (isHolding && gunType == GunType.Rifle || gunType == GunType.SMG)  //只有連射武器會需要用到，如果是一般單發射擊不用用到。1
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
        if (other.gameObject.tag == "Clip" && other.GetComponent<BulletClip>().gunType == this.gunType)     //裝彈藥123
        {
            if (currentBullet <= 0 && other.gameObject.GetComponent<BulletClip>().isHolding == false)
            {
                other.transform.SetParent(clipTransform);
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                other.gameObject.transform.eulerAngles = new Vector3(clipTransform.eulerAngles.x, clipTransform.eulerAngles.y, clipTransform.eulerAngles.z);
                other.gameObject.transform.position = clipTransform.position;

                other.gameObject.GetComponent<BulletClip>().anim.SetBool("Reload", true);
                currentBullet = maxBullet;
                UpdateUiText();
                Destroy(other.gameObject, 1.3f);
            }
        }
    }
    void ShowFire()
    {
        GameObject effect=Instantiate(fireLight,firePos.position,Quaternion.identity);
        effect.transform.SetParent(firePos);
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
        }
        else                                                            //如果沒有打到物件就會變到這裡。
        {
            lineRenderer.SetPosition(1, fireRay.position + fireRay.forward * maxRayDistance);
        }

    }
    void UpdateUiText()
    {
        ammoText.text = $"{currentBullet}/{maxBullet}";
    }

    void InstantiateBullet()
    {
        Vector3 gunSpread = Random.insideUnitSphere * spread;               //子彈、設限的隨機方向
        Vector3 dir = firePos.forward + gunSpread;

        Ray ray = new Ray(firePos.position, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayDistance, layerMask))      //如果設限有打到指定的layerMask就會產生彈孔在hit.point的位置上。
        {
            rotate = bulletHoleRotate(hit.normal);
            GameObject bh = Instantiate(bulletHole, hit.point, rotate);
            bh.transform.SetParent(hit.collider.transform);
            Destroy(bh, 10f);
        }

        GameObject bb = Instantiate(bullet, firePos.position, Quaternion.identity);  //生成子彈
        bb.GetComponent<Rigidbody>().AddForce(dir * bulletSpeed, ForceMode.Impulse);
        bb.GetComponent<Bullet>().MaxDmg = maxDmg;
        bb.GetComponent<Bullet>().MinDmg = minDmg;
        Destroy(bb, 2f);

        if (this.gunType != GunType.Shotgun)                                            //如果槍的類型不是霰彈槍就生成特效，原因是因為霰彈槍有for迴圈，所以霰彈槍的特效是獨立分開寫的。
        {
            GameObject effect = Instantiate(shootEffect, firePos.position, Quaternion.identity);
            effect.transform.SetParent(firePos);
            effect.transform.localEulerAngles = new Vector3(0, -90, 0);
            audioSource.PlayOneShot(shootClip);
            Destroy(effect, 0.5f);
            ShowFire();
        }

        

    }

    void ContinueShoot()    //連射武器用的，例如步槍
    {
        if (currentBullet > 0)
        {
            timer += Time.deltaTime;
            if (timer > fireDelay)
            {
                timer = 0;

                InstantiateBullet();
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
            InstantiateBullet();
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
                InstantiateBullet();
            }

            GameObject effect = Instantiate(shootEffect, firePos.position, Quaternion.identity);                            //產生開火特效和音效
            effect.transform.SetParent(firePos);
            effect.transform.localEulerAngles = new Vector3(0, -90, 0);
            audioSource.PlayOneShot(shootClip);
            Destroy(effect, 0.5f);

            ShowFire();

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
        if (t == Vector3.up || t == Vector3.down)               //向量為上或下時，X軸旋轉90
        {
            return Quaternion.Euler(90, 0, 0);

        }
        else if (t == Vector3.left || t == Vector3.right)       //向量為左或右時，Y軸旋轉90
        {
            return Quaternion.Euler(0, 90, 0);
        }
        else if (t == Vector3.forward || t == Vector3.back)     //向量為前或後時,不變。
        {
            return Quaternion.Euler(0, 0, 0);
        }
        else                                                    //有問題時，為預設
        {
            return Quaternion.identity;
        }

    }
}
