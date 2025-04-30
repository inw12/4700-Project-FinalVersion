using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject lockedPath;
    [SerializeField] private GameObject roomClearReward;

    private Vector3 spawnOffset = new(0f, 1.5f, 0f);
    private bool rewardSpawned = false;

    private void Start()
    {
        // De-activate every entity in the room
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
    }

    // On room enter...
    public void RoomStart() {
        // Activate any existing room entities
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
    }

    // On room clear...
    public void RoomEnd() {
        // 1. Unlock all doors
        lockedPath.SetActive(false);
        // 2. Spawn reward in center of room
        if (!rewardSpawned) {
            if (roomClearReward) Instantiate(roomClearReward, transform.position + spawnOffset, Quaternion.identity);
            rewardSpawned = true;
        }
    }

}
