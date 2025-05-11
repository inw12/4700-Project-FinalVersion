// *---*  General script for boss-like enemies  *-------*
using UnityEngine;

public class Boss : MonoBehaviour
{
    // *--- BOSS VARIABLES ---* 
    public static Boss Instance { get; private set; }
    public bool isAlive;

    [SerializeField] private float HP = 100;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private Signal bossDeathSignal;

    private Rigidbody2D rigidBody;
    private Animator anim;
    private Vector2 movementDirection;
    private Knockback knockback;
    private Flash flash;
    
    private void Awake() {
        Instance = this;
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("isMoving", false);
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
        isAlive = true;
    }

    private void FixedUpdate() {
        // Dis-allow movement if enemy is getting knocked back or is attacking 
        if (knockback.GettingKnockedBack || anim.GetBool("isAttacking")) { return; }
        // Otherwise, trigger enemy movement
        rigidBody.MovePosition(rigidBody.position + movementDirection * (moveSpeed * Time.fixedDeltaTime));
    }


    // update movement
    public void MoveTo(Vector2 targetPosition)  
    {
        anim.SetBool("isMoving", true);
        movementDirection = targetPosition;
        anim.SetFloat("moveX", movementDirection.x);
        anim.SetFloat("moveY", movementDirection.y);
    }

    // stop movement
    public void StopMoving()    
    {
        anim.SetBool("isMoving", false);
        movementDirection = Vector3.zero;
    }

    // hp management
    public void TakeDamage(float receivedDamage, float knockbackThrust, float knockbackDuration) {
        HP -= receivedDamage;
        CheckForDeath();

        if (HP > 0) {
            StartCoroutine(flash.WhiteFlashRoutine());  // damage flash
            // < knockback >
        }
    }

    // check for boss death
    public void CheckForDeath() {
        if (HP <= 0 && isAlive) {
            isAlive = false;
            if (bossDeathSignal) bossDeathSignal.Raise();
        }
    }

    // damage player on contact
    private void OnTriggerEnter2D(Collider2D other) {
        float baseAttack = 2f;   
        float knockbackThrust = 10f;   
        float knockbackDuration = 0.1f;   

        if (other.gameObject.GetComponent<Player>()) {
            Player player = other.GetComponent<Player>();
            if (player != null) {
                player.TakeDamage(baseAttack, knockbackThrust, knockbackDuration);
            }
            else {
                Debug.LogWarning("Player object not found!");
            }
        }
    }
}