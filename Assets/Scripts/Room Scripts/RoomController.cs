using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject enemyCount;
    [SerializeField] private GameObject lockedPath;
    [SerializeField] private GameObject roomClearReward;
    [SerializeField] private Signal backgroundMusicSignal;
    [SerializeField] private Signal battleMusicSignal;

    private bool rewardSpawned = false;

    private void Start() {
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

        // MUSIC
        if (enemyCount && enemyCount.transform.childCount != 0) {
            if (battleMusicSignal) battleMusicSignal.Raise();
        }
    }

    // On room clear...
    public void RoomEnd() {
        // Unlock all doors
        if (lockedPath) Destroy(lockedPath);

        // MUSIC
        if (backgroundMusicSignal) backgroundMusicSignal.Raise();

        // Spawn reward in center of room
        if (!rewardSpawned) {
            Vector3 spawnOffset = new(0f, 1.5f, 0f);
            if (roomClearReward) Instantiate(roomClearReward, transform.position + spawnOffset, Quaternion.identity);
            rewardSpawned = true;
        }
    }

}
