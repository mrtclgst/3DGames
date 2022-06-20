using UnityEngine;
using UnityEngine.Audio;
using System;

namespace EndlessRunner
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public Sound[] sounds;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            foreach (Sound s in sounds)
            {
                s.audioSource = gameObject.AddComponent<AudioSource>();
                s.audioSource.clip = s.clip;
                s.audioSource.volume = s.volume;
            }
        }
        private void Start()
        {
            Play("MainTheme");
        }
        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sounds => sounds.name == name);
            s.audioSource.Play();

        }
    }
}