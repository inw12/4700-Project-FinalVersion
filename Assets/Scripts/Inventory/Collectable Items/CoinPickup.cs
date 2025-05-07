using System.Collections;
using UnityEngine;

public class CoinPickup : MonoBehaviour, ICollectableItem
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryItem item;
    [SerializeField] private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Collect() {
        if (playerInventory) {
            StartCoroutine(CollectRoutine());
        }
    }

    private void SetInvisible() {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }

    private IEnumerator CollectRoutine() {
        audioSource.Play();
        boxCollider2D.enabled = false;
        SetInvisible();

        // *--*  Item Effect  *--*
        item.runtimeAmountHeld++;   

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);    
    }
}
