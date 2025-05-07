using System.Collections;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, ICollectableItem
{
    [SerializeField] private GameObject weaponPrefab;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Collect() {
        if (weaponPrefab) {
            StartCoroutine(CollectRoutine());
        }
    }

    private void SetInvisible() {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
    }

    private IEnumerator CollectRoutine() {
        SetInvisible();
        boxCollider.enabled = false;

        // *--*  Item Effect  *--*
        Weapon.Instance.SwapCurrentWeapon(weaponPrefab);
        
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);    
    }
}
