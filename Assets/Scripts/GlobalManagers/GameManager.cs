using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<ScriptableObject> itemsToReset;

    private GameObject playerInstance;

    protected override void Awake() {
        base.Awake();
        ResetGameObjects();
    }

    public void SpawnPlayer() {
        if (!playerInstance) {
            playerInstance = Instantiate(playerPrefab);
            DontDestroyOnLoad(playerInstance);
        }
    }

    public void ResetGameObjects() {
        // Reset scriptable objects
        foreach (ScriptableObject item in itemsToReset) {
            (item as IResettable)?.Reset();
        }
        // Update player weapon to basic pistol
        if (Weapon.Instance) Weapon.Instance.UpdateCurrentWeapon();
        if (MusicManager.Instance) MusicManager.Instance.ResetAudio();
    }
}