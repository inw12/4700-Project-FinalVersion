using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingArea : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip battleMusic;
    [SerializeField] private GameObject fadeFromBlack;

    private void Awake() {
        GameManager.Instance.SpawnPlayer();         // player
        MenuManager.Instance.InitializePlayerHUD(); // player HUD
        MusicManager.Instance.LoadSongs(backgroundMusic, battleMusic);

        if (fadeFromBlack) {
            GameObject panel = Instantiate(fadeFromBlack, Vector3.zero, Quaternion.identity);
            Destroy(panel, 1);
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        MusicManager.Instance.FadeToSongA();
    }

    public void BattleMusic() {
        MusicManager.Instance.FadeToSongB();
    }

    public void BackgroundMusic() {
        MusicManager.Instance.FadeToSongA();
    }
}
