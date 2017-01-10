using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    public AudioClip[] LevelMusic;

    static AudioSource Music;

    void Start() {
        DontDestroyOnLoad(gameObject);
        Music = GetComponent<AudioSource>();
        ChangeMusic(0);
    }

    public void ChangeMusic(int Track) {
        if (LevelMusic.Length >= Track) {
            Music.Stop();
            Music.clip = LevelMusic[Track];
            Music.loop = true;
            Music.Play();
        }
    }

    public static void ChangeMusic(AudioClip Track) {
        Music.Stop();
        Music.clip = Track;
        Music.loop = true;
        Music.Play();
    }
}
