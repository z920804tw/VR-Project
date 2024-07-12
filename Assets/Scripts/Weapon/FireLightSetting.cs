
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireLightSetting : MonoBehaviour
{
    // Start is called before the first frame update
    Light light;
    float a;
    void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(fireLightEffect());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator fireLightEffect()
     {
         float timer = 0;
         float duringTime = 0.5f;
         while (timer < duringTime)
         {
             timer += Time.deltaTime;
             float t = timer / duringTime;
             light.intensity = Mathf.Lerp(3.5f, 0, t);
             yield return null;
         }
         Destroy(this.gameObject);
     }
}
