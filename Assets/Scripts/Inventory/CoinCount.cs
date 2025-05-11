using System.Collections;
using TMPro;
using UnityEngine;

public class CoinCount : MonoBehaviour
{
    [SerializeField] private InventoryItem coin;
    
    private TextMeshProUGUI itemAmountText;
    private Color defaultColor;

    private readonly float flashDuration = 0.12f;

    private void Awake() {
        itemAmountText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();     
        defaultColor = itemAmountText.color;
    }

    private void Update() {
        int amount = coin.runtimeAmountHeld;
        if (amount < 10) {
            itemAmountText.text = "0" + amount;
        } else {
            itemAmountText.text = "" + amount;
        }
    }

    public void GreenTextFlash() {
        StartCoroutine(ColorFlashRoutine(Color.green));
    }

    public void RedTextFlash() {
        StartCoroutine(ColorFlashRoutine(Color.red));
    }

    private IEnumerator ColorFlashRoutine(Color color) {
        itemAmountText.color = color;
        yield return new WaitForSeconds(flashDuration);
        itemAmountText.color = defaultColor;
    }
}