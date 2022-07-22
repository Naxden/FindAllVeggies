using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Assets.Code.Menu.Settings {
    public class MusicButton : MonoBehaviour {
        public Sprite on, off;
        public Image status;

        private void Update() {
            if (SettingsMaster.SettingsMasterInstance.m_muted) {
                status.sprite = off;
            } else {
                status.sprite = on;
            }
        }

        public void MuteOrUnmute() {
            if (SettingsMaster.SettingsMasterInstance.m_muted) {
                PlayerPrefs.SetInt("m_muted", 0);
                SettingsMaster.SettingsMasterInstance.m_muted = false;
                SettingsMaster.SettingsMasterInstance.m_mixer.SetFloat("MusicVolume", 0);
            } else {
                PlayerPrefs.SetInt("m_muted", 1);
                SettingsMaster.SettingsMasterInstance.m_muted = true;
                SettingsMaster.SettingsMasterInstance.m_mixer.SetFloat("MusicVolume", -80);
            }
        }
    }
}