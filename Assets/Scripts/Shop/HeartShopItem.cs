// *---*  Upon purchase, player's HP will be restored  *-----*

using System.Collections;
using TMPro;
using UnityEngine;

public class HeartShopItem : MonoBehaviour, IShopItem
{
    [SerializeField] private FloatValue playerHealth;
    [SerializeField] private InventoryItem coinInventoryItem;
    [SerializeField] private TextMeshProUGUI countCointText;
    [SerializeField] private AudioSource purchaseSound;
    [SerializeField] private AudioSource errorSound;

    private readonly Color defaultTextColor = Color.white;
    private readonly float restoreAmount = 1f;
    private readonly int itemCost = 2;  // price of purchasing a heart

    public void Purchase() {
        // IF not enough coins...
        if (coinInventoryItem.runtimeAmountHeld < itemCost) {
            errorSound.Play();
            StartCoroutine(TextFlashRoutine(Color.red));
            return;
        }
        // Otherwise...
        else {
            purchaseSound.Play();

            // *--*  Reduce player's coin count  *--*
            coinInventoryItem.runtimeAmountHeld -= itemCost;
            if (coinInventoryItem.runtimeAmountHeld < 0) {
                coinInventoryItem.ResetRuntimeAmount();
            }

            StartCoroutine(TextFlashRoutine(Color.green));

            // *--*  Purchase Effect  *--*
            playerHealth.runtimeValue += restoreAmount; 
            if (playerHealth.runtimeValue > 10f) {
                playerHealth.runtimeValue = 10f;
            }
        }
    }

    private IEnumerator TextFlashRoutine(Color color) {
        countCointText.color = color;
        yield return new WaitForSeconds(0.1f);
        countCointText.color = defaultTextColor;
    }
}
