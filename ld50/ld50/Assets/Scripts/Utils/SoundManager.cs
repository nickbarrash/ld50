using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public Sound[] sounds;

    public const string TOKEN = "collect token";
    public const string DECREMENT = "decrement increase";
    public const string COMBO = "combo finish";

    private void Awake() {
        foreach (var sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
    }

    public void Play(string sound, float pitch = 1) {
        foreach (var s in sounds) {
            if (s.name == sound) {
                s.source.pitch = pitch;
                s.source.Play();
                return;
            }
        }
    }
}
