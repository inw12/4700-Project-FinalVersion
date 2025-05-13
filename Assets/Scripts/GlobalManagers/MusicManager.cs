using UnityEngine;
using System.Collections;

public class MusicManager : Singleton<MusicManager>
{
    private AudioSource songA;  // for background music
    private AudioSource songB;  // for battle music

    private readonly float defaultFadeDuration = 1.2f;
    private readonly float defaultVolume = 0.5f;

    protected override void Awake() {
        base.Awake();

        songA = GetComponents<AudioSource>()[0];
        songB = GetComponents<AudioSource>()[1];
    }


    public void LoadSongs(AudioClip newsongA, AudioClip newsongB) {
        songA.clip = newsongA;
        songB.clip = newsongB;
    }
    public void FadeToSongA() {
        StartCoroutine(FadeOut(songB, defaultFadeDuration));
        StartCoroutine(FadeIn(songA, defaultFadeDuration));        
    }
    public void FadeToSongB() {
        StartCoroutine(FadeOut(songA, defaultFadeDuration));
        StartCoroutine(FadeIn(songB, defaultFadeDuration));        
    }
    public void FadeToSongA(float fadeDuration) {
        StartCoroutine(FadeOut(songB, fadeDuration));
        StartCoroutine(FadeIn(songA, fadeDuration));        
    }
    public void FadeToSongB(float fadeDuration) {
        StartCoroutine(FadeOut(songA, fadeDuration));
        StartCoroutine(FadeIn(songB, fadeDuration));        
    }


    // Fade out current song
    private IEnumerator FadeOut(AudioSource source, float fadeDuration) {
        float startVolume = source.volume;
        while (source.volume > 0) {
            source.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        source.Pause();
    }
    // Fade in next song
    private IEnumerator FadeIn(AudioSource source, float fadeDuration) {
        source.volume = 0;
        source.Play();
        while (source.volume < defaultVolume) {
            source.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }
        source.volume = defaultVolume;
    }

    public void ResetAudio() {
        if (songA.isPlaying) {
            songA.Stop();
        } else {
            songB.Stop();
        }
        songA.clip = null;
        songB.clip = null;
    }
}
