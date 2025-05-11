using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject, IResettable
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;

    [NonSerialized] public int runtimeAmountHeld;

    private readonly int defaultAmountHeld = 0;

    private void OnEnable() {
        Reset();
    }

    public void Reset() {
        runtimeAmountHeld = defaultAmountHeld;
    }
}