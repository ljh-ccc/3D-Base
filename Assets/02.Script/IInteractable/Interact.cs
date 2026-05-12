using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] ShootingRay ray;

    [SerializeField] GameObject target;

    private void Start()
    {
        if(ray == null) { Debug.LogWarning("[Interact] ray is null"); return; }
        if(ray != null) { ray.OnTarget += TargetOn; }
    }

    private void OnDisable()
    {
        ray.OnTarget -= TargetOn;
    }

    void TargetOn(GameObject obj)
    {
        target = obj;
    }

    public void InteractObject()
    {
        if (target == null) { Debug.Log("[Interact] target is null"); return; }

        IInteractable interact = target.GetComponent<IInteractable>();
        if(interact != null)
        {
            interact.Interact();
        }
    }
}
