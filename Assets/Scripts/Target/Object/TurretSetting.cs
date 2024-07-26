using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSetting : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("砲塔設定")]
    public GameObject turretBody;
    public Transform rayPos;
    public Transform firePos;
    public Animator anim;
    public LayerMask layerMask;
    [Header("砲塔參數設定")]
    public float fireDelay;

    public float fireTime;
    public float fireSpeed;
    public float turnSpeed;
    public float rayDistance;

    [Header("Debug")]

    [SerializeField] bool isHolding;
    [SerializeField] bool canRotate;
    bool resetRotate;
    [SerializeField] GameObject target;


    void Start()
    {
        canRotate = false;
        resetRotate = false;
    }

    // Update is called once per frame
    void Update()
    {
        isHolding = GetComponent<ObjectSetting>().isHolding;
        if (isHolding == false)
        {
            if (target == null)
            {
                if (resetRotate == false)
                {
                    resetRotate=true;
                    turretBody.transform.localEulerAngles=new Vector3(0,0,0);
                }
                SearchTarget();
                anim.SetBool("attack", false);
                anim.SetBool("search", true);
                canRotate = false;

            }
            else if (target != null)
            {
                anim.SetBool("search", false);
                anim.SetBool("attack", true);
                canRotate = true;
                anim.Play("attack");
                if (canRotate)
                {
                    Vector3 dir = target.transform.position - turretBody.transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(dir);
                    turretBody.transform.rotation = Quaternion.Slerp(turretBody.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                }

            }

        }
        else if (isHolding == true)
        {
            anim.SetBool("search", false);
            anim.SetBool("attack", false);
            resetRotate=false;
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
}
