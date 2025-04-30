using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private readonly string startingScene = "PrototypeScene";

    public void StartGame() {
        SceneManager.LoadScene(startingScene);
    }

    public void QuitToDesktop() {
        Application.Quit();
    }
}
