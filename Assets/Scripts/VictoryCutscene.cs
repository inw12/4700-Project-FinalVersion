using System.Collections;
using UnityEngine;

public class VictoryCutscene : MonoBehaviour
{
    [SerializeField] private FinalBoss boss;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private GameObject fadeToBlack;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private GameObject victoryMenu;
    
    private Camera mainCamera;
    private Animator anim;
    private bool cutsceneTriggered;

    private void Awake() {
        mainCamera = Camera.main;
        anim = boss.GetComponent<Animator>();
        cutsceneTriggered = false;
    }

    // * Triggered by a SIGNAL from the editor
    public void TriggerVictoryCutscene() {

        if (!cutsceneTriggered) {
            cutsceneTriggered = true;

            // Destroy any active projectiles in the scene
            //Projectile[] projectiles1 = FindObjectsByType<Projectile>(FindObjectsSortMode.None);
            //foreach (Projectile proj in projectiles1) Destroy(proj.gameObject);
            BossProjectile[] projectiles2 = FindObjectsByType<BossProjectile>(FindObjectsSortMode.None);
            foreach (BossProjectile proj in projectiles2) Destroy(proj.gameObject);

            // Focus the camera on the boss
            mainCamera.GetComponent<CameraMovement>().target = boss.transform;

            boss.StopMoving();
            Player.Instance.gameObject.SetActive(false);

            // Trigger boss death animation
            anim.Play("death");

            // Loop explosions until victory menu is active
            StartCoroutine(SpawnExplosions());

            // Transition to victory menu
            StartCoroutine(CutsceneRoutine());
        }
    }

    private IEnumerator CutsceneRoutine()
    {
        yield return new WaitForSecondsRealtime(4f);
        yield return StartCoroutine(FadeToBlack());
        playerHUD.SetActive(false);
        victoryMenu.SetActive(true);
        // desitryo th ethingy 
    }

    private IEnumerator SpawnExplosions() {
        while (!victoryMenu.activeInHierarchy) {
            float offsetX = Random.Range(-1.5f, 1.5f);
            float offsetY = Random.Range(-1.25f, 1.5f);
            Vector3 spawnPos = new(boss.transform.position.x + offsetX, boss.transform.position.y + offsetY, boss.transform.position.z);
            Instantiate(explosionVFX, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(.33f);
        }
    }

    private IEnumerator FadeToBlack() {
        // Fade to black effect
        if (fadeToBlack) {
            GameObject fadePanel = Instantiate(fadeToBlack, Vector3.zero, Quaternion.identity);
            yield return StartCoroutine(WaitForTargetAnimation(fadePanel.GetComponent<Animator>(), "toBlack"));
            Destroy(fadePanel);
        }
    }

    private IEnumerator WaitForTargetAnimation(Animator anim, string stateName)
    {
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) yield return null;  // wait for target animation to finish
    }
}