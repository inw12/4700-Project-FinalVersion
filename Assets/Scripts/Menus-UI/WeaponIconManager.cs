using UnityEngine;
using UnityEngine.UI;

public class WeaponIconManager : MonoBehaviour
{
    private Image weaponIcon;

    private void Awake() {
        weaponIcon = GetComponent<Image>();
    }

    private void Start() {
        UpdateIcon();
    }

    public void UpdateIcon() {
        Transform currentWeapon = Weapon.Instance.transform.childCount > 0 ? Weapon.Instance.transform.transform.GetChild(0) : null;
        Sprite weaponSprite = currentWeapon.GetComponent<SpriteRenderer>().sprite;
        AspectRatioFitter aspectRatio = weaponIcon.GetComponent<AspectRatioFitter>();

        if (aspectRatio && weaponSprite) {
            float aspect = weaponSprite.rect.width / weaponSprite.rect.height;
            aspectRatio.aspectRatio = aspect;
        }

        weaponIcon.sprite = weaponSprite;
    }
}