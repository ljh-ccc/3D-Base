using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_New", menuName = "RPG Data/Weapon")]
[System.Serializable]
public class WeaponData : ScriptableObject
{
    [Header("Default Data")]
    public string weaponId;
    public string weaponName;
    public WeaponType weaponType;
    public Sprite icon;

    [Header("Combat Data")]
    public int damage;
    public float attackRange;
    public float attackRate;
    public float criticalChance;
    public float criticalMultiplier = 1.5f;

    [Header("Ranged Weapon")]
    public int magazineSize;
    public float reloadTime;
    public bool useAmmo;

    [Header("Prefab and Effects")]
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
    public ParticleSystem fireEffect;
    public AudioClip attackSound;

}
