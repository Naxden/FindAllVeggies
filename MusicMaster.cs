using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code
{
    public class MusicMaster : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip audioClipMenu, audioClipMarket;
        public static MusicMaster musicMasterInstance { get; private set; }
        private void Awake()
        {
            SingletonSetup();
        }
        private void SingletonSetup()
        {
            if (musicMasterInstance == null)
            {
                musicMasterInstance = this;
                DontDestroyOnLoad(gameObject);
            } else
            {
                Destroy(gameObject);
            }
        }
        public void PlayMusicInMenu()
        {
            audioSource.clip = audioClipMenu;
            audioSource.Play();
        }
        public void PlayMusicInMarket()
        {
            audioSource.clip = audioClipMarket;
            audioSource.Play();
        }
        public void StopMusic()
        {
            audioSource.Stop();
        }
    }
}