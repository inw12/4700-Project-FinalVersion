using System.Collections;
using UnityEngine;

public class CoinPickup : MonoBehaviour, ICollectableItem
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryItem item;
    [SerializeField] private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
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
        SetInvisible();
        circleCollider.enabled = false;

        // *--*  Item Effect  *--*
        item.runtimeAmountHeld++;   

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);    
    }
}
