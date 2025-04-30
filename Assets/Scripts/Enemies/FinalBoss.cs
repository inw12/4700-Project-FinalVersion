// *---*  this script is PURELY responsible for spawning/shooting projectiles in the ANIMATOR  *----------*

using System.Collections;
using UnityEngine;

public class FinalBoss : Enemy
{
    // Bullet Prefabs
    [SerializeField] private GameObject bullet1;    // for single shot
    [SerializeField] private GameObject bullet2;    // for shotgun bursts
    [SerializeField] private GameObject bullet3;    // for AoE blast

    [SerializeField] private float basicProjectileSpeed;

    // Shotgun
    [SerializeField] private float shotgunProjectileSpeed;
    [SerializeField] private int bulletsPerBurst = 6;
    [SerializeField] [Range(0,359)] private float shotgunSpread;

    // AoE Blast
    [SerializeField] private float blastProjectileSpeed;
    [SerializeField] private int bulletsPerBlast = 36;
    [SerializeField] [Range(0,359)] private float circleBlastSpread;

    private readonly int burstCount = 1;
    private readonly int blastCount = 1;
    private readonly float startingDistance = 0.1f;
    private readonly float timeBeweenBursts = 0.1f;

    private bool shooting1 = false;
    private bool shooting2 = false;
    private bool shooting3 = false;


    public void ShootSingleShot() {
        if (Player.Instance.gameObject && isAlive) {
            Vector2 targetDirection = Player.Instance.transform.position - transform.position;
            GameObject newBullet = Instantiate(bullet1, transform.position, Quaternion.identity);
            newBullet.transform.right = targetDirection;
            if (newBullet.TryGetComponent(out BossProjectile projectile)) {
                projectile.UpdateProjectileSpeed(basicProjectileSpeed);                    
            }
        }
    }

    public void ShootShotgunBurst() {
        if (Player.Instance.gameObject && isAlive) {
            StartCoroutine(RadialAttackRoutine(bullet2, shotgunProjectileSpeed, burstCount, bulletsPerBurst, shotgunSpread));
        }
    }

    public void ShootCircleBlast() {
        if (Player.Instance.gameObject && isAlive) {
            StartCoroutine(RadialAttackRoutine(bullet3, blastProjectileSpeed, blastCount, bulletsPerBlast, circleBlastSpread));
        }
    }

    private IEnumerator RadialAttackRoutine(GameObject bullet, float projectileSpeed, int shotCount, int bulletsPerShot, float angleSpread) {
        TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, bulletsPerShot, angleSpread);
        for (int i = 0; i < shotCount; i++) {
            for (int j = 0; j < bulletsPerShot; j++) {
            
                Vector2 pos = FindBulletSpreadPosition(currentAngle);
                GameObject newBullet = Instantiate(bullet, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out BossProjectile projectile)) {
                    projectile.UpdateProjectileSpeed(projectileSpeed);                    
                }

                currentAngle += angleStep;
            }

            yield return new WaitForSeconds(timeBeweenBursts);
            TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, bulletsPerShot, angleSpread);
        }
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, int bulletsPerShot, float angleSpread) {
        Vector2 targetDirection = Player.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        startAngle = targetAngle;
        currentAngle = targetAngle;
        float endAngle = targetAngle;

        float halfAngleSpread = 0f;
        angleStep = 0;

        if (angleSpread != 0) {
            angleStep = angleSpread / (bulletsPerShot - 1);
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
