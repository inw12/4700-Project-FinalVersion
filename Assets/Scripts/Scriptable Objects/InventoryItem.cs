using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;

    [NonSerialized] public int runtimeAmountHeld;

    private readonly int defaultAmountHeld = 0;

    private void OnEnable() {
        ResetRuntimeAmount();
    }

    public void ResetRuntimeAmount() {
        runtimeAmountHeld = defaultAmountHeld;
    }
}