using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float followSpeed = 100f;

    public void SetTarget(Transform newTarget) {
        target = newTarget;
    }

    private void Update()
    {
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
    }
}