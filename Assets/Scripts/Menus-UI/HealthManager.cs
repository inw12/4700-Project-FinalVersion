using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Image[] healthBars;
    [SerializeField] private Sprite healthBar;
    [SerializeField] private Sprite emptyBar;
    [SerializeField] private FloatValue currentHealth;

    private void Start() {
        InitialHealth();
    }

    private void Update() {
        UpdateHealth();
    }

    private void InitialHealth()
    {
        for (int i = 0; i < currentHealth.value; i++) {
            healthBars[i].gameObject.SetActive(true);
            healthBars[i].sprite = healthBar;
        }
    }

    public void UpdateHealth()
    {
        for (int i = 0; i < currentHealth.value; i++) {
            if (i <= currentHealth.runtimeValue - 1) {
                healthBars[i].sprite = healthBar;
            }
            else {
                healthBars[i].sprite = emptyBar;
            }
        }
    }
}
