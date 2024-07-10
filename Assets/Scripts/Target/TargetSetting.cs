using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSetting : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("靶子設定")]
    public int TargetHP;
    


    [Header("靶子UI設定")]
    public GameObject dmg_UI;

    int rndDmg;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (TargetHP <= 0)
        {
            Destroy(this.gameObject);
        }

    }



}
