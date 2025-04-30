using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Signal roomClearSignal;

    void Update()
    {
        if (transform.childCount == 0) {
            roomClearSignal.Raise();
        }
    }
}
