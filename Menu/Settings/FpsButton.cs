using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Menu.Settings {
    public class FpsButton : MonoBehaviour {
        public Sprite on, off;
        public Image status;

        private void Update() {
            if (SettingsMaster.SettingsMasterInstance.fpsShown) {
                status.sprite = on;
            } else {
                status.sprite = off;
            }
        }

        public void OnOrOff() {
            if (SettingsMaster.SettingsMasterInstance.fpsShown) {
                SettingsMaster.SettingsMasterInstance.fpsShown = false;
                PlayerPrefs.SetInt("fps_status", 0);
            } else {
                SettingsMaster.SettingsMasterInstance.fpsShown = true;
                PlayerPrefs.SetInt("fps_status", 1);
            }
        }
    }
}
