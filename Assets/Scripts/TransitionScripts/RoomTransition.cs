using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    [SerializeField] private MinMaxVectorValue cameraBounds;
    [SerializeField] private Vector2 cameraChange;    // how much to move camera
    [SerializeField] private Vector3 playerChange;    // how much to move player
    [SerializeField] private Signal transitionSignal;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() && !other.isTrigger)  {
            cameraBounds.runtimeMax += cameraChange;    // update cam's max bounds
            cameraBounds.runtimeMin += cameraChange;    // update cam's min bounds
            other.transform.position += playerChange;   // move the character a bit

            if (transitionSignal) transitionSignal.Raise();
        }
    }
}
