using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public float volume;
        public bool isLoop;
        public AudioSource audioSource;
    }
}