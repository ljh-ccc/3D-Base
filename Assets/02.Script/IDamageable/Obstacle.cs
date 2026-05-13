using UnityEngine;

public class Obstacle : MonoBehaviour, IDamageable
{
    [SerializeField] string msg;

    TextSpot spot;

    void Awake()
    {
        spot = GetComponent<TextSpot>();
    }

    public void TakeDamage(int value)
    {
        spot.GetPoolObject(msg);
    }
}
