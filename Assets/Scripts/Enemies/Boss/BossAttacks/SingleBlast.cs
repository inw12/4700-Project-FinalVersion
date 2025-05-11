using UnityEngine;

public class SingleBlast : MonoBehaviour, IBossAttack
{
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private float projectileSpd = 12;
    [SerializeField] private AudioSource audioSrc;

    private readonly float spreadAngle = 360;   // spread angle of AoE blast
    private readonly int bulletsPerShot = 36;   // bullets per blast
    private readonly float startingDistance = 0.1f;

    public void Attack(Transform bulletSpawn) {
        TargetConeOfInfluence(bulletSpawn, out float startAngle, out float currentAngle, out float angleStep);
        for (int j = 0; j < bulletsPerShot; j++) {
            if (audioSrc) audioSrc.Play();
        
            Vector2 pos = FindBulletSpreadPosition(currentAngle, bulletSpawn);
            GameObject newBullet = Instantiate(projectilePrefab, pos, Quaternion.identity);
            newBullet.transform.right = newBullet.transform.position - bulletSpawn.position;

            if (newBullet.TryGetComponent(out BossProjectile projectile)) {
                projectile.UpdateProjectileSpeed(projectileSpd);                    
            }

            currentAngle += angleStep;
        }
    }

    // Calculates angles for projectiles to fly in a cone-pattern
    private void TargetConeOfInfluence(Transform bulletSpawn, out float startAngle, out float currentAngle, out float angleStep) {
        Vector2 targetDirection = Player.Instance.transform.position - bulletSpawn.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        startAngle = targetAngle;
        currentAngle = targetAngle;
        float endAngle = targetAngle;

        float halfAngleSpread = 0f;
        angleStep = 0;

        if (spreadAngle != 0) {
            angleStep = spreadAngle / (bulletsPerShot - 1);
            halfAngleSpread = spreadAngle / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    // Calculates projectile spawn position
    private Vector2 FindBulletSpreadPosition(float currentAngle, Transform bulletSpawn) {
        float x = bulletSpawn.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = bulletSpawn.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new(x, y);

        return pos;
    }
}
