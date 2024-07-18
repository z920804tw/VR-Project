using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor.Callbacks;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    None = 0,
    Zombie,
    Skeleton,
    Target,

}
public class TargetSetting : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("目標設定")]
    public EnemyType enemyType;
    public Transform targetPos;
    public Transform rayPos;
    public NavMeshAgent navAgent;

    public Animator anim;
    [Header("目標參數設定")]
    public int TargetHP;
    public float maxRayDistance;

    public LayerMask layerMask;


    [Header("目標UI設定")]
    public GameObject dmg_UI;
    [Header("Debug")]
    public bool isDead;
    public bool isHit;
    Rigidbody rb;

    void Start()
    {
        //navAgent = GetComponent<NavMeshAgent>();
        // anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        isDead = false;
        isHit = false;
    }

    // Update is called once per frame
    void Update()
    {

        switch (enemyType)
        {
            case EnemyType.Zombie:
            case EnemyType.Skeleton:
                if (targetPos != null)
                {
                    EnemySetting();
                }
                else if (targetPos == null)
                {
                    anim.SetBool("attack", false);
                    anim.SetBool("walk", false);
                    FindTarget();
                }
                break;
            case EnemyType.Target:

                break;
        }


    }
    void FindTarget()                                               //只要指定的物件進到射線範圍內，就會去設定目標。
    {

        Ray ray = new Ray(rayPos.position, rayPos.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRayDistance, layerMask))
        {
            targetPos = hit.collider.transform;
        }
        Debug.DrawRay(rayPos.position, rayPos.forward * maxRayDistance, Color.red);
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
                    if (distance > navAgent.stoppingDistance + 0.2f)                                                //如果距離>設定停下距離+0.2f，就繼續撥放走路的動畫
                    {
                        anim.SetBool("attack", false);
                        anim.SetBool("walk", true);

                        navAgent.SetDestination(targetPos.position);

                    }
                    else if (distance <= navAgent.stoppingDistance + 0.2f)                                    //如果距離<=設定停下距離+0.2f，就撥放攻擊的動畫
                    {
                        anim.SetBool("walk", false);
                        anim.SetBool("attack", true);
                    }
                }
                else
                {
                    navAgent.isStopped = true;
                    isHit = true;
                }

            }
            else if (TargetHP <= 0) //當目標HP<=0時，座以下內容
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
        transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, 0.1f * Time.deltaTime);
    }




}
