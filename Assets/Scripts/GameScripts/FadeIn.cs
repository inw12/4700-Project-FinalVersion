using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private GameObject fadeFromBlack;
    
    private void Awake() {
        if (fadeFromBlack) {
            GameObject panel = Instantiate(fadeFromBlack, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1);  // 1 second of delay before object is destroyed
        }
    }
}
