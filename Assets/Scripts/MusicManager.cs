using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip battleMusic;
    [SerializeField] private AudioSource audioSource;

    private void Awake() {
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }

    public void PlayBackgroundMusic() {
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }
    
    public void PlayBattleMusic() {
        audioSource.clip = battleMusic;
        audioSource.Play();
    }
}
