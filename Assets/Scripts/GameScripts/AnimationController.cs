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
        audioSource.Play();
    }

    public void SetInvisible() {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }

    public void DestroyObject() {
        Destroy(gameObject);
    }
}
