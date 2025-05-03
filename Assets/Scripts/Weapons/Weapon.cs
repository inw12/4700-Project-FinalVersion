using UnityEngine;

public class Weapon : Singleton<Weapon>
{
    [SerializeField] private MonoBehaviour rangedWeapon;
    [SerializeField] private float fireRate = 0.3f;
    //[SerializeField] private MonoBehaviour meleeWeapon;

    private PlayerControls playerControls;
    private bool attackButtonDown = false;
    private float fireTimer = 0f;

    //private bool isAttackingRanged = false;
    //private bool isAttackingMelee = false;
    //private bool meleeAttackTriggered = false;

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

        //playerControls.Combat.Melee.started += _ => StartMeleeAttack();   // on right-click press
        //playerControls.Combat.Melee.canceled += _ => StopMeleeAttack();   // on right-click release
    }

    private void Update() {
        if (attackButtonDown) {   
            Shoot();    
        }
    }

    // *---*  Ranged Attacks  *---------------------------------------------------------*
    private void StartRangedAttack() {
        if (rangedWeapon) {
            //rangedWeapon.gameObject.SetActive(true);

            attackButtonDown = true;
            //isAttackingRanged = true; 
            fireTimer = fireRate;
        }
    }

    private void StopRangedAttack() {
        if (rangedWeapon) {
            //rangedWeapon.gameObject.SetActive(false);

            attackButtonDown = false;
            //isAttackingRanged = false;
            fireTimer = 0f;
        }
    }

    private void Shoot() {
        fireTimer += Time.deltaTime;

        if (attackButtonDown && (fireTimer >= fireRate || fireTimer == 0f)) {
            //isAttackingRanged = true;
            (rangedWeapon as IWeapon).Attack();
            fireTimer = 0f;
        }
    }


    // *---*  MELEE Attacks  *---------------------------------------------------------*
    /*
    private void StartMeleeAttack() {
        if (meleeWeapon && !meleeAttackTriggered) {
            meleeAttackTriggered = true; 
            isAttackingMelee = true;
            (meleeWeapon as IWeapon).Attack(); 
        }
    }

    private void StopMeleeAttack() {
        if (meleeWeapon) {
            isAttackingMelee = false;
            attackButtonDown = false; 
            meleeAttackTriggered = false;
        }
    }
    */
}
