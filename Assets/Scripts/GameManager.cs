using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject playerPrefab;
    private GameObject playerInstance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (playerInstance == null)
        {
            playerInstance = Instantiate(playerPrefab);
            playerInstance.SetActive(false);
            DontDestroyOnLoad(playerInstance);
        }
    }

    public void RestartGame(string sceneName)
    {
        // Optional: clean up or reset other data
        SceneManager.LoadScene(sceneName);
    }
}