using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private MonoBehaviour chestType;
    [SerializeField] private PlayerControls playerControls;

    private bool playerInRange = false;
    private bool isOpen;

    private void Start() {
        playerControls.Interaction.Interact.started += _ => OpenChest();        
    }

    private void Awake() {
        playerControls = new PlayerControls();
        isOpen = false;
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void OpenChest() {
        if (playerInRange && !isOpen) {
            // Open the chest
            isOpen = true;
            (chestType as IChest).OpenChest();
        }        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            playerInRange = false;
        }
    }
}
