using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Signal roomClearSignal;

    private bool signalRaise = false;

    void Update() {
        if (transform.childCount == 0 && !signalRaise) {
            roomClearSignal.Raise();
            signalRaise = true;
        }
    }
}
