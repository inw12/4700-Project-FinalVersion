using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneToStart = "Level1";
    [SerializeField] private GameObject fadeToBlack;

    private GameObject fadePanel;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Upon entering this scene...
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        StartCoroutine(SceneEnterRoutine());

        if (fadePanel) Destroy(fadePanel);  // *** NEW ***
    }
    
    public void StartGame() {
        if (Player.Instance) Player.Instance.gameObject.SetActive(true);
        if (fadeToBlack) fadePanel = Instantiate(fadeToBlack, Vector3.zero, Quaternion.identity);
        StartCoroutine(SceneTransitionRoutine(fadePanel.GetComponent<Animator>(), "toBlack"));
    }

    public void QuitToDesktop() {
        Application.Quit();
    }    

    private IEnumerator SceneEnterRoutine() {
        while(!Player.Instance) yield return null;      // wait for player to instantiate (if haven't already)
        yield return new WaitForSeconds(0.1f);          // wait a little extra for other things to load
        Player.Instance.gameObject.SetActive(false);    // turn off player so they cannot do stuff in the main menu
    }
    
    // fades to black before running the next scene
    private IEnumerator SceneTransitionRoutine(Animator anim, string stateName)
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))  yield return null;  // wait for previous animation to finish
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) yield return null;  // wait for target animation to finish
        SceneManager.LoadScene(sceneToStart);
    }
}
