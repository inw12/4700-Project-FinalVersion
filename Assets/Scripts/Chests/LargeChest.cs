using System.Collections;
using UnityEngine;

public class LargeChest : MonoBehaviour, IChest
{
    [SerializeField] private InventoryItem item;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private AudioSource openChestAudio;
    [SerializeField] private GameObject itemPickupEffect;
    [SerializeField] private GameObject destroyEffect;

    private Animator anim;
    private bool itemPickupStarted = false;
    private bool despawnStarted = false;
    private readonly int rewardAmount = 3;  // how many of the item the player will receive

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void PlayAudio() {
        openChestAudio.Play();
    }

    public void OpenChest() {
        anim.SetBool("isOpen", true);

        // Add item to player inventory + play item pickup effect
        if (playerInventory && !itemPickupStarted) {
            StartCoroutine(WaitForTargetAnimation(anim, "Idle (Opened)"));  
            StartCoroutine(MultipleRewardsEffectRoutine());
            itemPickupStarted = true;
        }

        // Despawn the chest
        if (!despawnStarted) {
            StartCoroutine(DestroyChestRoutine());
            despawnStarted = true;
        }
    }

    private IEnumerator MultipleRewardsEffectRoutine() {
        Vector3 spawnOffset = new(0f, 1f, 0f);
        for (int i = 0; i < rewardAmount; i++) {
            item.runtimeAmountHeld++;
            Instantiate(itemPickupEffect, transform.position + spawnOffset, Quaternion.identity);
            yield return new WaitForSeconds(0.12f);
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
