using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{
    public bool FacingLeft { get { return isFacingLeft; } set { isFacingLeft = value; } }
    [SerializeField] private float damage = 1f;
    [SerializeField] private float projectileSpd = 10f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] [Range(0,180)] private float angleSpread;
    [SerializeField] private int bulletsPerBurst = 8;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private GameObject pickupableCounterpart;   // ** NEW **

    private AudioSource audioSource;
    private SpriteRenderer mySpriteRenderer;
    private float fireTimer = 0f;
    private bool isFacingLeft = false;

    private readonly float startingDistance = 0.1f;

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
        while (Vector3.Distance(Player.Instance.transform.position, itemPosition) < 1.25f) yield return null;
        collider.enabled = true;
    }

    // Attack method
    public void Attack() {
        if (fireTimer >= fireRate) {
            TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep);
            for (int j = 0; j < bulletsPerBurst; j++) {
            
                Vector2 pos = FindBulletSpreadPosition(currentAngle);
                GameObject newBullet = Instantiate(projectilePrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                audioSource.Play();

                if (newBullet.TryGetComponent(out Projectile projectile)) {
                    projectile.UpdateProjectile(damage, projectileSpd);                    
                }

                currentAngle += angleStep;
            }
            fireTimer = 0f;
        }
    }

    // Calculates angles for projectiles to fly in a cone-pattern
    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep) {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 targetDirection = -(transform.position - mousePos);

        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        startAngle = targetAngle;
        currentAngle = targetAngle;
        float endAngle = targetAngle;

        float halfAngleSpread = 0f;
        angleStep = 0;

        if (angleSpread != 0) {
            angleStep = angleSpread / (bulletsPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    // Calculates projectile spawn position
    private Vector2 FindBulletSpreadPosition(float currentAngle) {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        UnityEngine.Vector2 pos = new(x, y);

        return pos;
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
