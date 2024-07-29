using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TurretSetting : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("砲塔設定")]
    public GameObject turretBody;
    public GameObject bullet;
    public GameObject fireEffect;
    public GameObject fireLight;
    public Transform rayPos;
    public Transform firePos;
    public Animator anim;

    public LayerMask layerMask;
    [Header("音效設定")]
    public AudioSource audioSource;
    public AudioClip shootClip;
    public AudioClip turnClip;

    [Header("砲塔參數設定")]

    public float fireDelay;
    public float bulletSpeed;
    public float turnSpeed;
    public float rayDistance;
    public int maxDmg;
    public int minDmg;

    [Header("Debug")]
    [SerializeField] GameObject target;
    [SerializeField] bool isHolding;
    bool reset;
    float timer;



    void Start()
    {
        reset = false;

    }

    // Update is called once per frame
    void Update()
    {
        isHolding = GetComponent<ObjectSetting>().isHolding;
        if (isHolding == false)
        {
            if (target == null)
            {
                if (reset == false)
                {
                    reset = true;
                    turretBody.transform.localEulerAngles = new Vector3(0, 0, 0);

                    audioSource.clip = turnClip;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                SearchTarget();
                anim.SetBool("attack", false);
                anim.SetBool("search", true);
            }
            else if (target != null)
            {
                anim.SetBool("search", false);
                StopSound();
                Shoot();
                reset = false;


                Vector3 dir = target.GetComponent<TargetSetting>().targetCenter.transform.position - turretBody.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                turretBody.transform.rotation = Quaternion.Slerp(turretBody.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);


                if (target.GetComponent<TargetSetting>().isDead)
                {
                    target = null;
                }
            }

        }
        else if (isHolding == true)
        {
            anim.SetBool("search", false);
            anim.SetBool("attack", false);

            StopSound();

            reset = false;
            target = null;

            anim.Play("idle");
        }
    }

    void SearchTarget()
    {
        Ray ray = new Ray(rayPos.position, rayPos.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))          //只會搜尋帶有敵人layer的目標而已
        {
            target = hit.collider.gameObject;
        }
        Debug.DrawRay(rayPos.position, rayPos.forward * rayDistance, Color.red);
    }

    void Shoot()
    {
        timer += Time.deltaTime;
        if (timer > fireDelay)
        {
            timer = 0;
            audioSource.PlayOneShot(shootClip);

            anim.Play("attack");
            GameObject T_Bullet = Instantiate(bullet, firePos.position, Quaternion.identity);
            T_Bullet.GetComponent<Rigidbody>().AddForce(firePos.forward * bulletSpeed, ForceMode.Impulse);
            T_Bullet.GetComponent<Bullet>().MaxDmg = maxDmg;
            T_Bullet.GetComponent<Bullet>().MinDmg = minDmg;

            GameObject effect = Instantiate(fireEffect, firePos.position, Quaternion.identity);
            effect.transform.SetParent(firePos);
            effect.transform.localEulerAngles = new Vector3(0, -90, 0);
            Destroy(effect, 1f);

            GameObject light = Instantiate(fireLight, firePos.position, Quaternion.identity);
            light.transform.SetParent(firePos);

        }
    }
    void StopSound()//取消搜尋時的聲音clip
    {
        audioSource.clip = null;
    }


}
