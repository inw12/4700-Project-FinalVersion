using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 1f;   
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

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Player>() && other.isTrigger) {
            Player.Instance.TakeDamage(damage, knockbackThrust, knockbackDuration);
            Destroy(gameObject);
        }
        else if (!other.isTrigger && !other.gameObject.GetComponent<Boss>() && !other.gameObject.GetComponent<Player>()) {
            Destroy(gameObject);
        }
    }
}

