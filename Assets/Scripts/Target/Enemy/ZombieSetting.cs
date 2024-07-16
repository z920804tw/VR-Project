using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSetting : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent navAgent;
    public Transform targetPos;
    public Animator anim;
    int enemyHP;
    bool isDead;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        enemyHP = GetComponent<TargetSetting>().TargetHP;
        

    }
}
