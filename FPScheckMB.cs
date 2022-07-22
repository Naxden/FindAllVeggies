using TMPro;
using UnityEngine;

namespace Assets.Code {
    public class FPScheckMB : MonoBehaviour {

        [Range(10, 10000)] public int framerateTarget;
        private int avgFrameRate;

        [Space(10)]
        public TextMeshProUGUI display;

        private void Awake() {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = framerateTarget;

            if(SettingsMaster.SettingsMasterInstance.fpsShown == true) {
                //Empty
            } else {
                this.gameObject.SetActive(false);
            }
        }

        public void Update() {
            float current = 0;
            current = (int)(1f / Time.unscaledDeltaTime);
            avgFrameRate = (int)current;
            display.text = avgFrameRate.ToString() + " FPS";
        }
    }
}