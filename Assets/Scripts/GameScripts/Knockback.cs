// *----------*  FOR APPLYING KNOCKBACK ONTO OBJECTS  *------------------------------* 

using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool GettingKnockedBack { get; private set; }

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TriggerKnockback(Transform damageSource, float knockbackThrust, float knockbackDuration) {
        if (GettingKnockedBack) return;

        GettingKnockedBack = true;

        // Knockback direction/magnitude
        Vector2 direction = (transform.position - damageSource.position).normalized * knockbackThrust;

        // Apply Knockback
        rb.AddForce(direction, ForceMode2D.Impulse); 

        // Knockback "stagger"
        StartCoroutine(KnockbackRoutine(knockbackDuration));
    }

    private IEnumerator KnockbackRoutine(float knockbackDuration) {
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}
