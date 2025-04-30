using System.Collections;
using UnityEngine;

public class ShotgunRanged : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float projectileSpd = 8f;
    [SerializeField] private int burstCount = 1;
    [SerializeField] private int bulletsPerBurst = 8;
    [SerializeField] [Range(0,359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBeweenBursts = 0.1f;

    [SerializeField] private AudioSource audioSource;
    
    private Enemy enemy;

    private void Awake() {
        enemy = GetComponent<Enemy>();        
    }

    public void Attack() {
        if (Player.Instance.gameObject && enemy.isAlive) {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine() {
        TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep);

        yield return new WaitForSeconds(Random.value);
        for (int i = 0; i < burstCount; i++) {
            for (int j = 0; j < bulletsPerBurst; j++) {
            
                Vector2 pos = FindBulletSpreadPosition(currentAngle);
                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                audioSource.Play();

                if (newBullet.TryGetComponent(out EnemyProjectile projectile)) {
                    projectile.UpdateProjectileSpeed(projectileSpd);                    
                }

                currentAngle += angleStep;
            }

            yield return new WaitForSeconds(timeBeweenBursts);
            TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep);
        }
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep) {
        Vector2 targetDirection = Player.Instance.transform.position - transform.position;
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

    private Vector2 FindBulletSpreadPosition(float currentAngle) {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        UnityEngine.Vector2 pos = new(x, y);

        return pos;
    }
}
