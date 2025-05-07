using System.Collections;
using TMPro;
using UnityEngine;

public class WeaponShopItem : MonoBehaviour, IShopItem
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private InventoryItem coinInventoryItem;
    [SerializeField] private TextMeshProUGUI countCointText;
    [SerializeField] private GameObject destroyEffect;

    private AudioSource[] sfx;
    private AudioSource purchaseSound;
    private AudioSource errorSound;
    private bool despawnStarted = false;

    private readonly int itemCost = 5;  // $$$ ITEM PRICE $$$
    private readonly Color defaultTextColor = Color.white;

    private void Awake() {
        sfx = GetComponents<AudioSource>();
        purchaseSound = sfx[0];
        errorSound = sfx[1];
    }

    public void Purchase() {
        // Effect when not enough coins
        if (coinInventoryItem.runtimeAmountHeld < itemCost) {
            errorSound.Play();
            StartCoroutine(TextFlashRoutine(Color.red));
            return;
        }
        // Purchase effect
        else
        {
            purchaseSound.Play();

            // *--*  Reduce player's coin count  *--*
            coinInventoryItem.runtimeAmountHeld -= itemCost;
            if (coinInventoryItem.runtimeAmountHeld < 0) {
                coinInventoryItem.ResetRuntimeAmount();
            }

            StartCoroutine(TextFlashRoutine(Color.green));

            // *--*  Purchase Effect  *--*
            Weapon.Instance.SwapCurrentWeapon(weaponPrefab);

            // *---*  Destroy Effect  *---*
            if (!despawnStarted) {
                StartCoroutine(DestroyObjectRoutine());
                despawnStarted = true;
            }
        }
    }

    private IEnumerator DestroyObjectRoutine() {
        Vector3 offset = new(0f, 0.6f, 0f);
        GameObject poof = Instantiate(destroyEffect, transform.position + offset, Quaternion.identity);
        poof.transform.localScale = destroyEffect.transform.localScale * 1.3f; 

        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    private IEnumerator TextFlashRoutine(Color color) {
        countCointText.color = color;
        yield return new WaitForSeconds(0.1f);
        countCointText.color = defaultTextColor;
    }
}
