using UnityEngine;
using UnityEngine.InputSystem;

public class _WeaponTestController : MonoBehaviour
{
    [Header("Weapon List")]
    [SerializeField] _WeaponData[] weapons;

    [Header("Input (Input System)")]
    [SerializeField] InputAction equipWeapon1Action = new InputAction(name: "EquipWeapon1", InputActionType.Button, binding: "<Keyboard>/1");
    [SerializeField] InputAction equipWeapon2Action = new InputAction(name: "EquipWeapon2", InputActionType.Button, binding: "<Keyboard>/2");
    [SerializeField] InputAction equipWeapon3Action = new InputAction(name: "EquipWeapon3", InputActionType.Button, binding: "<Keyboard>/3");
    [SerializeField] InputAction equipWeapon4Action = new InputAction(name: "EquipWeapon4", InputActionType.Button, binding: "<Keyboard>/4");
    [SerializeField] InputAction attackAction = new InputAction(name: "Attack", InputActionType.Button, binding: "<Mouse>/leftButton");
    [SerializeField] InputAction reloadAction = new InputAction(name: "Reload", InputActionType.Button, binding: "<Keyboard>/r");

    _WeaponTester currentWeapon;
    _PlayerData playerData;
    int currentWeaponIndex;
    float nextAttackTime;

    private void Awake()
    {
        if(playerData == null) { playerData = GetComponent<_PlayerData>(); }
    }

    private void OnEnable()
    {
        equipWeapon1Action.performed += OnEquipWeapon1Performed;
        equipWeapon2Action.performed += OnEquipWeapon2Performed;
        equipWeapon3Action.performed += OnEquipWeapon3Performed;
        equipWeapon4Action.performed += OnEquipWeapon4Performed;
        attackAction.performed += OnAttackPerformed;
        reloadAction.performed += OnReloadPerformed;

        equipWeapon1Action.Enable();
        equipWeapon2Action.Enable();
        equipWeapon3Action.Enable();
        equipWeapon4Action.Enable();
        attackAction.Enable();
        reloadAction.Enable();
    }

    private void OnDisable()
    {
        equipWeapon1Action.Disable();
        equipWeapon2Action.Disable();
        equipWeapon3Action.Disable();
        equipWeapon4Action.Disable();
        attackAction.Disable();
        reloadAction.Disable();

        equipWeapon1Action.performed -= OnEquipWeapon1Performed;
        equipWeapon2Action.performed -= OnEquipWeapon2Performed;
        equipWeapon3Action.performed -= OnEquipWeapon3Performed;
        equipWeapon4Action.performed -= OnEquipWeapon4Performed;
        attackAction.performed -= OnAttackPerformed;
        reloadAction.performed -= OnReloadPerformed;
    }

    void OnEquipWeapon1Performed(InputAction.CallbackContext context)
    {
        EquipWeapon(index: 0);
    }

    void OnEquipWeapon2Performed(InputAction.CallbackContext context)
    {
        EquipWeapon(index: 1);
    }

    void OnEquipWeapon3Performed(InputAction.CallbackContext context)
    {
        EquipWeapon(index: 2);
    }

    void OnEquipWeapon4Performed(InputAction.CallbackContext context)
    {
        EquipWeapon(index: 3);
    }

    void OnAttackPerformed(InputAction.CallbackContext context)
    {
        TryAttack();
    }

    void OnReloadPerformed(InputAction.CallbackContext context)
    {
        Reload();
    }

    void EquipWeapon(int index)
    {
        if (weapons == null || weapons.Length == 0) return;
        if (index < 0 || index >= weapons.Length) return;
        if (playerData == null) return;
        
        _WeaponTester newWeapon = new _WeaponTester(playerData, weapons[index]);

        if (!newWeapon.canUse) { return; }
        
        currentWeaponIndex = index;
        currentWeapon = newWeapon;

        string color = currentWeapon.weaponData.meleeWeapon ? "green" : "cyan";
        Debug.Log($"[_WeaponTestController] Weapon Change : <color={color}>{currentWeapon.weaponData.itemName}</color>"); 
    }

    void TryAttack()
    {
        if (currentWeapon == null) return;

        if (Time.time < nextAttackTime)
        {
            Debug.Log("[_WeaponTestController] Attack Cooltime");
            return;
        }

        if (!currentWeapon.HasAmmo())
        {
            Debug.Log("[_WeaponTestController] <color=white>Press to 'R'.</color> Reload Ammo");
            return;
        }

        nextAttackTime = Time.time + (1f / currentWeapon.weaponData.speed);

        currentWeapon.ConsumeAmmo();

        int finalDamage = CalculateDamage();

        if (currentWeapon.weaponData.rangedWeapon)
        {
            Debug.Log($"[_WeaponTestController] \n" +
            $"<color=cyan>{currentWeapon.weaponData.itemName}</color> : " +
            $"Damage: <color=cyan>{finalDamage}</color>," +
            $"Range: <color=cyan>{currentWeapon.weaponData.range}</color>," +
            $"Ammo: <color=cyan>{currentWeapon.currentAmmo}</color>");
        }
        else if(currentWeapon.weaponData.meleeWeapon)
        {
            Debug.Log($"[_WeaponTestController] \n" +
            $"<color=green>{currentWeapon.weaponData.itemName}</color> : " +
            $"Damage: <color=green>{finalDamage}</color>");
        }
    }

    int CalculateDamage()
    {
        int damage = currentWeapon.weaponData.damage;

        float randomValue = Random.value;
        if (randomValue <= currentWeapon.weaponData.criticalRatio)
        {
            damage = Mathf.RoundToInt(f: damage * currentWeapon.weaponData.criticalMultiplier);
            Debug.Log("[_WeaponTestController] <color=orange>Critical</color>");
        }
        return damage;
    }
    void Reload()
    {
        if (currentWeapon == null) { return; }
        if (currentWeapon.weaponData.meleeWeapon) 
        { 
            Debug.Log($"[_WeaponTestController] Can't use this function : <color=red>Reload</color>"); 
            return; 
        }
        if(currentWeapon.currentAmmo == currentWeapon.weaponData.magazineSize)
        {
            Debug.Log($"[_WeaponTestController] <color=orange>Ammo is already max</color>");
            return;
        }

        currentWeapon.Reload();

        Debug.Log($"[_WeaponTestController] {currentWeapon.weaponData.itemName} Reload / " + $"Ammo: <color=cyan>{currentWeapon.currentAmmo}</color>");
    }
}
