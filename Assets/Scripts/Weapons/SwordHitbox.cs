using System.Collections;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    [SerializeField] private FloatValue damage;
    [SerializeField] private float knockbackThrust;
    [SerializeField] private float knockbackDuration;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Enemy>()) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage.value, knockbackThrust, knockbackDuration);
        }
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }    
}

