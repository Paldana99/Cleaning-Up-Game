using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip level_song;

    [SerializeField]
    private AudioClip intro_song;

    [SerializeField]
    private AudioClip begin_song;

    public AudioSource source_intro, source_begin, source_level;


    public void Awake() {

      source_intro.clip = intro_song;
      source_begin.clip = begin_song;
      source_level.clip = level_song;

      source_intro.loop = true;
      source_begin.loop = false;
      source_level.loop = true;
    }

    public void StopMusic() {
      source_intro.Stop();
      source_begin.Stop();
      source_level.Stop();
    }

}
