using UnityEngine;

namespace Assets.Code.Stages {
    public class DestroyAfterTimeMB : MonoBehaviour {
        public float time;

        private void Start() {
            Destroy(gameObject, time);
        }
    }
}