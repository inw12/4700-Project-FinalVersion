using UnityEngine;

public class SingleShot : MonoBehaviour, IBossAttack
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpd = 15;
    [SerializeField] private AudioSource audioSrc;

    public void Attack(Transform bulletSpawn) {
        if (audioSrc) audioSrc.Play();

        Vector2 targetDirection = Player.Instance.transform.position - bulletSpawn.position;
        GameObject newBullet = Instantiate(projectilePrefab, bulletSpawn.position, Quaternion.identity);
        newBullet.transform.right = targetDirection;

        if (newBullet.TryGetComponent(out BossProjectile projectile)) {
            projectile.UpdateProjectileSpeed(projectileSpd);                    
        }
    }
}
