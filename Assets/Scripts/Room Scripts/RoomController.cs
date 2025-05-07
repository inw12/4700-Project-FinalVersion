using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject smallChestSpawn;
    [SerializeField] private GameObject largeChestSpawn;
    [SerializeField] private Signal backgroundMusicSignal;
    [SerializeField] private Signal battleMusicSignal;

    private GameObject enemyCount;
    private GameObject lockedPath;
    private bool rewardSpawned = false;

    private void Awake() {
        enemyCount = gameObject.transform.GetChild(0).gameObject;
        lockedPath = gameObject.transform.GetChild(1).gameObject;

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

        // Music Transition
        if (backgroundMusicSignal) backgroundMusicSignal.Raise();

        // 50% chance for a chest to spawn
        if (Random.value < 0.5f) {
            if (!rewardSpawned) {
                Vector3 spawnOffset = new(0f, 1.5f, 0f);
                // 75% chance for a small chest
                if (Random.value < 0.75f) {
                    if (smallChestSpawn) Instantiate(smallChestSpawn, transform.position + spawnOffset, Quaternion.identity);
                }
                // 25% chance for a large chest
                else {
                    if (largeChestSpawn) Instantiate(largeChestSpawn, transform.position + spawnOffset, Quaternion.identity);
                }
                rewardSpawned = true;
            }
        }
    }

}
