using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float knockbackThrust = 2f;
    [SerializeField] private float knockbackDuration = 0.1f;
    [SerializeField] [Range(0, 100)] private float accuracy;

    private float damage = 1f;  
    private float projectileSpd = 10f;
    private float offset;

    private void Awake() {
        float min = (accuracy - 100) / 100;
        float max = (100 - accuracy) / 100;
        offset = Random.Range(min, max);
    }

    private void Update() {
        transform.Translate(projectileSpd * Time.deltaTime * new Vector3(1, offset, 0));
    }

    // used by other methods to manipulate projectile stats
    public void UpdateProjectile(float damage, float newSpeed) {
        this.damage = damage;
        projectileSpd = newSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // deal damage to enemies
        if (other.GetComponent<Enemy>()) {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.TakeDamage(damage, knockbackThrust, knockbackDuration);
                Destroy(gameObject);
            }
        }
        // destroy on contact w/ terrain
        else if (!other.isTrigger && !other.GetComponent<Player>()) {
            Destroy(gameObject);
        }
    }
}
