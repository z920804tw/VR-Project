using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public GunType gunType;
    public int MaxDmg;
    public int MinDmg;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag != "Gun"&& other.collider.tag!="Bullet")
        {

            Destroy(this.gameObject);
        }
    }


}
