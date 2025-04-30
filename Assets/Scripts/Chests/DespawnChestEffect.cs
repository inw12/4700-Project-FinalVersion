using UnityEngine;

public class DespawnChestEffect : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;

    public void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayAudio() {
        audioSource.Play();
    }

    public void SetInvisible() {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
