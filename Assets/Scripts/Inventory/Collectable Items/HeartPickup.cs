using System.Collections;
using UnityEngine;

public class HeartPickup : MonoBehaviour, ICollectableItem
{
    [SerializeField] private FloatValue playerHealth;
    [SerializeField] private AudioSource audioSource;

    private readonly float restoreAmount = 2f;
    private SpriteRenderer spriteRenderer;
    
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        // *--*  Item Effect  *--*
        playerHealth.runtimeValue += restoreAmount; 
        if (playerHealth.runtimeValue > 10f) {
            playerHealth.runtimeValue = 10f;
        }
        
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);    
    }
}
