using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3Value initialPosition;
    [SerializeField] private MinMaxVectorValue camBounds;
    [SerializeField] private float smoothing;

    private Transform target;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake() {
        if (initialPosition) transform.position = initialPosition.runtimeValue;
        if (camBounds) camBounds.ResetRuntimeValues();
    }

    private void Start() {
        if (target) target = Player.Instance.transform;    
    }

    private void LateUpdate() {
        if (target) {
            // "if transform position IS NOT the target position..."
            if (transform.position != target.position) {   
                // Determine position of the target (the player)
                Vector3 targetPosition = new(target.position.x, target.position.y + 1.5f, transform.position.z);

                // Return coordinates for where the camera should be within its bounds
                targetPosition.x = Mathf.Clamp(targetPosition.x, camBounds.runtimeMin.x, camBounds.runtimeMax.x);     // Clamp(what we want to bound, min bounds, max bounds); returns value BETWEEN min/max bounds
                targetPosition.y = Mathf.Clamp(targetPosition.y, camBounds.runtimeMin.y, camBounds.runtimeMax.y);
                
                // Move the camera 
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);   // Lerp(where we are, where we wanna be, movement interval between those 2 points)
            }
        }
    }

    // *** NEW ***
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Player player = FindFirstObjectByType<Player>();
        target = player.transform;
    }

    public void ChangeTarget(Transform newTarget) {
        target = newTarget;
    }
}
