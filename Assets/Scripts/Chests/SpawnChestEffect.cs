using UnityEngine;

public class SpawnChestEffect : MonoBehaviour
{
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private AudioSource audioSource;

    private Vector3 chestOffset = new(0f, -1.32f, 0f);
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayAudio() {
        audioSource.Play();
    }

    public void SpawnChest() {
        Instantiate(chestPrefab, transform.position + chestOffset, Quaternion.identity);
    }

    public void SetInvisible() {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}