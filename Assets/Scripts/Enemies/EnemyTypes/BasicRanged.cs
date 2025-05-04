using System.Collections;
using UnityEngine;

public class BasicRanged : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float projectileSpd = 10f;
    [SerializeField] private AudioSource audioSource;

    private Enemy enemy;

    private void Awake() {
        enemy = GetComponent<Enemy>();        
    }

    public void Attack() {
        if (Player.Instance != null && enemy.isAlive) {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine() {
        yield return new WaitForSeconds(Random.Range(0f, 0.33f));
        Vector2 targetDirection = Player.Instance.transform.position - transform.position;
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.transform.right = targetDirection;
        audioSource.Play();

        if (newBullet.TryGetComponent(out EnemyProjectile projectile)) {
            projectile.UpdateProjectileSpeed(projectileSpd);
        }
    }
}
