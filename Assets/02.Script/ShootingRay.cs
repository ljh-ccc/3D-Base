using System;
using System.Collections;
using UnityEngine;

public class ShootingRay : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    [SerializeField] LayerMask targetLayer;

    [SerializeField] float rayDistance = 10.0f;
    [SerializeField] Color rayColor;

    [SerializeField] public GameObject currentObj;
    [SerializeField] string interactMsg;

    [SerializeField] Transform muzzle;

    Vector3 screenCenter;
    
    WaitForSeconds coolTime;
    bool isDead;

    public event Action<GameObject> OnTarget;

    private void Awake()
    {
        if (mainCamera == null) { mainCamera = Camera.main; }
        coolTime = new WaitForSeconds(0.5f);
        isDead = false;
        screenCenter = new Vector3(0.5f, 0.5f, 0f);
        interactMsg = null;
    }

    private void Start()
    {
        StartShootingRay();
    }

    void StartShootingRay()
    {
        if(mainCamera == null) { Debug.LogError("[ShootingRay] mainCamera is null"); return; }

        StartCoroutine(ShootingRayCoroutine());
    }

    //ScreenCenter가 바뀌면 그에 맞춰 값을 변경해야함

    IEnumerator ShootingRayCoroutine()
    {
        while (!isDead)
        {
            Ray ray = mainCamera.ViewportPointToRay(screenCenter);
            

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, targetLayer, QueryTriggerInteraction.Ignore))
            {    
                Debug.DrawRay(muzzle.position, ray.direction * hit.distance, rayColor, 0.5f);

                GameObject target = hit.collider.gameObject;

                Debug.Log($"[ShootingRay] hit name : <color=green>{target}</color> hit distance : <color=yellow>{hit.distance}</color> " +
                    $"hit position : <color=white>{hit.point}</color>");

                TryAddCurrentObj(target);

                HowToInteract(target);
            }
            else
            {
                Debug.DrawRay(muzzle.position, ray.direction * rayDistance, Color.yellow, 0.5f);

                if(currentObj != null)
                {
                    currentObj = null;
                    OnTarget?.Invoke(currentObj);
                    interactMsg = null;
                    Debug.Log($"[ShootingRay] currentObj : {((currentObj == null) ? "null" : currentObj.name)}");
                }
            }

            yield return coolTime;
        }
    }

    void TryAddCurrentObj(GameObject obj)
    {
        IInteractable interact = obj.GetComponent<IInteractable>();
        if (interact != null && currentObj != obj)
        {
            currentObj = obj;
            Debug.Log($"[ShootingRay] Sucessed add : <color=blue>{currentObj.name}</color>");
            
            OnTarget?.Invoke(currentObj);
        }
        else if(interact == null)
        {
            currentObj = null;
            OnTarget?.Invoke(currentObj);
        }
    }

    string CheckTag(GameObject obj)
    {
        if (obj == null) { Debug.Log("[ShootingRay] obj is null"); return null; }
        string msg = "";

        IInteractable interact = obj.GetComponent<IInteractable>();
        if(interact == null) { return null; }

        if(obj.CompareTag("NPC"))
        {
            msg += $"Talk to {obj.name}";
        }
        else if(obj.CompareTag("Box"))
        {
            msg += interact.InteractMsg;
        }

        return msg;
    }

    void DebugInteractText(GameObject obj)
    {
        string msg = CheckTag(obj);

        Debug.Log($"[ShootingRay] <color=white>{msg}</color>");
        interactMsg = msg;
    }

    void HowToInteract(GameObject obj)
    {
        CheckTag(obj);

        DebugInteractText(obj);
    }
}
