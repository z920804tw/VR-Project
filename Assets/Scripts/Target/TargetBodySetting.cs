using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor.Search;
using UnityEngine;

public enum BodyType
{
    None,
    Body,
    Head,
}
public class TargetBodySetting : MonoBehaviour
{
    // Start is called before the first frame update
    public BodyType bodyType;
    public Transform dmgUITransform;
    public float upTimeLimit;
    public float upSpeed;                       //建議不要太高大概0.01左右

    public int maxDmg;
    public int minDmg;
    int rndDmg;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            if (bodyType == BodyType.Head)
            {
                takeDmg(minDmg, maxDmg);
            }
            else if (bodyType == BodyType.Body)
            {
                takeDmg(minDmg, maxDmg);
            }

        }

    }

    void takeDmg(int min, int max)
    {
        rndDmg = Random.Range(min, max);
        GameObject parent = transform.parent.GetComponent<TargetSetting>().dmg_UI;

        Vector3 randomSpread = Random.insideUnitSphere * 0.5f;
        Vector3 pos = dmgUITransform.position + randomSpread;

        GameObject dmg = Instantiate(parent, pos, Quaternion.identity);
        dmg.transform.SetParent(this.transform);

        StartCoroutine(dmg.GetComponent<TargetDmgUI>().dmgTextMoveUP());
        parent.GetComponentInChildren<TextMeshProUGUI>().text = $"-{rndDmg}";
        transform.parent.GetComponent<TargetSetting>().TargetHP -= rndDmg;

    }


}
