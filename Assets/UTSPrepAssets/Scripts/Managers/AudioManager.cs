using UnityEngine;
using System.Collections.Generic;

namespace UTSOrientationGamePrototype
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AudioManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("AudioManager");
                        _instance = go.AddComponent<AudioManager>();
                    }
                }
                return _instance;
            }
        }

        [System.Serializable]
        public class Sound
        {
            public string name;
            public AudioClip clip;
            [Range(0f, 1f)]
            public float volume = 1f;
            [Range(0.1f, 3f)]
            public float pitch = 1f;
            public bool loop = false;

            [HideInInspector]
            public AudioSource source;
        }

        public Sound[] musicTracks;
        public Sound[] sfxSounds;

        private float musicVolume = 1f;
        private float sfxVolume = 1f;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            foreach (Sound s in musicTracks)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume * musicVolume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }

            foreach (Sound s in sfxSounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume * sfxVolume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }

        public void PlayMusic(string name)
        {
            Sound s = System.Array.Find(musicTracks, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Music: " + name + " not found!");
                return;
            }
            s.source.Play();
        }

        public void PlaySFX(string name)
        {
            Sound s = System.Array.Find(sfxSounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("SFX: " + name + " not found!");
                return;
            }
            s.source.Play();
        }

        public void StopMusic(string name)
        {
            Sound s = System.Array.Find(musicTracks, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Music: " + name + " not found!");
                return;
            }
            s.source.Stop();
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = volume;
            foreach (Sound s in musicTracks)
            {
                s.source.volume = s.volume * musicVolume;
            }
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = volume;
            foreach (Sound s in sfxSounds)
            {
                s.source.volume = s.volume * sfxVolume;
            }
        }
    }
}