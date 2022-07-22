using Assets.Code.Stages.Player;
using Assets.Code.Stages.Tasks;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Code.Stages.Timer {
    public class TimerMB : MonoBehaviour {
        #region variables

        [Header("Links")]

        public TextMeshProUGUI display;

        public PersistentGameMasterMB gameMaster;

        public PlayerMB hook;

        public Animator timesUpMenu, pauseButton;

        public GameObject tapToBegin;

        public TasksMB tasks; 

        [Header("Variables")]
        [Space(10)]
        public float time;

        [HideInInspector]
        public bool gameInProgress;

        #endregion

        private void Start() {
            SetTime();
            HideSomeELements();
        }

        private void Update() {
            BeginGame();
            CalculateTime();
            DisplayTime();
            EndTime();
        }

        #region functions

        private void SetTime() {
            StartCoroutine(TimeToCalculate());
        }       

        private void HideSomeELements() {
            pauseButton.gameObject.SetActive(false);
        }

        private IEnumerator TimeToCalculate() {
            yield return new WaitUntil(() => hook.canStart);

            time = PersistentGameMasterMB.GameMasterInstance.upTime;

            Time.timeScale = 0.0f;
        }

        private void BeginGame() {
            if (Input.GetMouseButtonDown(0) && time > 0 && !gameInProgress) {
                gameInProgress = true;

                tasks.Begin(); //Start receaving tasks

                tapToBegin.SetActive(false);
                pauseButton.SetTrigger("scale");
                pauseButton.gameObject.SetActive(true);

                Time.timeScale = 1f;
            }
        }

        private void CalculateTime() {
            if(gameInProgress) {
                time -= Time.deltaTime;
            }
        }

        private void DisplayTime() {
            if (time < 9.49f) {
                display.text = "0" + time.ToString("F0");
            } else {
                display.text = time.ToString("F0");
            }
        }

        public void StopGame() {
            StopTimer();
            EndScene();
        }

        private void EndTime() {
            if(time <= 0 && gameInProgress) {
                StopTimer();
                EndScene();
            }
        }

        private void StopTimer() {
            gameInProgress = false;
            time = 0.0f;
        }

        private void EndScene() {
            pauseButton.SetTrigger("shrink");
            pauseButton.gameObject.SetActive(false);
            timesUpMenu.SetTrigger("appear");
            Time.timeScale = 0.0f;
        }

        #endregion
    }
}