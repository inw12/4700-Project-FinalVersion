using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource battleMusic;

    private readonly float fadeDuration = 1.25f;

    private readonly float backgroundVolume = 0.5f;
    private readonly float battleVolume = 0.35f;

    public void PlayBackgroundMusic() {
        StartCoroutine(FadeOut(battleMusic));
        StartCoroutine(FadeIn(backgroundMusic, backgroundVolume));
    }
    
    public void PlayBattleMusic() {
        StartCoroutine(FadeOut(backgroundMusic));
        StartCoroutine(FadeIn(battleMusic, battleVolume));
    }
    
    private IEnumerator FadeOut(AudioSource source) {
        float startVolume = source.volume;
        while (source.volume > 0) {
            source.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        source.Pause();
    }

    private IEnumerator FadeIn(AudioSource source, float targetVolume) {
        source.volume = 0;
        source.Play();
        while (source.volume < targetVolume) {
            source.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }
        source.volume = targetVolume;
    }
}