using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource battleMusic;

    private float backgroundTargetVolume;
    private float battleTargetVolume;

    private readonly float fadeDuration = 1.25f;

    private bool isFadingOut = false;
    private bool isFadingIn = false;

    private void Awake() {
        backgroundTargetVolume = backgroundMusic.volume;
        battleTargetVolume = battleMusic.volume;
    }

    public void ReducedVolume() {
        backgroundMusic.volume = backgroundTargetVolume / 2;
        battleMusic.volume = battleTargetVolume / 2;
    }

    public void DefaultVolume() {
        backgroundMusic.volume = backgroundTargetVolume;
        battleMusic.volume = battleTargetVolume;
    }

    public void PlayBackgroundMusic() {
        StartCoroutine(FadeOut(battleMusic));
        StartCoroutine(FadeIn(backgroundMusic, backgroundTargetVolume));
    }
    
    public void PlayBattleMusic() {
        StartCoroutine(FadeOut(backgroundMusic));
        StartCoroutine(FadeIn(battleMusic, battleTargetVolume));
    }

    private IEnumerator FadeOut(AudioSource source) {
        isFadingOut = true;

        if (isFadingOut) {
            float startVolume = source.volume;

            while (source.volume > 0) {
                source.volume -= startVolume * Time.deltaTime / fadeDuration;
                yield return null;
            }

            source.Pause();
            source.volume = startVolume; // reset for reuse
        }

        isFadingOut = false;
    }

    private IEnumerator FadeIn(AudioSource source, float targetVolume) {
        isFadingIn = true;

        if (isFadingIn) {
            source.volume = 0;
            source.Play();

            while (source.volume < targetVolume) {
                source.volume += Time.deltaTime / fadeDuration;
                yield return null;
            }

            source.volume = targetVolume;
        }

        isFadingIn = false;
    }
}
