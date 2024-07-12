using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class BulletClip : MonoBehaviour
{
    // Start is called before the first frame update
    public GunType gunType;
    public Animator anim;
    public bool isHolding;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Hold()
    {
        if (isHolding == false)
        {
            isHolding = true;
        }
        else
        {
            isHolding = false;
        }
    }
}
