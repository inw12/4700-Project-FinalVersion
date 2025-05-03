using UnityEngine;

public class MusicManager_BossRoom : MonoBehaviour
{
    [SerializeField] private AudioSource music;

    private float defaultVolume;

    private void Awake() {
        defaultVolume = music.volume;
    }

    public void ReducedVolume() {
        if (music) music.volume = defaultVolume / 3;
    }

    public void DefaultVolume() {
        if (music) music.volume = defaultVolume;
    }
}
