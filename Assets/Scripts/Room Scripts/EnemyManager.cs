using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Signal roomClearSignal;

    private bool signalRaise = false;

    // If the room initially has NO enemies
    private void Awake() {
        if (transform.childCount == 0 && !signalRaise) {
            signalRaise = true;
        }
    }

    // Effect when the room is cleared of enemies
    private void Update() {
        if (transform.childCount == 0 && !signalRaise) {
            roomClearSignal.Raise();
            signalRaise = true;
        }
    }
}
