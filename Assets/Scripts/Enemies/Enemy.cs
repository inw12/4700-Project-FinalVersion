using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance { get; private set; }

    [SerializeField] private float HP = 5;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private GameObject deathFX;
    [SerializeField] private AudioSource deathSFX;

    [SerializeField] private List<GameObject> itemPool;     // list of items that may potentially drop on death
    private readonly float dropChance = 0.33f;   // 33% item drop chance

    private Rigidbody2D rigidBody;
    private Collider2D hurtbox;
    private Animator anim;
    private Vector2 movementDirection;
    private Knockback knockback;
    private Flash flash;
    private bool deathAnimationStarted = false;

    // Damage values for when players come into contact w/ enemy
    private readonly float baseAttack = 2f;   
    private readonly float knockbackThrust = 5f;   
    private readonly float knockbackDuration = 0.1f;   
    
    public bool isAlive;

    private void Awake() {
        Instance = this;
        rigidBody = GetComponent<Rigidbody2D>();
        hurtbox = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
        isAlive = true;
    }

    private void FixedUpdate() {
        // Dis-allow movement if enemy is getting knocked back or is attacking 
        if (knockback.GettingKnockedBack || anim.GetBool("isMoving") == false) {
            return;       
        }
        // Otherwise, trigger enemy movement
        else {
            rigidBody.MovePosition(rigidBody.position + movementDirection * (moveSpeed * Time.fixedDeltaTime));
        }
    }

    // Update enemy movement direction
    public void MoveTo(Vector2 targetPosition) {
        anim.SetBool("isMoving", true);
        movementDirection = targetPosition;
        anim.SetFloat("moveX", movementDirection.x);
        anim.SetFloat("moveY", movementDirection.y);
    }

    // Stop enemy movement
    public void StopMoving() {
        anim.SetBool("isMoving", false);
        movementDirection = Vector3.zero;
    }

    public virtual void CheckForDeath() {
        if (HP <= 0) {
            isAlive = false;
            hurtbox.enabled = false;
            StopMoving();         
            if (deathSFX) deathSFX.Play();
            StartCoroutine(flash.RedFlashRoutine());
            knockback.TriggerKnockback(Player.Instance.transform, 8f, 0.15f);
            anim.Play("Death");
        }
    }

    private void RandomDrop() {
        // Random chance roll
        float randRoll = Random.value;
        // Random item selection
        int i = Random.Range(0, itemPool.Count);

        if (randRoll < dropChance) {
            // Pull from random item from list
            Instantiate(itemPool[i], transform.position, Quaternion.identity);
        }
    }

    // For when player comes into contact w/ enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
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

    // Damage/HP Manager
    public void TakeDamage(float receivedDamage, float knockbackThrust, float knockbackDuration)
    {
        HP -= receivedDamage;
        CheckForDeath();

        // Apply damage effects if not dead
        if (HP > 0) {
            // damage flash
            StartCoroutine(flash.WhiteFlashRoutine());
            // knockback
            knockback.TriggerKnockback(Player.Instance.transform, knockbackThrust, knockbackDuration);
        }
    }

    public void DestroyObject() {
        if (!deathAnimationStarted) {
            deathAnimationStarted = true;
            if (deathFX) Instantiate(deathFX, transform.position, Quaternion.identity);
            RandomDrop();
            Destroy(gameObject);
        }
    }
}
