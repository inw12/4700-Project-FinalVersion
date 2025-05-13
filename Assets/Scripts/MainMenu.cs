using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private readonly string startingScene = "TestingArea";

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Upon entering this scene...
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        StartCoroutine(SceneEnterRoutine());
    }
    
    public void StartGame() {
        if (Player.Instance) Player.Instance.gameObject.SetActive(true);
        SceneManager.LoadScene(startingScene);
    }

    public void QuitToDesktop() {
        Application.Quit();
    }    

    private IEnumerator SceneEnterRoutine() {
        while(!Player.Instance) yield return null;
        yield return new WaitForSeconds(0.1f);
        Player.Instance.gameObject.SetActive(false);
    }
}
