using System;
using System.Collections;
using UnityEngine;

public class TwoPointScan : MonoBehaviour
{
    private struct AimResult
    {
        public Ray ray;
        public bool didHit;
        public Vector3 point;
        public RaycastHit hit;
    }

    private struct ShotResult
    {
        public Vector3 origin;
        public Vector3 direction;
        public float distance;
        public bool didHit;
        public RaycastHit hit;
    }

    [SerializeField] Camera aimCamera;
    [SerializeField] Transform muzzlePoint;
    [SerializeField] float aimRange = 30f;
    [SerializeField] float shotRange = 30f;
    [SerializeField] int damage = 10;

    [SerializeField] LayerMask aimMask;
    [SerializeField] LayerMask shotMask;
    [SerializeField] LayerMask muzzleBlockMask;

    [SerializeField] float shotRadius = 0f;
    [SerializeField] float muzzleBlockRadius = 0.15f;

    Vector3 screenCenter;
    bool isDead;
    bool wasMuzzleBlocked;

    WaitForSeconds coolTime;

    public event Action<int> OnColor;

    private void Awake()
    {
        if(aimCamera == null) { aimCamera = Camera.main; }
        screenCenter = new Vector3(0.5f, 0.5f, 0f);
        isDead = false;
        wasMuzzleBlocked = false;
        coolTime = new WaitForSeconds(0.5f);
    }

    private void Start()
    {
        StartShootingRay();
    }

    void StartShootingRay()
    {
        if (aimCamera == null) { Debug.LogError("[ShootingRay] mainCamera is null"); return; }

        StartCoroutine(ShootingAimRayCoroutine());
    }

    IEnumerator ShootingAimRayCoroutine()
    {
        while(!isDead)
        {
            if (aimCamera == null || muzzlePoint == null) { Debug.LogError("[TwoPointScan] aimCamera or muzzlePoint is null"); yield break; }

            if (IsMuzzleBlocked()) 
            {
                OnColor?.Invoke(2);

                if (!wasMuzzleBlocked)
                {
                    Debug.Log("[TwoPointScan] scan failed : muzzle is too close to obstacle");
                    wasMuzzleBlocked = true;
                }
                yield return null; 
                continue; 
            }
            wasMuzzleBlocked = false;

            AimResult aimResult = ResolveAimPoint();
            ShotResult shotResult = FireFromMuzzle(aimResult);

            AimColor(aimResult, shotResult);

            DrawDebugRay(aimResult, shotResult);

            yield return coolTime;
        } 
    }

    AimResult ResolveAimPoint()
    {
        Ray aimRay = aimCamera.ViewportPointToRay(screenCenter);

        AimResult result =
            new AimResult
            {
                ray = aimRay,
                didHit = false,
                point = aimRay.GetPoint(aimRange)
            };

        if(Physics.Raycast(aimRay, out RaycastHit hit,aimRange, aimMask, QueryTriggerInteraction.Ignore))
        {
            result.didHit = true;
            result.hit = hit;
            result.point = hit.point;
        }

        return result;
    }

    ShotResult FireFromMuzzle(AimResult aimResult)
    {
        Vector3 toAimPoint = aimResult.point - muzzlePoint.position;

        if(toAimPoint.sqrMagnitude < 0.0001f)
        {
            toAimPoint = aimCamera.transform.forward;
        }
        

        Vector3 shotDirection = toAimPoint.normalized;
        float distanceToAimPoint = toAimPoint.magnitude;

        float castDistance = aimResult.didHit ? Mathf.Min(shotRange, distanceToAimPoint + 0.05f) : shotRange;

        ShotResult result =
            new ShotResult
            {
                origin = muzzlePoint.position,
                direction = shotDirection,
                distance = castDistance,
                didHit = false
            };

        if(CastShot(muzzlePoint.position, shotDirection, castDistance, out RaycastHit shotHit))
        {
            result.didHit = true;
            result.hit = shotHit;
        }

        return result;
    }

    bool CastShot(Vector3 origin, Vector3 direction, float distance, out RaycastHit hit)
    {
        if(shotRadius > 0f)
        {
            return
                Physics.SphereCast(origin, shotRadius, direction, out hit, distance, shotMask, QueryTriggerInteraction.Ignore);
        }

        return
            Physics.Raycast(origin, direction, out hit, distance, shotMask, QueryTriggerInteraction.Ignore);
    }

    bool IsMuzzleBlocked()
    {
        //if (checkMuzzleBlocked == false)
        //{
        //    return false;
        //}
            
        return Physics.CheckSphere(muzzlePoint.position, muzzleBlockRadius, muzzleBlockMask, QueryTriggerInteraction.Ignore);
    }

    void HandleHit(RaycastHit hit, AimResult aimResult)
    {
        string aimName = aimResult.didHit ? aimResult.hit.collider.name : "¾øÀ½";

        string shotName = hit.collider.name;

        Debug.Log($"[TwoPointScan] Camera aim : {aimName} / Hit : {shotName}");

        IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();

        if(damageable != null) 
        {
            damageable.TakeDamage(damage);
        }
    }

    public void Fire()
    {
        if (aimCamera == null || muzzlePoint == null) { Debug.LogWarning("[TwoPointScan] aimCamera or muzzlePoint is null"); return; }

        if (IsMuzzleBlocked()) 
        {
            OnColor?.Invoke(2);
            Debug.Log("[TwoPointScan] shot failed : muzzle is too close to obstacle"); 
            return; 
        }

        AimResult aimResult = ResolveAimPoint();
        ShotResult shotResult = FireFromMuzzle(aimResult);

        AimColor(aimResult, shotResult);

        DrawDebugRay(aimResult, shotResult);

        if (shotResult.didHit)
        {
            HandleHit(shotResult.hit, aimResult);
        }
    }

    void AimColor(AimResult aimResult, ShotResult shotResult)
    {
        if(!aimResult.didHit || !shotResult.didHit)
        {
            OnColor?.Invoke(1);
            return;
        }


        if ((((1 << aimResult.hit.collider.gameObject.layer) & muzzleBlockMask) != 0)
            || (((1 << shotResult.hit.collider.gameObject.layer) & muzzleBlockMask) != 0))
        {
            OnColor?.Invoke(2);
            return;
        }
        
        if (aimResult.hit.collider == shotResult.hit.collider)
        {
            OnColor?.Invoke(0);
        }
        else if (aimResult.hit.collider != shotResult.hit.collider)
        {
            OnColor?.Invoke(1);
        }
        
    }

    void DrawDebugRay(AimResult aim, ShotResult shot)
    {
        float aimDistance = aim.didHit ? aim.hit.distance : aimRange;
        Debug.DrawRay(aim.ray.origin, aim.ray.direction * aimDistance, Color.cyan, 0.5f);

        float shotDistance = shot.didHit ? shot.hit.distance : shotRange;
        Debug.DrawRay(shot.origin, shot.direction * shotDistance, shot.didHit ? Color.red : Color.yellow, 0.5f);
    }

    private void OnDrawGizmos()
    {
        if(muzzlePoint != null)
        {
            Gizmos.DrawWireSphere(muzzlePoint.position, muzzleBlockRadius);
        }
    }
}
