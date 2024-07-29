
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireLightSetting : MonoBehaviour
{
    new
        // Start is called before the first frame update
        Light light;
    public float duringTime;
    public float lightStrength;
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
         
         while (timer < duringTime)
         {
             timer += Time.deltaTime;
             float t = timer / duringTime;
             light.intensity = Mathf.Lerp(lightStrength, 0, t);
             yield return null;
         }
         Destroy(this.gameObject);
     }
}
