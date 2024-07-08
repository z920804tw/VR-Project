using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class WeaponGun : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("槍枝設定")]
    [SerializeField] GameObject bullet;
    public Transform firePos;
    public Transform fireRay;
    [SerializeField] int maxBullet;
    int currentBullet;


    [SerializeField] float fireDelay;
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxRayDistance;

    [Header("雜項設定")]
    public LineRenderer lineRenderer;
    public GameObject shootEffect;
    public GameObject GunUI;
    public TextMeshProUGUI ammoText;

    [Header("槍枝音效")]
    public AudioClip shootClip;
    AudioSource audioSource;

    [Header("Debug")]

    public bool useLineRay;
    public bool useContinueShoot;
    [SerializeField] bool isHolding;

    float timer;




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
        if (useLineRay)
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled=false;
        }
        ShowGunRay();
    }
    private void OnTriggerStay(Collider other)
    {

        if (isHolding && useContinueShoot)  //只有連射武器會需要用到，如果是一般單發射擊不用用到。
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
    void UpdateUiText()
    {
        ammoText.text = $"{currentBullet}/{maxBullet}";

    }

    void ContinueShoot()    //連射武器用的
    {
        if (currentBullet > 0)
        {
            timer += Time.deltaTime;
            if (timer > fireDelay)
            {
                timer = 0;
                GameObject bb = Instantiate(bullet, firePos.position, Quaternion.identity);
                bb.GetComponent<Rigidbody>().AddForce(firePos.forward * bulletSpeed, ForceMode.Impulse);
                Destroy(bb, 1f);

                GameObject effect = Instantiate(shootEffect, firePos.position, Quaternion.identity);
                effect.transform.SetParent(firePos);
                effect.transform.localEulerAngles = new Vector3(0, -90, 0);
                audioSource.PlayOneShot(shootClip);
                Destroy(effect, 0.5f);

                currentBullet--;
                UpdateUiText();
            }
        }

    }
    void ShowGunRay()
    {
        Ray ray = new Ray(fireRay.position, fireRay.forward);        //槍枝雷射用的
        RaycastHit hit;
        lineRenderer.SetPosition(0, fireRay.position);
        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            lineRenderer.SetPosition(1, hit.point);

        }
        else
        {
            lineRenderer.SetPosition(1, fireRay.position + fireRay.forward * maxRayDistance);
        }
    }
    public void Shoot()         //單發單發射的武器用
    {
        if (currentBullet > 0)
        {
            GameObject bb = Instantiate(bullet, firePos.position, Quaternion.identity);
            bb.GetComponent<Rigidbody>().AddForce(firePos.forward * bulletSpeed, ForceMode.Impulse);
            Destroy(bb, 1f);

            GameObject effect = Instantiate(shootEffect, firePos.position, Quaternion.identity);
            effect.transform.SetParent(firePos);
            effect.transform.localEulerAngles = new Vector3(0, -90, 0);
            audioSource.PlayOneShot(shootClip);
            Destroy(effect, 0.5f);


            currentBullet--;
            UpdateUiText();
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
        GunUI.SetActive(false);
    }
}
