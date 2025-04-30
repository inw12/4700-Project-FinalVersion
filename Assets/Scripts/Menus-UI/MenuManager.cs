using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;

    [SerializeField] private GameObject playerHUD;

    [SerializeField] private GameObject fadeToBlack;
    [SerializeField] private GameObject fadeFromBlack;

    [SerializeField] private PlayerControls playerControls;

    private bool isPaused;
    //private readonly string gameScene = "PrototypeScene";
    private readonly string menuScene = "MainMenu";

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
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

            Player.Instance.gameObject.SetActive(false);
        }
    }

    public void GameOverMenu() {
        isPaused = !isPaused;

        if (isPaused) {
            Time.timeScale = 0f;
            gameOverMenu.SetActive(true);
        }
    }

    public void Resume() {
        Time.timeScale = 1f;

        playerHUD.SetActive(true);
        pauseMenu.SetActive(false);

        Player.Instance.gameObject.SetActive(true);
        playerControls.Enable();
    }

    public void TryAgain() {
        Debug.Log("Resetting Time.timeScale to 1 and reloading the scene.");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMain() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }
}
