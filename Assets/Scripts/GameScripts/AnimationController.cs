// **************************************************************************************************
// ! ---  For special effects that occur independently/concurrently with other game objects  --- !  
// **************************************************************************************************

using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    public void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio() {
        if (audioSource) audioSource.Play();
    }

    public void SetInvisible() {
        if (spriteRenderer) spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }

    public void DestroyObject() {
        if (gameObject) Destroy(gameObject);
    }
}
