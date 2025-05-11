// *---*  Upon purchase, player's HP will be restored  *-----*

using UnityEngine;

public class HeartShopItem : MonoBehaviour, IShopItem
{
    [SerializeField] private FloatValue playerHealth;
    [SerializeField] private InventoryItem coinInventoryItem;
    [SerializeField] private Signal successSignal;
    [SerializeField] private Signal failedSignal;

    private AudioSource[] sfx;
    private AudioSource purchaseSound;
    private AudioSource errorSound;

    private readonly Color defaultTextColor = Color.white;
    private readonly float restoreAmount = 1f;
    private readonly int itemCost = 2;  // price of purchasing a heart

    private void Awake() {
        sfx = GetComponents<AudioSource>();
        purchaseSound = sfx[0];
        errorSound = sfx[1];
    }

    public void Purchase() {
        // IF not enough coins...
        if (coinInventoryItem.runtimeAmountHeld < itemCost) {
            errorSound.Play();
            failedSignal.Raise();
            return;
        }
        // Otherwise...
        else {
            purchaseSound.Play();

            // *--*  Reduce player's coin count  *--*
            coinInventoryItem.runtimeAmountHeld -= itemCost;
            if (coinInventoryItem.runtimeAmountHeld < 0) {
                coinInventoryItem.Reset();
            }

            successSignal.Raise();

            // *--*  Purchase Effect  *--*
            playerHealth.runtimeValue += restoreAmount; 
            if (playerHealth.runtimeValue > 10f) {
                playerHealth.runtimeValue = 10f;
            }
        }
    }
}
