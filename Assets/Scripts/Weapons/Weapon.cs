using System.Collections;
using UnityEngine;

public class Weapon : Singleton<Weapon>
{
    [SerializeField] private GameObject defaultWeapon;
    [SerializeField] private Signal weaponSwapSignal;

    private PlayerControls playerControls;
    private MonoBehaviour currentWeapon;    // script of the current weapon
    private GameObject weaponInstance;      // object of the current weapon
    private AudioSource audioSrc;
    private bool attackButtonDown = false;

    private readonly Vector3 weaponOffset = new(0.5f, 0f, 0f);  // position where weapon should be relative to player 

    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();
        audioSrc = GetComponent<AudioSource>();

        // Instantiate the default weapon
        weaponInstance = Instantiate(defaultWeapon, transform.position + weaponOffset, transform.rotation, transform);  
        currentWeapon = weaponInstance.GetComponent<MonoBehaviour>();
        // IMPORTANT!!! DON'T CALL METHODS ON INSTANTIATED OBJECTS; THEY ARE NOT THE OBJECT, THEY ARE INSTANCES OF THEM!!!
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

    public void SwapCurrentWeapon (GameObject newWeapon) {
        if (currentWeapon) {
            // Drop current weapon on the floor
            (currentWeapon as IWeapon).Drop();
            Destroy(Instance.currentWeapon.gameObject);
            currentWeapon = null;

            // Equip new weapon + Update 'currentWeapon' instance
            weaponInstance.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation); // get position of weapon
            weaponInstance = Instantiate(newWeapon, position, rotation, transform);                         // instantiate new weapon @ that position
            audioSrc.Play();
            currentWeapon = weaponInstance.GetComponent<MonoBehaviour>();                                   // update 'currentWeapon'
            StartCoroutine(SignalDelay());
        }        
    }

    // Wait for objects to update before signaling to the UI
    private IEnumerator SignalDelay() {
        yield return new WaitForSeconds(0.1f);
        weaponSwapSignal.Raise();
    }
}
