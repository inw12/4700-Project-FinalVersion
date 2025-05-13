using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip battleMusic;
    [SerializeField] private GameObject fadeFromBlack;

    private void Awake() {
        MusicManager.Instance.LoadSongs(introMusic, battleMusic);

        // Fade from black transition
        if (fadeFromBlack) {
            GameObject panel = Instantiate(fadeFromBlack, Vector3.zero, Quaternion.identity);
            Destroy(panel, 1);
        }
        
        MusicManager.Instance.FadeToSongA();
    }

    public void BattleMusic() {
        MusicManager.Instance.FadeToSongB(0.1f);
    }
}
