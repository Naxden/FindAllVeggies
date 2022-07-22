using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Menu.Settings {
    public class SoundButton : MonoBehaviour {
        public Sprite on, off;
        public Image status;

        private void Update() {
            if (SettingsMaster.SettingsMasterInstance.s_muted) {
                status.sprite = off;
            } else {
                status.sprite = on;
            }
        }

        public void MuteOrUnmute() {
            if (SettingsMaster.SettingsMasterInstance.s_muted) {
                PlayerPrefs.SetInt("s_muted", 0);
                SettingsMaster.SettingsMasterInstance.s_muted = false;
                SettingsMaster.SettingsMasterInstance.s_mixer.SetFloat("SoundsVolume", 0);
            } else {
                PlayerPrefs.SetInt("s_muted", 1);
                SettingsMaster.SettingsMasterInstance.s_muted = true;
                SettingsMaster.SettingsMasterInstance.s_mixer.SetFloat("SoundsVolume", -80);
            }
        }
    }
}