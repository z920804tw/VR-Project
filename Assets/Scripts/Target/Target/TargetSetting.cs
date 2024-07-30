using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor.Callbacks;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public enum TargetType
{
    None = 0,
    Zombie,
    Skeleton,
    Target,                                     //靶子用

}
public class TargetSetting : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("目標設定")]
    public TargetType TargetType;
    Transform targetPos;
    public Transform targetCenter;
    public Transform defaultTarget;
    public Transform[] rayPos;
    public NavMeshAgent navAgent;

    public Animator anim;
    [Header("目標參數設定")]
    public int TargetHP;
    public float maxRayDistance;
    public float turnSpeed;
    public int maxDmg;
    public int minDmg;

    public LayerMask layerMask;


    [Header("目標UI設定")]
    public GameObject dmg_UI;
    [Header("Debug")]
    public bool isDead;
    public bool isHit;




    void Start()
    {
        isDead = false;
        isHit = false;

    }

    // Update is called once per frame
    void Update()
    {

        switch (TargetType)                     //會去看這個物件的type是什麼，如果是殭屍跟骷髏就會有找敵人的動作
        {
            case TargetType.Zombie:
            case TargetType.Skeleton:
                if (targetPos != null)                          //先判斷targetPos是不是有東西，如果有就執行EnemySetting ，否則就會去找預設目標
                {
                    EnemySetting();
                }
                else if (targetPos == null)
                {
                    if (defaultTarget != null)                 //會看預設目標是否存在，如果存在就往目標走，如果不再就會待在原地撥放idle動畫，直到有目標進到射線範圍內
                    {
                        navAgent.SetDestination(defaultTarget.position);
                        anim.SetBool("attack", false);
                        anim.SetBool("walk", true);
                    }
                    else
                    {
                        anim.SetBool("attack", false);
                        anim.SetBool("walk", false);
                        Debug.Log("找不到目標和預設目標");
                    }
                    FindTarget();                               //不論預設目標有沒有存在都會找，只要有目標被偵測到，就會設定該目標為TargetPos
                }
                break;
            case TargetType.Target:

                break;

            default:
                break;

        }


    }
    void FindTarget()                                               //只要指定的物件進到射線範圍內，就會去設定目標。
    {

        for (int i = 0; i < 2; i++)
        {
            Ray ray = new Ray(rayPos[i].position, rayPos[i].forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxRayDistance, layerMask))
            {
                targetPos = hit.collider.transform;
            }
            Debug.DrawRay(rayPos[i].position, rayPos[i].forward * maxRayDistance, Color.red);
        }

    }

    void EnemySetting()
    {
        if (navAgent != null)
        {
            if (TargetHP >= 0)     //當目標HP大於0時，做以下內容
            {
                if (isHit == false)                                                                             //沒被打中時會追蹤目標,否則就停下。
                {
                    float distance = Vector3.Distance(transform.position, targetPos.position);                      //取得目標與玩家的距離

                    EnemyRotation();
                    if (distance > navAgent.stoppingDistance + 0.3f)                                                //如果距離>設定停下距離+0.2f，就繼續撥放走路的動畫
                    {
                        anim.SetBool("attack", false);
                        anim.SetBool("walk", true);

                        navAgent.SetDestination(targetPos.position);
                    }
                    else if (distance <= navAgent.stoppingDistance + 0.3f)                                    //如果距離<=設定停下距離+0.2f，就撥放攻擊的動畫
                    {
                        anim.SetBool("walk", false);
                        anim.SetBool("attack", true);
                    }
                }
                else
                {
                    navAgent.isStopped = true;
                }
            }
            else if (TargetHP <= 0) //當目標HP<=0時，就代表死亡
            {
                if (isDead == false)        //判斷isDead是不是等於false，這樣可以避免裡面的程式會一直重複執行
                {
                    isDead = true;

                    navAgent.isStopped = true;  //導航設定暫停
                    navAgent.updatePosition = false;    //導航位置更新暫停
                    navAgent.updateRotation = false;    //導航旋轉更新暫停
                    Destroy(this.gameObject, 5f);       //5秒後摧毀此物件
                }
                anim.Play("dying");              //播放死亡狀態動畫

            }
        }
    }
    public void Reset()                                 //當hit被播放時，等待hit動畫結束會撥放初始idle的動畫，接下來就會自動撥放對應的動畫。
    {
        if (isDead == false)
        {
            anim.Play("idle");
            isHit = false;
            navAgent.isStopped = false;
        }

    }

    void EnemyRotation()                    //敵人慢慢旋轉的效果
    {
        Vector3 dir = targetPos.position - transform.position;
        dir.y = 0;
        Quaternion TargetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, turnSpeed * Time.deltaTime);
    }


    public void AttackObject()       //給敵人攻擊物件時，物件減HP時用的
    {
        if (targetPos != null)
        {
            if (targetPos.gameObject.tag == "Object")                   //如果targetPos的tag是物件，就去抓她的ObjectSetting裡面的takeDmg
            {
                targetPos.GetComponent<ObjectSetting>().takeDmg(minDmg, maxDmg);
            }
            else if (targetPos.gameObject.tag == "Player")
            {

            }
        }
    }

}
