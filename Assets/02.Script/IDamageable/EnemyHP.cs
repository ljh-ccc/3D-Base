using UnityEngine;

public class EnemyHP : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;

    public bool IsDead => currentHP <= 0;

    TextSpot spot;

    private void Awake()
    {
        currentHP = maxHP;
        spot = GetComponent<TextSpot>();
    }

    public void TakeDamage(int value)
    {
        Debug.Log($"[EnemyHP] {name} Try TakeDamage : {value}, currentHP : {currentHP}");

        if (IsDead || value == 0) { return; }

        currentHP = Mathf.Max(currentHP - value, 0);

        Debug.Log($"[EnemyHP] {name} took damage. HP : {currentHP}/{maxHP}");
        

        if (IsDead)
        {
            Debug.Log($"[EnemyHP] {name} is dead");
            spot.GetPoolObject("Die");
            gameObject.SetActive(false);
        }
        else
        {
            spot.GetPoolObject($"{value}");
        }
    }
}
