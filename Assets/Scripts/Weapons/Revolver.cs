using System.Collections;
using UnityEngine;

public class Revolver : MonoBehaviour, IWeapon
{
    public bool FacingLeft { get { return isFacingLeft; } set { isFacingLeft = value; } }
    [SerializeField] private float damage = 2f;
    [SerializeField] private float projectileSpd = 16f;
    [SerializeField] private float fireRate = 0.69f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private GameObject pickupableCounterpart;   // ** NEW **

    private AudioSource audioSource;
    private SpriteRenderer mySpriteRenderer;
    private float fireTimer = 0f;
    private bool isFacingLeft = false;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        AdjustFacingDirection();
        fireTimer += Time.deltaTime;
    }

    // drops currently equipped weapon on the ground
    public void Drop() {
        Vector3 offset = new(0f, -0.3f, 0f);
        GameObject droppedItem = Instantiate(pickupableCounterpart, Player.Instance.transform.position + offset, Quaternion.identity);
        BoxCollider2D boxCollider = droppedItem.GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        droppedItem.GetComponent<MonoBehaviour>().StartCoroutine(ColliderCooldownRoutine(boxCollider, droppedItem.transform.position));
    }
    // items that drop have their colliders disabled until the player walks away from it
    private IEnumerator ColliderCooldownRoutine(BoxCollider2D collider, Vector3 itemPosition) {
        while (Vector3.Distance(Player.Instance.transform.position, itemPosition) < 1) yield return null;
        collider.enabled = true;
    }

    // Attack method
    public void Attack() {
        if (fireTimer >= fireRate) {
            GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPoint.position, Weapon.Instance.transform.rotation);
            audioSource.Play();
            if (bullet.TryGetComponent(out Projectile projectile)) {
                projectile.UpdateProjectile(damage, projectileSpd);                    
            } 
            fireTimer = 0f;
        }
    }

    // Sprite flip method
    private void AdjustFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x) {
            mySpriteRenderer.flipY = true;
            FacingLeft = true;
        } else {
            mySpriteRenderer.flipY = false;
            FacingLeft = false;
        }
    }
}
