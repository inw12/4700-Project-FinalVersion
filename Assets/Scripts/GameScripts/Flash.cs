using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlash;   // indicates damage taken
    [SerializeField] private Material redFlash;     // indicates death
    [SerializeField] private float duration = .1f;  // duration of the flash

    private SpriteRenderer spriteRenderer;
    private Material defaultMat;
    private Color defaultColor;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        defaultMat = spriteRenderer.material;
    }

    public void ResetSpriteColor() {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    public IEnumerator WhiteFlashRoutine() {
        if (spriteRenderer) {
            // 1. Reset color of sprite
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            // 2. Set material
            spriteRenderer.material = whiteFlash;
            // 3. Set flash duration
            yield return new WaitForSeconds(duration);
            // 4. Restore defaults
            spriteRenderer.color = defaultColor;
            spriteRenderer.material = defaultMat;
        }
    }

    public IEnumerator RedFlashRoutine() {
        // 1. Reset color of sprite
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        // 2. Set material
        spriteRenderer.material = redFlash;
        // 3. Set flash duration
        yield return new WaitForSeconds(duration);
        // 4. Restore defaults
        spriteRenderer.color = defaultColor;
        spriteRenderer.material = defaultMat;
    }
}
