using System.Collections;
using UnityEngine;

public class Weapon : Singleton<Weapon>
{
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private Signal weaponSwapSignal;

    private GameObject weaponInstance;      // Reference to the spawned weapon from 'playerWeapon'
    private MonoBehaviour currentWeapon;    // Reference to the Monobehavior in 'weaponInstance'

    private PlayerControls playerControls;
    private AudioSource audioSrc;
    private bool attackButtonDown = false;

    private readonly Vector3 weaponOffset = new(0.5f, 0f, 0f);  // position where weapon should be relative to player 

    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();
        audioSrc = GetComponent<AudioSource>();

        weaponInstance = Instantiate(playerWeapon.currentWeapon, transform.position + weaponOffset, transform.rotation, transform);  
        currentWeapon = weaponInstance.GetComponent<MonoBehaviour>();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Start() {
        playerControls.Combat.Ranged.started += _ => StartRangedAttack();  // on left-click press
        playerControls.Combat.Ranged.canceled += _ => StopRangedAttack();  // on left-click release
    }

    private void Update() {
        if (attackButtonDown) {
            (currentWeapon as IWeapon).Attack();
        }
    }

    private void StartRangedAttack() {
        if (currentWeapon) attackButtonDown = true;
    }

    private void StopRangedAttack() {
        if (currentWeapon) attackButtonDown = false;
    }

    // Used for updating weapon instances when weapons are forcibly changed by the system (i.e. restarts)
    public void UpdateCurrentWeapon() {
        if (currentWeapon) {
            Destroy(Instance.currentWeapon.gameObject);
            currentWeapon = null;

            weaponInstance.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
            weaponInstance = Instantiate(playerWeapon.currentWeapon, position, rotation, transform);
            currentWeapon = weaponInstance.GetComponent<MonoBehaviour>();
            StartCoroutine(SignalDelay());
        }
    }

    // Drops current weapon on the ground + Swaps player's weapon for a new one
    public void SwapCurrentWeapon (GameObject newWeapon) {
        if (currentWeapon) {
            // Drop current weapon on the floor
            (currentWeapon as IWeapon).Drop();
            Destroy(Instance.currentWeapon.gameObject);
            currentWeapon = null;

            // Equip new weapon + Update 'currentWeapon' instance
            weaponInstance.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation); // get position + rotation of weapon
            playerWeapon.currentWeapon = newWeapon;
            weaponInstance = Instantiate(newWeapon, position, rotation, transform);                         // instantiate new weapon @ that position
            audioSrc.Play();
            currentWeapon = weaponInstance.GetComponent<MonoBehaviour>();                                   // update 'currentWeapon'
            StartCoroutine(SignalDelay());
        }        
    }

    // Raise signal to UI to update the HUD; delayed for weapon instance to load
    private IEnumerator SignalDelay() {
        yield return new WaitForSeconds(0.1f);
        weaponSwapSignal.Raise();
    }

    public void EnableWeapons() {
        playerControls.Enable();
    }
    public void DisableWeapons() {
        playerControls.Disable();
    }
}
