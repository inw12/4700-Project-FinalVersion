using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private int amount = 0;

    [NonSerialized] public int runtimeAmount;

    public void OnAfterDeserialize() {
        runtimeAmount = amount;
    } 

    public void OnBeforeSerialize() {}
}
