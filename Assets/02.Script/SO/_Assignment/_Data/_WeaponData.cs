using UnityEngine;

[CreateAssetMenu(fileName = "_Weapon_new", menuName = "Assignment/_Weapon")]
public class _WeaponData : ScriptableObject
{
    [Header("Default_Data")]
    public string itemId;
    public string itemName;
    public _ItemType itemType;
    public Sprite icon;

    [Header("Currency_Data")]
    public bool canSell;
    public int buyPrice;
    public int sellPrice;

    [Header("Consume")]
    public bool Consume;

    [Header("Stack")]
    public bool canStack;
    public int maxStackCount;

    [Header("Weapon_Type")]
    public _WeaponType _weaponType;
    public _JobType[] canUseJob = new _JobType[5];

    [Header("Weapon_Status")]
    public int damage;
    [Range(1f, 10f)]
    public float speed;
    public float criticalRatio;
    public float criticalMultiplier = 1.5f;

    [Header("Weapon_Classification")]
    public bool rangedWeapon;
    public bool meleeWeapon;

    [Header("Ranged_Weapon")]
    public bool useAmmo;
    public int magazineSize;
    public float reloadTime;
    public float range;

    //[Header("Special_Effect")]
    //public bool hasEffect;
    //public string effectName;
    //[TextArea]
    //public string effectDescription;
    //public List<_WeaponEffect> weaponEffects = new List<_WeaponEffect>();

    [Header("Prefab and Effects")]
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
    public ParticleSystem hitEffect;
    public AudioClip attackSound;
}
