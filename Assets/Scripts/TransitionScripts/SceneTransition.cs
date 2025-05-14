using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;      
    [SerializeField] private GameObject fadeToBlack;

    private GameObject fadePanel;

    private void Awake() {
        if (fadePanel) Destroy(fadePanel);  // *** NEW ***
    }

    // When entering the trigger zone...
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>() && !other.isTrigger) {
            if (fadeToBlack) fadePanel = Instantiate(fadeToBlack, Vector3.zero, Quaternion.identity);
            StartCoroutine(SceneTransitionRoutine(fadePanel.GetComponent<Animator>(), "toBlack"));
        }
    }

    private IEnumerator SceneTransitionRoutine(Animator anim, string stateName) {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))  yield return null;  // wait for previous animation to finish
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) yield return null;  // wait for target animation to finish
        SceneManager.LoadScene(sceneToLoad);
    }
}
