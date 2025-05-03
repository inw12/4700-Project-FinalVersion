using System.Collections;
using UnityEngine;

public class HeartPickup : MonoBehaviour, ICollectableItem
{
    [SerializeField] private FloatValue playerHealth;
    [SerializeField] private AudioSource audioSource;

    private readonly float restoreAmount = 1f;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D circleCollider;
    
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<BoxCollider2D>();
    }

    public void Collect() {
        if (playerHealth) {
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
        playerHealth.runtimeValue += restoreAmount; 
        if (playerHealth.runtimeValue > 10f) {
            playerHealth.runtimeValue = 10f;
        }
        
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);    
    }
}
