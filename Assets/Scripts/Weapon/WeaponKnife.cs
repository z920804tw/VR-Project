using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnife : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("進戰武器設定")]
    public int maxDmg;
    public int minDmg;
    [Header("武器雜項")]
    public GameObject knifeMarkPrefab;
    public LayerMask layerMask;

    public Transform knifePos;

    [Header("Debug")]

    [SerializeField] GameObject currentHitObject;
    [SerializeField] float maxDistance;
    [SerializeField] bool isHoldig;
    bool hasSpawnMark;


    Rigidbody rb;
    BoxCollider bc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        isHoldig = false;
        hasSpawnMark = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(knifePos.position, knifePos.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {
            currentHitObject = hit.collider.gameObject;
            if (hasSpawnMark == false && currentHitObject.gameObject.tag != "Enemy")
            {
                hasSpawnMark = true;
                Quaternion rotate = KnifeMarkRotate(hit.normal);
                GameObject km = Instantiate(knifeMarkPrefab, hit.point, rotate);

                km.transform.SetParent(hit.collider.transform);
                Destroy(km, 10f);
            }
        }
        else
        {
            currentHitObject = null;
            hasSpawnMark = false;
        }
        Debug.DrawRay(knifePos.position, knifePos.forward * maxDistance, Color.red);
    }

    public void Holding()
    {
        if (isHoldig == false)      //拿起狀態，設定boxCollider為Trigger
        {
            bc.isTrigger = true;
            isHoldig = true;
        }
        else
        {
            if (currentHitObject != null)           //如果放開時，刀有插在物體上就坐下面的事情，否則就坐else的事情
            {
                this.transform.SetParent(currentHitObject.transform);
                isHoldig = false;
                rb.isKinematic = true;
            }
            else
            {
                isHoldig = false;
                this.transform.SetParent(null);
                rb.isKinematic = false;
                bc.isTrigger = false;

            }

        }
    }
    Quaternion KnifeMarkRotate(Vector3 t)        //刀的刮痕方向設定
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
