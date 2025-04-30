using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory/Player Inventory")]
public class PlayerInventory : ScriptableObject {
    public List<InventoryItem> inventoryList = new();
}