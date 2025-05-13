using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject playerAnimator;
    [SerializeField] private GameObject lockedPath;

    private PlayableDirector director;
    private bool cutsceneRunning = true;

    private void Awake() {
        director = GetComponent<PlayableDirector>();
        if (lockedPath) lockedPath.SetActive(false);
    }

    private IEnumerator Start() {
        // Wait for player instance to spawn
        yield return new WaitUntil(() => Player.Instance != null);
        yield return null;

        // Deactivate player
        DisablePlayer();
    }

    private void Update() {
        // When the cutscene is finished
        if (director.state != PlayState.Playing && cutsceneRunning) {
            cutsceneRunning = false;            
            Destroy(playerAnimator);
            EnablePlayer();
            lockedPath.SetActive(true);
        }
    }

    public bool IsPlaying() {
        return cutsceneRunning;
    }

    public void EndCutscene() {
        director.Stop();
        cutsceneRunning = false;
    }

    private void DisablePlayer() {
        Player.Instance.gameObject.SetActive(false);    // disable player object
        Player.Instance.DisableControls();              // turn off controls
        MenuManager.Instance.ToggleHUD();               // turn off HUD
    }
    private void EnablePlayer() {
        Player.Instance.gameObject.SetActive(true);
        Player.Instance.EnableControls();
        MenuManager.Instance.ToggleHUD();
    }
}
