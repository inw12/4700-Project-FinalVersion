// *---*  this script is MOSTLY responsible for spawning/shooting projectiles in the ANIMATOR  *----------*

using System.Collections;
using UnityEngine;

public class FinalBoss : Enemy
{
    // Bullet Prefabs
    [SerializeField] private GameObject bullet1;    // for single shot
    [SerializeField] private GameObject bullet2;    // for shotgun bursts
    [SerializeField] private GameObject bullet3;    // for AoE blast
    [SerializeField] private float bullet1Speed = 14;
    [SerializeField] private float bullet2Speed = 8;
    [SerializeField] private float bullet3Speed = 8;
    [SerializeField] private AudioSource shoot1;
    [SerializeField] private AudioSource shoot2;
    [SerializeField] private AudioSource shoot3;
    [SerializeField] private Signal deathSignal;

    private readonly float shotgunSpread = 80;      // spread angle of shotgun burst
    private readonly int bulletsPerBurst = 7;       // bullets per shotgun burst
    private readonly float circleBlastSpread = 359; // spread angle of AoE blast
    private readonly int bulletsPerBlast = 32;      // bullets per blast hit
    private readonly int burstCount = 1;
    private readonly int blastCount = 1;
    private readonly float startingDistance = 0.1f;
    private readonly float timeBeweenBursts = 0.24f;

    public bool canMove = true;

    public override void CheckForDeath() {
        if (HP <= 0 && isAlive) {
            isAlive = false;
            canMove = false;
            if (deathSignal) deathSignal.Raise();
        }
    }

    public void ShootSingleShot() {
        if (Player.Instance.gameObject && isAlive) {
            Vector2 targetDirection = Player.Instance.transform.position - transform.position;
            if (shoot1) shoot1.Play();
            GameObject newBullet = Instantiate(bullet1, transform.position, Quaternion.identity);
            newBullet.transform.right = targetDirection;
            if (newBullet.TryGetComponent(out BossProjectile projectile)) {
                projectile.UpdateProjectileSpeed(bullet1Speed);                    
            }
        }
    }

    public void ShootShotgunBurst() {
        if (Player.Instance.gameObject && isAlive) {
            StartCoroutine(RadialAttackRoutine(bullet2, bullet2Speed, burstCount, bulletsPerBurst, shotgunSpread, shoot2));
        }
    }

    public void ShootCircleBlast() {
        if (Player.Instance.gameObject && isAlive) {
            StartCoroutine(RadialAttackRoutine(bullet3, bullet3Speed, blastCount, bulletsPerBlast, circleBlastSpread, shoot3));
        }
    }

    private IEnumerator RadialAttackRoutine(GameObject bullet, float projectileSpeed, int shotCount, int bulletsPerShot, float angleSpread, AudioSource audio) {
        TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, bulletsPerShot, angleSpread);
        for (int i = 0; i < shotCount; i++) {
            for (int j = 0; j < bulletsPerShot; j++) {
            
                Vector2 pos = FindBulletSpreadPosition(currentAngle);
                if (audio) audio.Play();
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
