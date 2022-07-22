using UnityEngine;

namespace Assets.Code.Stages.Player {
    public class LineDrawMB : MonoBehaviour {
        public LineRenderer line;

        private Vector3 startingPos;

        private void Start() {
            startingPos = transform.position;
        }

        private void Update() {
            line.SetPosition(0, startingPos);
            line.SetPosition(1, transform.position);
        }
    }
}