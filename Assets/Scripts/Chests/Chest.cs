using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private InventoryItem item;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject itemPickupEffect;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private PlayerControls playerControls;

    private Animator anim;
    private bool isOpen;
    private bool playerInRange = false;

    private bool itemPickupStarted = false;
    private bool despawnStarted = false;

    private void Awake() {
        playerControls = new PlayerControls();
        anim = GetComponent<Animator>();
        isOpen = false;
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void Start() {
        playerControls.Interaction.Interact.started += _ => OpenChest();        
    }

    public void PlayAudio() {
        audioSource.Play();
    }

    private void OpenChest() {
        if (playerInRange && !isOpen) {
            // Open the chest
            anim.SetBool("isOpen", true);
            isOpen = true;
            
            // Add item to player's inventory
            if (playerInventory) {
                item.runtimeAmount++;
            }

            // Play reward animation
            if (!itemPickupStarted) {
                StartCoroutine(ItemPickupRoutine());
            }

            // Despawn the chest
            if (!despawnStarted) {
                StartCoroutine(DestroyChestRoutine());
                despawnStarted = true;
            }
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

    private IEnumerator ItemPickupRoutine() {
        yield return new WaitForSeconds(0.22f);
        Vector3 offset = new(0f, 1f, 0f);
        Instantiate(itemPickupEffect, transform.position + offset, Quaternion.identity);
        itemPickupStarted = true;
    }

    private IEnumerator DestroyChestRoutine() {
        yield return new WaitForSeconds(1.25f);
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

}
