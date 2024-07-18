using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxDistance;
    public float downSpeed;
    public LayerMask layerMask;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {

        }
        else
        {
            transform.Translate(Vector3.down*downSpeed*Time.deltaTime);
        }
        Debug.DrawRay(transform.position,Vector3.down*maxDistance, Color.red);
    }
}
