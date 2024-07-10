using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDmgUI : MonoBehaviour
{
    // Start is called before the first frame update
    float timer;

    void Start()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator dmgTextMoveUP()
    {
        timer = 0;
        float upTimeLimit = transform.parent.GetComponent<TargetBodySetting>().upTimeLimit;
        float upSpeed = transform.parent.GetComponent<TargetBodySetting>().upSpeed;
        while (timer < upTimeLimit)
        {
            timer += Time.deltaTime;
            this.transform.position+=transform.up*upSpeed;
            yield return null;
        }
        timer = 0;
        Destroy(this.gameObject);
    }
}
