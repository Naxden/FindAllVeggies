using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Code {
    public class SettingsMaster : MonoBehaviour {
        public static SettingsMaster SettingsMasterInstance { get; private set; }

        public bool s_muted = false, m_muted = false, fpsShown = false;
        public AudioMixer s_mixer, m_mixer;

        private void Awake() {
            SingletonSetup();
        }

        private void Start() {
            CheckSoundStatus();
            CheckFpsDisplayStatus();
        }

        private void CheckFpsDisplayStatus() {
            if(PlayerPrefs.GetInt("fps_status") > 0) {
                fpsShown = true;
            } else if (PlayerPrefs.GetInt("fps_status") <= 0) {
                fpsShown = false;
            }
        }

        private void CheckSoundStatus() {
            CheckSFX();
            CheckMusic();
        }

        private void CheckSFX() {
            if (PlayerPrefs.GetInt("s_muted") > 0) {
                s_mixer.SetFloat("SoundsVolume", -80);
                s_muted = true;
            } else if (PlayerPrefs.GetInt("s_muted") <= 0) {
                s_mixer.SetFloat("SoundsVolume", 0);
                s_muted = false;
            }
        }

        private void CheckMusic() {
            if (PlayerPrefs.GetInt("m_muted") > 0) {
                m_mixer.SetFloat("MusicVolume", -80);
                m_muted = true;
            } else if (PlayerPrefs.GetInt("m_muted") <= 0) {
                m_mixer.SetFloat("MusicVolume", 0);
                m_muted = false;
            }
        }

        private void SingletonSetup() {
            if (SettingsMasterInstance == null) {
                SettingsMasterInstance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }
    }
}
