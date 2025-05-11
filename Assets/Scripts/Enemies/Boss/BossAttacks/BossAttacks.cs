using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletSpawnPoints {
    public string spawnName;
    public Transform spawnTransform;
}

public class BossAttacks : MonoBehaviour
{    
    [SerializeField] private MonoBehaviour singleShotAttack;
    [SerializeField] private MonoBehaviour shotgunShotAttack;
    [SerializeField] private MonoBehaviour singleBlastAttack;
    [SerializeField] private MonoBehaviour multiBlastAttack;
    [SerializeField] private List<BulletSpawnPoints> bulletSpawnPoints;

    private Dictionary<string, Transform> spawnPointDict;
    private Vector2 distanceToPlayer;
    private string currentDirection;
    
    private void Awake() {
        // Set up dictionary for projectile spawn positions
        spawnPointDict = new Dictionary<string, Transform>();
        foreach (var point in bulletSpawnPoints) {
            if (!spawnPointDict.ContainsKey(point.spawnName)) {
                spawnPointDict.Add(point.spawnName, point.spawnTransform);
            }
        }
    }

    private void Update() {
        distanceToPlayer = (Player.Instance.transform.position - transform.position).normalized;
        currentDirection = GetDominantDirection(distanceToPlayer);
    }

    // Single-Shot
    public void SingleShootDown() {   
        if (currentDirection == "Down") (singleShotAttack as IBossAttack).Attack(GetSpawnPoint("Down"));
    }
    public void SingleShootUp() {   
        if (currentDirection == "Up") (singleShotAttack as IBossAttack).Attack(GetSpawnPoint("Up"));
    }
    public void SingleShootLeft() {   
        if (currentDirection == "Left") (singleShotAttack as IBossAttack).Attack(GetSpawnPoint("Left"));
    }
    public void SingleShootRight() {   
        if (currentDirection == "Right") (singleShotAttack as IBossAttack).Attack(GetSpawnPoint("Right"));
    }

    // Shotgun Shot
    public void ShotgunShootDown() {   
        if (currentDirection == "Down") (shotgunShotAttack as IBossAttack).Attack(GetSpawnPoint("Down"));
    }
    public void ShotgunShootUp() {   
        if (currentDirection == "Up") (shotgunShotAttack as IBossAttack).Attack(GetSpawnPoint("Up"));
    }
    public void ShotgunShootLeft() {   
        if (currentDirection == "Left") (shotgunShotAttack as IBossAttack).Attack(GetSpawnPoint("Left"));
    }
    public void ShotgunShootRight() {   
        if (currentDirection == "Right") (shotgunShotAttack as IBossAttack).Attack(GetSpawnPoint("Right"));
    }

    // Single AoE Burst
    public void SingleBurst() {
        (singleBlastAttack as IBossAttack).Attack(Boss.Instance.transform);
    }
    public void MultiBurst() {
        (multiBlastAttack as IBossAttack).Attack(Boss.Instance.transform);
    }

    // returns bullet spawn point of respective direction (relative to object sprite)
    private Transform GetSpawnPoint(string name) {
        return spawnPointDict.TryGetValue(name, out var point) ? point : null;
    }

    // returns the "general direction" of where the player is
    private string GetDominantDirection(Vector2 dir)    
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) {
            return dir.x > 0 ? "Right" : "Left";
        }
        else {
            return dir.y > 0 ? "Up" : "Down";
        }
    }
}
