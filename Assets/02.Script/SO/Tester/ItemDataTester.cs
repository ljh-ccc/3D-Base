
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    private void Start()
    {
        if(itemData == null)
        {
            Debug.LogWarning("[ItemDataTester] ItemData is null");
            return;
        }

        Debug.Log($"[ItemDataTester] Item ID : {itemData.itemId}");
        Debug.Log($"[ItemDataTester] Item Name : {itemData.itemName}");
        Debug.Log($"[ItemDataTester] Item Type : {itemData.itemType}");
        Debug.Log($"[ItemDataTester] Buy Price : {itemData.buyPrice}");
        Debug.Log($"[ItemDataTester] Sell Price : {itemData.sellPrice}");
        Debug.Log($"[ItemDataTester] Can Stack : {itemData.canStack}");
    }
}
