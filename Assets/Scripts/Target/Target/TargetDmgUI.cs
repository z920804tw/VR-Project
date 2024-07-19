using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDmgUI : MonoBehaviour
{
    // Start is called before the first frame update
    float timer;
    public float upTimeLimit, upSpeed;


    void Start()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        if (upSpeed != 0 && upTimeLimit != 0)
        {
            StartCoroutine(dmgTextMoveUP());
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator dmgTextMoveUP()
    {
        timer = 0;

        while (timer < upTimeLimit)
        {
            timer += Time.deltaTime;
            this.transform.position += transform.up * upSpeed;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
