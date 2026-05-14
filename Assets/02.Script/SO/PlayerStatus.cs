using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("HP")]
    public int maxHp = 100;
    public int currentHp = 50;

    [Header("MP")]
    public int maxMp = 50;
    public int currentMp = 10;

    [Header("Combat")]
    public int attack = 10;
    public int defense = 5;

    [Header("Currency")]
    public int gold = 0;

    public void HealHp(int amount)
    {
        currentHp = Mathf.Min(currentHp + amount, maxHp);
        Debug.Log($"[PlayerState] Heal HP : +{amount}, Current HP : {currentHp}/{maxHp}");
    }

    public void HealMp(int amount)
    {
        currentMp = Mathf.Min(currentMp + amount, maxMp);
        Debug.Log($"[PlayerState] Heal MP : +{amount}, Current MP : {currentMp}/{maxMp}");
    }

    public void IncreaseAttack(int amount)
    {
        attack += amount;
        Debug.Log($"[PlayerStatus] Add Attack : +{amount}, Current Attack : {attack}");
    }

    public void IncreaseDefense(int amount)
    {
        defense += amount;
        Debug.Log($"[PlayerStatus] Add Defense : +{amount}, Current Defense : {defense}");
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"[PlayerStatus] Add Gold : +{gold}, Current Gold : {gold}");
    }
}
