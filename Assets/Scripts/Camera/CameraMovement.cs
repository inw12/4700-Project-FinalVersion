using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing;
    
    public Vector3Value initialPosition;
    public MinMaxVectorValue camBounds;
    
    private void Start() {
        transform.position = initialPosition.runtimeValue;
        camBounds.ResetRuntimeValues();
    }

    private void LateUpdate()
    {
        if (target != null) {
            if (transform.position != target.position)  // "if transform position IS NOT the target position..."
            {   
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

    public Vector2 GetMaxBounds() {
        return camBounds.runtimeMax;
    }

    public Vector2 GetMinBounds() {
        return camBounds.runtimeMin;
    }
}
