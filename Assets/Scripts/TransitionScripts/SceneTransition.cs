using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;      
    [SerializeField] private GameObject fadeToBlack;
    [SerializeField] private GameObject fadeFromBlack;
    
    private void Awake() {
        if (fadeFromBlack != null) {
            GameObject panel = Instantiate(fadeFromBlack, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1);  // 1 second of delay before object is destroyed
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>() && !other.isTrigger) {
            StartCoroutine(FadeToBlackRoutine());
        }
    }

    public IEnumerator FadeToBlackRoutine() {
        // Fade to black effect
        if (fadeToBlack) {
            GameObject fadePanel = Instantiate(fadeToBlack, Vector3.zero, Quaternion.identity);
            yield return StartCoroutine(WaitForTargetAnimation(fadePanel.GetComponent<Animator>(), "toBlack"));
        }

        

        // Load next scene
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncOp.isDone) yield return null;
    }

    private IEnumerator WaitForTargetAnimation(Animator anim, string stateName)
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))  yield return null;  // wait for previous animation to finish
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) yield return null;  // wait for target animation to finish
    }
}
