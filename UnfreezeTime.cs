using UnityEngine;

namespace Assets.Code {
    public class UnfreezeTime : MonoBehaviour {
        void Start() {
            Time.timeScale = 1f;
        }
    }
}