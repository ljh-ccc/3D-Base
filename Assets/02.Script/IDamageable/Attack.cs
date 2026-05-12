using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] GameObject attackPoint;

    [SerializeField] int damage;
    [SerializeField] float radius;
    [SerializeField] LayerMask targetLayer;

    Collider[] results = new Collider[3];

    [SerializeField] float waitTime;
    WaitForSeconds someTime;


    private void Awake()
    {
        someTime = new WaitForSeconds(waitTime);
    }

    public void StartAttack()
    {
        StartCoroutine(OptimizedOverlap());
    }

    IEnumerator OptimizedOverlap()
    {
        if(attackPoint == null) { Debug.LogWarning("[Attack] attakcPoint is null"); yield break; }

        yield return someTime;

        int count = Physics.OverlapSphereNonAlloc(attackPoint.transform.position, radius, results, targetLayer);

        for(int i = 0; i < count; i++)
        {
            GameObject target = results[i].gameObject;
            IDamageable damageable = target.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    }
}
