using UnityEngine;

public class Weapon : Singleton<Weapon>
{
    [SerializeField] private MonoBehaviour rangedWeapon;
    [SerializeField] private float fireRate = 0.3f;

    private PlayerControls playerControls;
    private bool attackButtonDown = false;
    private float fireTimer = 0f;

    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Start()
    {
        playerControls.Combat.Ranged.started += _ => StartRangedAttack();  // on left-click press
        playerControls.Combat.Ranged.canceled += _ => StopRangedAttack();  // on left-click release
    }

    private void Update() {
        fireTimer += Time.deltaTime;
        if (attackButtonDown) {
            Shoot();
        }
    }

    // *---*  Ranged Attacks  *---------------------------------------------------------*
    private void StartRangedAttack() {
        if (rangedWeapon) attackButtonDown = true;
    }

    private void StopRangedAttack() {
        if (rangedWeapon) attackButtonDown = false;
    }

    private void Shoot() {
        if (fireTimer >= fireRate) {
            (rangedWeapon as IWeapon).Attack();
            fireTimer = 0f;
        }
    }
}
