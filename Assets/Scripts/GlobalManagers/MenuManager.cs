using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject playerHUDPrefab;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private GameObject gameOverMenu;

    private bool isPaused;
    private GameObject playerHUD;

    private readonly string level1Scene = "Level1";
    private readonly string mainMenuScene = "MainMenu";

    protected override void Awake() {
        base.Awake();
    }

    public void InitializePlayerHUD() {
        if (!playerHUD) {
            playerHUD = Instantiate(playerHUDPrefab);
            DontDestroyOnLoad(playerHUD);
        } 
    }

    public void ToggleHUD() {
        playerHUD.SetActive(!playerHUD.activeInHierarchy);
    }

    // *-----  MENUS  ---------------*
    public void TogglePauseMenu() {
        if (!isPaused) {
            PauseGame();
            MusicManager.Instance.ReduceVolume();
            playerHUD.SetActive(false);
            pauseMenu.SetActive(true);
        } else {
            UnpauseGame();
            MusicManager.Instance.ResetVolume();
            playerHUD.SetActive(true);
            pauseMenu.SetActive(false);
        }
    }
    public void EnableVictoryMenu() {
        PauseGame();
        playerHUD.SetActive(false);
        victoryMenu.SetActive(true);
    }
    public void EnableGameOverMenu() {
        PauseGame();
        playerHUD.SetActive(false);
        gameOverMenu.SetActive(true);
    }


    // *-----  MENU FUNCTIONALITY  ---------------*
    private void PauseGame() {
        if (!isPaused) {
            isPaused = !isPaused;
            Time.timeScale = 0f;
            Player.Instance.DisableControls();
            Weapon.Instance.DisableWeapons();
        }
    }
    private void UnpauseGame() {
        if (isPaused) {
            isPaused = !isPaused;
            Time.timeScale = 1f;
            Player.Instance.EnableControls();
            Weapon.Instance.EnableWeapons();
        }
    }

    public void Retry() {
        ResetMenus();
        SceneManager.LoadScene(level1Scene);
    }
    public void QuitToMain() {
        ResetMenus();
        SceneManager.LoadScene(mainMenuScene);
    }

    private void ResetMenus() {
        UnpauseGame();
        GameManager.Instance.ResetGameObjects();
        if (playerHUD) playerHUD.SetActive(true);
        pauseMenu.SetActive(false);
        victoryMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }
}
