using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class EnemySpawnPoints {
    public string spawnName;
    public Transform spawnTransform;
}

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private GameObject spawnEnemyPrefab;
    [SerializeField] private List<EnemySpawnPoints> enemySpawnPoints;
    private Dictionary<string, Transform> spawnPointDict;

    private void Awake() {
        spawnPointDict = new Dictionary<string, Transform>();
        foreach (var point in enemySpawnPoints) {
            if (!spawnPointDict.ContainsKey(point.spawnName)) {
                spawnPointDict.Add(point.spawnName, point.spawnTransform);
            }
        }
    }

    public void SpawnTopLeft() {
        Instantiate(spawnEnemyPrefab, GetSpawnPoint("TopLeft").position, Quaternion.identity);
    }
    public void SpawnTopRight() {
        Instantiate(spawnEnemyPrefab, GetSpawnPoint("TopRight").position, Quaternion.identity);
    }
    public void SpawnBottomLeft() {
        Instantiate(spawnEnemyPrefab, GetSpawnPoint("BottomLeft").position, Quaternion.identity);
    }
    public void SpawnBottomRight() {
        Instantiate(spawnEnemyPrefab, GetSpawnPoint("BottomRight").position, Quaternion.identity);
    }

    private Transform GetSpawnPoint(string name) {
        return spawnPointDict.TryGetValue(name, out var point) ? point : null;
    }
}
