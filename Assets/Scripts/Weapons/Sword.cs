using UnityEngine;

public class Melee : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private Transform hitboxSpawnPoint;
    [SerializeField] private Transform stickTo; // who is swinging the sword?
    [SerializeField] private AudioSource audioSource;

    public void Attack() {
        audioSource.Play();
        GameObject hitbox = Instantiate(hitboxPrefab, hitboxSpawnPoint.position, Weapon.Instance.transform.rotation);
        hitbox.GetComponent<FollowPlayer>().SetTarget(stickTo);
    }
}
