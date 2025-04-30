using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpd;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private AudioSource audioSource;

    public void Attack() {
        GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPoint.position, Weapon.Instance.transform.rotation);
        audioSource.Play();

        if (bullet.TryGetComponent(out Projectile projectile)) {
            projectile.UpdateProjectileSpeed(projectileSpd);                    
        } 
    }
}
