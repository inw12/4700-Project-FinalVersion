using System.Collections.Generic;
using UnityEngine;

public class RandomizedWeaponShop : MonoBehaviour
{
    [SerializeField] private List<GameObject> shopInventory;

    private void Awake() {
        int index = Random.Range(0, shopInventory.Count);
        Instantiate(shopInventory[index], transform.position, Quaternion.identity, transform.parent);
    }
}
