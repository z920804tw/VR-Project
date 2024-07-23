using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HandType
{
    None = 0,
    Left,
    Right,
}
public class HandInput : MonoBehaviour
{
    // Start is called before the first frame update
    public InputActionReference HandTrigger;

    public GameObject hand;
    public HandType handType;
    public LayerMask layerMask;
    GameObject currentHit, preHit;

    void Start()
    {
        hand = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (handType == HandType.Left)
        {
            ShowOutline();
        }
        else if (handType == HandType.Right)
        {
            ShowOutline();
        }
    }

    void ShowOutline()
    {
        Ray leftRay = new Ray(hand.transform.position, hand.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(leftRay, out hit, 10, layerMask))
        {
            if (preHit != null)
            {
                preHit.GetComponent<Outline>().enabled = false;
                preHit = null;
            }
            currentHit = hit.collider.gameObject;
            if (currentHit.GetComponent<Outline>() != null)
            {
                currentHit.GetComponent<Outline>().enabled = true;
                preHit = currentHit;
            }
        }
        else
        {
            if (preHit != null  && preHit.GetComponent<ObjectSetting>().isHolding==false)
            {
                preHit.GetComponent<Outline>().enabled = false;
            }
            preHit = null;
            currentHit = null;
        }
    }
}
