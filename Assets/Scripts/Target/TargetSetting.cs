using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    None = 0,
    Zombie,
    Target,

}
public class TargetSetting : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("目標設定")]
    public EnemyType enemyType;
    public int TargetHP;

    public Transform targetPos;
    public NavMeshAgent navAgent;
    public Animator anim;
    public bool isDead;

    [Header("目標UI設定")]
    public GameObject dmg_UI;

    void Start()
    {
        //navAgent = GetComponent<NavMeshAgent>();
        // anim = GetComponent<Animator>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyType)
        {
            case EnemyType.Zombie:
                ZombieSetting();
                break;
            case EnemyType.Target:

                break;

        }

    }

    void ZombieSetting()
    {
        if (navAgent != null)
        {
            if (TargetHP >= 0)     //當目標HP大於0時，做以下內容
            {
                float distance = Vector3.Distance(transform.position, targetPos.position);                      //取得目標與玩家的距離
                transform.LookAt(new Vector3(targetPos.position.x, transform.position.y, targetPos.position.z));


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
                anim.Play("zombie_dying");              //播放死亡狀態動畫

            }
        }
    }
    public void Reset()                                 //當hit被播放時，等待hit動畫結束會撥放初始idle的動畫，接下來就會自動撥放對應的動畫。
    {
        if (isDead == false)
        {
            anim.Play("zombie_idle");
        }

    }




}
