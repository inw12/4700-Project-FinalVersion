using Unity.VisualScripting;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private MonoBehaviour shopItem;
    
    private bool playerInRange;
    private PlayerControls playerControls;

    private void Awake() {
        playerControls = new PlayerControls();
        playerInRange = false;
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Start() {
        playerControls.Interaction.Interact.started += _ => TriggerPurchase();        
    }

    private void TriggerPurchase() {
        if (playerInRange) {
            (shopItem as IShopItem).Purchase();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Player>()) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.GetComponent<Player>()) {
            playerInRange = false;
        }
    }
}