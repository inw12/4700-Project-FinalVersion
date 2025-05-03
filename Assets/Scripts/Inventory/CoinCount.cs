using TMPro;
using UnityEngine;

public class CoinCount : MonoBehaviour
{
    [SerializeField] private InventoryItem coin;
    [SerializeField] private TextMeshProUGUI itemAmountText;

    private void Update() {
        int amount = coin.runtimeAmountHeld;

        if (amount < 10) {
            itemAmountText.text = "0" + amount;
        } else {
            itemAmountText.text = "" + amount;
        }

    }
}
