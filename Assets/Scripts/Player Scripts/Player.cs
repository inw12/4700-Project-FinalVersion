using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerControls))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public bool FacingLeft { get { return isFacingLeft; } set { isFacingLeft = value; } }

    [SerializeField] private FloatValue HP;
    [SerializeField] private FloatValue moveSpeed;
    [SerializeField] private FloatValue dashSpeed;
    [SerializeField] private BoxCollider2D hurtbox;
    [SerializeField] private Signal damageTakenSignal;
    [SerializeField] private Signal deathSignal;

    private PlayerControls playerControls;
    private Camera mainCamera;
    private Vector2 movement;
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private TrailRenderer trailRenderer;
    private AudioSource dashSFX;
    private Knockback knockback;
    private Flash flash;
    private bool isFacingLeft = false;
    private bool isDashing = false;
    private System.Action<UnityEngine.InputSystem.InputAction.CallbackContext> inputCallback;

    private readonly float dashDuration = 0.12f;
    private readonly float dashCooldown = 0.35f;

    private void Awake() {
        if (!Instance) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(GetGameObject());
        }

        playerControls = new PlayerControls();
        mainCamera = Camera.main;
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        dashSFX = GetComponent<AudioSource>();
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
        inputCallback = ctx => Dash(); 
    }

    private GameObject GetGameObject()
    {
        return gameObject;
    }

    private void OnEnable() {
        playerControls.Enable();
        playerControls.Movement.Dash.performed += inputCallback; 
        SceneManager.sceneLoaded += OnSceneLoaded;  // *** NEW ***
    }

    private void OnDisable() {
        playerControls?.Disable();
        playerControls.Movement.Dash.performed -= inputCallback; 
        SceneManager.sceneLoaded -= OnSceneLoaded;  // *** NEW ***
    }

    private void Start() {
        playerControls.Movement.Dash.performed += _ => Dash();
    }

    private void Update() {
        PlayerInput();
    }

    private void FixedUpdate() {
        // Dis-allow movement if player is getting knocked back
        if (knockback.GettingKnockedBack) return;    
        // Otherwise, trigger movement
        else {
            AdjustPlayerFacingDirection();
            Move();
        }            
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        if (movement != Vector2.zero)
        {
            myAnimator.SetFloat("moveX", movement.x);
            myAnimator.SetFloat("moveY", movement.y);
            myAnimator.SetBool("isMoving", true);
        }
       else {
            myAnimator.SetBool("isMoving", false);
        } 
    }

    private void Move() {
        myRigidBody.MovePosition(myRigidBody.position + movement * (moveSpeed.runtimeValue * Time.fixedDeltaTime));
    }

    private void Dash() {
        if (!isDashing) {
            StartCoroutine(DashRoutine());
        }
    }

    private void AdjustPlayerFacingDirection()
    {
        if (!mainCamera) return;

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = mainCamera.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x) {
            mySpriteRenderer.flipX = true;
            FacingLeft = true;
        } else {
            mySpriteRenderer.flipX = false;
            FacingLeft = false;
        }
    }

    public void TakeDamage(float damageReceived, float knockbackThrust, float knockbackDuration)
    {
        HP.runtimeValue -= damageReceived;
        if (damageTakenSignal) damageTakenSignal.Raise();
        if (Enemy.Instance) knockback.TriggerKnockback(Enemy.Instance.transform, knockbackThrust, knockbackDuration);
        StartCoroutine(flash.WhiteFlashRoutine());
        CheckForDeath();
    }

    private void CheckForDeath() {
        if (HP.runtimeValue <= 0) {
            StartCoroutine(flash.RedFlashRoutine());
            if (deathSignal) deathSignal.Raise();
            //gameObject.SetActive(false);
        }
    }

    private IEnumerator DashRoutine()
    {
        // Initiate Dash
        isDashing = true;
        hurtbox.enabled = false;
        if (dashSFX) dashSFX.Play();
        moveSpeed.runtimeValue = dashSpeed.value;
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        
        // End Dash
        hurtbox.enabled = true;
        moveSpeed.runtimeValue = moveSpeed.value;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }

    // *** NEW ***
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject spawnPoint = GameObject.FindWithTag("Respawn");

        if (player && spawnPoint) {
            player.transform.position = spawnPoint.transform.position;
        }
    }
}