using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private FloatValue damage;  
    [SerializeField] private float projectileSpd = 10f;
    [SerializeField] private float knockbackThrust = 2f;
    [SerializeField] private float knockbackDuration = 0.1f;

    private void Update() {
        MoveProjectile();
    }

    private void MoveProjectile() {
        transform.Translate(projectileSpd * Time.deltaTime * Vector3.right);
    }

    public void UpdateProjectileSpeed(float newSpeed) {
        projectileSpd = newSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>()) {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.TakeDamage(damage.value, knockbackThrust, knockbackDuration);
                Destroy(gameObject);
            }
        }
        else if (other.GetComponent<FinalBoss>()) {
            FinalBoss boss = other.GetComponent<FinalBoss>();
            if (boss != null) {
                boss.TakeDamage(damage.value, 0, 0);
                Destroy(gameObject);
            }
        }
        else if (!other.isTrigger) {
            Destroy(gameObject);
        }
    }
}

