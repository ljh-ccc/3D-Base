using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapon List")]
    [SerializeField] WeaponData[] weapons;

    [Header("Input (Input System)")]
    [SerializeField] InputAction equipWeapon1Action = new InputAction(name: "EquipWeapon1", InputActionType.Button, binding: "<Keyboard>/1");
    [SerializeField] InputAction equipWeapon2Action = new InputAction(name: "EquipWeapon2", InputActionType.Button, binding: "<Keyboard>/2");
    [SerializeField] InputAction equipWeapon3Action = new InputAction(name: "EquipWeapon3", InputActionType.Button, binding: "<Keyboard>/3");
    [SerializeField] InputAction attackAction = new InputAction(name: "Attack", InputActionType.Button, binding: "<Mouse>/leftButton");
    [SerializeField] InputAction reloadAction = new InputAction(name: "Reload", InputActionType.Button, binding: "<Keyboard>/r");

    WeaponRuntime currentWeapon;
    int currentWeaponIndex;
    float nextAttackTime;

    private void OnEnable()
    {
        equipWeapon1Action.performed += OnEquipWeapon1Performed;
        equipWeapon2Action.performed += OnEquipWeapon2Performed;
        equipWeapon3Action.performed += OnEquipWeapon3Performed;
        attackAction.performed += OnAttackPerformed;
        reloadAction.performed += OnReloadPerformed;

        equipWeapon1Action.Enable();
        equipWeapon2Action.Enable();
        equipWeapon3Action.Enable();
        attackAction.Enable();
        reloadAction.Enable();
    }

    private void OnDisable()
    {
        equipWeapon1Action.Disable();
        equipWeapon2Action.Disable();
        equipWeapon3Action.Disable();
        attackAction.Disable();
        reloadAction.Disable();

        equipWeapon1Action.performed -= OnEquipWeapon1Performed;
        equipWeapon2Action.performed -= OnEquipWeapon2Performed;
        equipWeapon3Action.performed -= OnEquipWeapon3Performed;
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
        if(weapons == null || weapons.Length == 0) return;
        if(index < 0 || index >= weapons.Length) return;

        currentWeaponIndex = index;
        currentWeapon = new WeaponRuntime(weapons[index]);

        Debug.Log($"무기 장착 : {currentWeapon.data.weaponName}");
    }

    void TryAttack()
    {
        if (currentWeapon == null) return;
        if(Time.time < nextAttackTime)
        {
            Debug.Log("공격 쿨타임 중입니다.");
            return;
        }

        if (!currentWeapon.HasAmmo())
        {
            Debug.Log("탄약이 없습니다. R키로 재장전 해주새요.");
            return;
        }

        nextAttackTime = Time.time + (1f / currentWeapon.data.attackRate);

        currentWeapon.ConsumeAmmo();

        int finalDamage = CalculateDamage();

        Debug.Log($"{currentWeapon.data.weaponName} 공격 / " +
            $"Damage: {finalDamage}," +
            $"Range: {currentWeapon.data.attackRange}," +
            $"Ammo: {currentWeapon.currentAmmo}");
    }

    int CalculateDamage()
    {
        int damage = currentWeapon.data.damage;

        float randomValue = Random.value;
        if(randomValue <= currentWeapon.data.criticalChance)
        {
            damage = Mathf.RoundToInt(f: damage * currentWeapon.data.criticalMultiplier);
            Debug.Log("Critical");
        }
        return damage;
    }
    void Reload()
    {
        if(currentWeapon == null) { return; }

        currentWeapon.Reload();

        Debug.Log($"{currentWeapon.data.weaponName} 재장전 완료 / " + $"Ammo: {currentWeapon.currentAmmo}");
    }
}
