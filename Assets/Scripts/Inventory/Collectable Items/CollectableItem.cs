// **************************************************************************
// ! ---  Items in the world that can be collected when walked over  --- !  
// **************************************************************************

using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private MonoBehaviour itemType;

    // Interaction when player walks over the item
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>() && !other.isTrigger) {
            (itemType as ICollectableItem).Collect();
        }
    }
} 