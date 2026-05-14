using UnityEngine;
using UnityEngine.InputSystem;

public class ItemUseTester : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] ItemData itemData;
    [SerializeField] InputAction useItemAction = new InputAction(name: "UseItem", InputActionType.Button, binding: "<Keyboard>/u");

    private void OnEnable()
    {
        useItemAction.performed += OnUseItemPerformed;
        useItemAction.Enable();
    }

    private void OnDisable()
    {
        useItemAction.Disable();
        useItemAction.performed -= OnUseItemPerformed;
    }

    void OnUseItemPerformed(InputAction.CallbackContext context)
    {
        UseItem(itemData);
    }

    void UseItem(ItemData itemData)
    {
        if(itemData == null)
        {
            Debug.LogWarning("[ItemUseTester] Don't have require item");
            return;
        }

        if (!itemData.canUse)
        {
            Debug.Log($"[ItemUseTester] {itemData.itemName} is can't use");
            return;
        }

        Debug.Log($"[ItemUseTester] Use Item : {itemData.itemName}");

        foreach(ItemEffect effect in itemData.effects)
        {
            ApplyEffect(effect);
        }
    }

    void ApplyEffect(ItemEffect effect)
    {
        switch (effect.effectType)
        {
            case ItemEffectType.HealHP:
                playerStatus.HealHp(effect.value);
                break;
            case ItemEffectType.HealMP:
                playerStatus.HealMp(effect.value);
                break;
            case ItemEffectType.IncreaseAttack:
                playerStatus.IncreaseAttack(effect.value);
                break;
            case ItemEffectType.IncreaseDefense:
                playerStatus.IncreaseDefense(effect.value);
                break;
            case ItemEffectType.AddGold:
                playerStatus.AddGold(effect.value);
                break;
        }
    }
}
