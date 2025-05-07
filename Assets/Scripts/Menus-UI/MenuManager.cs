using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private List<InventoryItem> itemsToReset;
    [SerializeField] private List<FloatValue> valuesToReset;

    private PlayerControls playerControls;
    private bool isPaused;
    private readonly string beginningScene = "Level1";
    private readonly string menuScene = "MainMenu";

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Start() {
        isPaused = false;
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        playerControls.Menus.Pause.started += _ => PauseMenu();        
    }

    private void PauseMenu() {
        isPaused = !isPaused;

        if (isPaused) {
            Time.timeScale = 0f;
            playerHUD.SetActive(false);
            pauseMenu.SetActive(true);

            Player.Instance.gameObject.SetActive(false);
        }
        else {
            Time.timeScale = 1f;
            playerHUD.SetActive(true);
            pauseMenu.SetActive(false);

            Player.Instance.gameObject.SetActive(true);
        }
    }

    public void GameOverMenu() {
        isPaused = !isPaused;

        if (isPaused) {
            Time.timeScale = 0f;
            playerHUD.SetActive(false);
            gameOverMenu.SetActive(true);
        }
    }

    public void Resume() {
        isPaused = !isPaused;
        Time.timeScale = 1f;

        playerHUD.SetActive(true);
        pauseMenu.SetActive(false);

        Player.Instance.gameObject.SetActive(true);
        playerControls.Enable();
    }

    public void Retry() {
        Time.timeScale = 1f;
        foreach (InventoryItem item in itemsToReset) item.ResetRuntimeAmount();
        foreach (FloatValue value in valuesToReset) value.ResetRuntimeValue();
        SceneManager.LoadScene(beginningScene);
    }

    public void QuitToMain() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }
}
