using UnityEngine;
using UnityEngine.InputSystem;

public class CenterRaycastShooter : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Camera m_cam;

    [SerializeField] private LayerMask m_hittableMask;
    [SerializeField] private float m_maxDistance = 100.0f;

    private PlayerInput _pi;
    private InputAction _fire;

    private void Awake()
    {
        _pi = GetComponent<PlayerInput>();
        _fire = _pi.actions.FindAction("Fire", true);

        if(m_cam == null) { m_cam = Camera.main; }
    }


    private void OnEnable()
    {
        _fire.performed += OnRayFire;
    }

    private void OnDisable()
    {
        _fire.performed -= OnRayFire;
    }

    void OnRayFire(InputAction.CallbackContext _)
    {
        Vector2 _screenCenter = new (Screen.width * 0.5f, Screen.height * 0.5f);

        Ray _ray = m_cam.ScreenPointToRay(_screenCenter);

        if (Physics.Raycast(_ray, out RaycastHit hit, m_maxDistance, m_hittableMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"[CenterRaycastShooter] Hit {hit.collider.name} at {hit.point}");

            Debug.DrawLine(_ray.origin, hit.point, Color.green, 1.0f);
        }
        else
        {
            Debug.DrawLine(_ray.origin, _ray.direction * m_maxDistance, Color.yellow, 0.5f);
        }
    }


    void SphereCastExample()
    {
        float radius = 3f;
        float maxDistance = 10.0f;

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (Physics.SphereCast(origin, radius, direction, out RaycastHit hit, maxDistance, m_hittableMask))
        {
            Debug.Log($"[CenterRaycastShooter] Shpere Hit {hit.collider.name}");
        }
    }

    void OverlapExample()
    {
        Vector3 center = transform.position;
        float radius = 5.0f;

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        foreach(var hitCollider in hitColliders)
        {
            Debug.Log($"[CenterRaycastShooter] Detected : {hitCollider.name}");
        }
    }

    private Collider[] result = new Collider[10];

    void OptimizedOverlap()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, 5.0f, result);

        for (int i = 0; i < count; i++)
        {
            Debug.Log($"[CenterRaycastShooter] NonAlloc Hit : {result[i].name}");
        }

        //Physics.RaycastAll
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 10f, 3f);
    }
}
