using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeChest : MonoBehaviour, IChest
{
    [SerializeField] private List<GameObject> weaponPool;
    [SerializeField] private List<GameObject> rewardEffects;
    [SerializeField] private GameObject destroyEffect;

    //private Weapon playerWeapon;
    private Animator anim;
    private AudioSource audioSrc;
    private bool itemPickupStarted = false;
    private bool despawnStarted = false;

    private void Awake() {
        //playerWeapon = Weapon.Instance;
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
    }

    public void PlayAudio() {
        audioSrc.Play();
    }

    public void OpenChest() {
        anim.SetBool("isOpen", true);
        int index = Random.Range(0, weaponPool.Count); // random index for weapon pool
    
        // Reward new weapon at random
        if (Weapon.Instance && weaponPool.Count > 0) {
            GameObject randomWeapon = weaponPool[index];
            Weapon.Instance.SwapCurrentWeapon(randomWeapon);
        }

        // Play reward animation
        if (!itemPickupStarted) {
            itemPickupStarted = true;
            StartCoroutine(WaitForTargetAnimation(anim, "Idle (Opened)"));  
            Vector3 spawnOffset = new(0f, 0.85f, 0f);
            Instantiate(rewardEffects[index], transform.position + spawnOffset, Quaternion.identity);
        }

        // Despawn the chest
        if (!despawnStarted) {
            StartCoroutine(DestroyChestRoutine());
            despawnStarted = true;
        }
    }

    private IEnumerator DestroyChestRoutine() {
        yield return new WaitForSeconds(1.25f);
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    private IEnumerator WaitForTargetAnimation(Animator anim, string stateName)
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))  yield return null;  // wait for previous animation to finish
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) yield return null;  // wait for target animation to finish
    }
}