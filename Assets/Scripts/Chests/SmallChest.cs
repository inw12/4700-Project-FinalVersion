using System.Collections;
using UnityEngine;

public class SmallChest : MonoBehaviour, IChest
{
    [SerializeField] private InventoryItem item;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private AudioSource openChestAudio;
    [SerializeField] private GameObject itemPickupEffect;
    [SerializeField] private GameObject destroyEffect;

    private Animator anim;
    private bool itemPickupStarted = false;
    private bool despawnStarted = false;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void PlayAudio() {
        openChestAudio.Play();
    }

    public void OpenChest() {
        anim.SetBool("isOpen", true);

        // Add item to inventory
        if (playerInventory) {
            item.runtimeAmountHeld++;
        }

        // Play reward animation
        if (!itemPickupStarted) {
            StartCoroutine(WaitForTargetAnimation(anim, "Idle (Opened)"));
            Vector3 spawnOffset = new(0f, 1f, 0f);
            Instantiate(itemPickupEffect, transform.position + spawnOffset, Quaternion.identity);
            itemPickupStarted = true;
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
