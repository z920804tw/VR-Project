using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI.BodyUI;

public class ObjectSetting : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("物件座標設定")]
    public Transform dmgUITransform;

    [Header("物件設定")]
    public GameObject dmgUI;
    public GameObject objectPrefab;
    public GameObject deadEffect;
    public LayerMask layerMask;

    [Header("物件參數設定")]
    public int objectHp;
    public float placeMaxDistance;
    public InputActionReference leftHandRotate;

    [Header("Debug")]

    [SerializeField] bool canPlace;
    [SerializeField] bool hasPreview;
    public bool isHolding;
    bool isDead;


    //內部參數
    GameObject hand;
    GameObject aa;
    Vector3 scale;
    Vector3 hitPoint;
    Outline outline;
    ContinuousMoveProviderBase move;
    float preSpeed;
    //內部參數


    void Start()
    {
        isHolding = false;
        canPlace = false;
        hasPreview = false;
        isDead = false;
        move = GameObject.Find("Move").GetComponent<ContinuousMoveProviderBase>();
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {

        if (objectHp > 0)
        {
            if (isHolding == true && hand != null)          //isHolding為true並且有取得到hand物件，就執行顯示預覽放置物件位置功能。
            {
                ShowObjectPreview();
            }
        }
        else if (objectHp < 0)
        {
            if (isDead == false)
            {
                isDead = true;
                GameObject eft = Instantiate(deadEffect, transform.position, Quaternion.identity);
                Destroy(eft,2f);
                Destroy(this.gameObject, 1f);
            }

        }



    }
    public void takeDmg(int minDmg, int maxDmg)         //給攻擊動畫用的
    {
        int rndDmg = Random.Range(minDmg, maxDmg);

        Vector3 randomSpread = Random.insideUnitSphere * 0.5f;
        Vector3 pos = dmgUITransform.position + randomSpread;

        GameObject dmg = Instantiate(dmgUI, pos, Quaternion.identity);
        dmg.transform.SetParent(dmgUITransform);
        dmgUI.GetComponentInChildren<TextMeshProUGUI>().text = $"-{rndDmg}";

        dmg.GetComponent<TargetDmgUI>().upSpeed = 0.01f;
        dmgUI.GetComponent<TargetDmgUI>().upTimeLimit = 2;

        objectHp -= rndDmg;
    }

    public void ObjectHold()            //會用在SelectEnter跟Exit上
    {
        if (isHolding == false)         //如果isHoding為false時，就設定為true，並縮小物件的scale。 會用在拿起物件的時刻
        {
            isHolding = true;
            scale = transform.localScale;
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            preSpeed = move.moveSpeed;

            outline.enabled = true;
            Debug.Log(transform.localScale);

        }
        else                            //如果isHolding為true時，會會重製所有基本設定，並角色移動速度、物件縮放、都會恢復之前的值。放開物件時用到
        {
            isHolding = false;
            hasPreview = false;
            canPlace = false;

            move.moveSpeed = preSpeed;
            transform.localScale = scale;
            this.gameObject.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); //放開物件只能限制Y有旋轉，其他軸都鎖定0

            hand = null;
            if (aa != null)
            {
                Destroy(aa);
            }
            outline.enabled = false;
            Debug.Log("放開手中物件");
        }
    }

    void ShowObjectPreview()                    //顯示手上的物件放置位置。
    {
        Ray ray = new Ray(hand.transform.position + hand.transform.forward * 0.2f, hand.transform.forward); //會有一條設限從手部的位置向前發射
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, placeMaxDistance, layerMask))
        {
            if (hasPreview == false)    //如果有打到指定的layerMask，就會有一個初始化的設定，這個只會執行一次。
            {
                hasPreview = true;
                aa = Instantiate(objectPrefab, hit.point, Quaternion.identity);
                move.moveSpeed = 0;
            }
            else if (hasPreview == true)    //之後會一直執行這邊，會一直修改預覽物件的位置到hitpoint上。
            {
                canPlace = true;
                hitPoint = hit.point;
                aa.transform.position = hit.point;

                Vector2 rotate = leftHandRotate.action.ReadValue<Vector2>();    //玩家可以使用左手炳的蘑菇投來旋轉這個預覽物件的Y軸旋轉角度。
                aa.transform.eulerAngles += new Vector3(0, rotate.x, 0);

            }
        }
        else                            //沒打到東西也會重製基本設定。
        {
            hasPreview = false;
            canPlace = false;
            move.moveSpeed = preSpeed;

            if (aa != null)
            {
                Destroy(aa);
                Debug.Log("刪除預覽物件");
            }
            Debug.Log("無法放置");
        }

        Debug.DrawRay(hand.transform.position, hand.transform.forward * placeMaxDistance, Color.red);
    }
    public void PlaceObject()       //當玩家在拿著物件時，按下板機鍵就會執行這邊，會去將手上的物件放下，並將她的位置、旋轉指定到對應的位置上。
    {
        if (canPlace == true)
        {
            XRInteractionManager manager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
            IXRSelectInteractable xRSelect = GetComponent<XRGrabInteractable>();

            XRBaseInteractor controller = hand.GetComponent<XRBaseInteractor>();
            manager.SelectExit(controller, xRSelect);

            this.transform.position = new Vector3(hitPoint.x, hitPoint.y + 0.1f, hitPoint.z);
            this.transform.rotation = aa.transform.rotation;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Hand" && hand == null)
        {
            hand = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hand" && hand != null)
        {
            hand = null;
        }
    }

}
