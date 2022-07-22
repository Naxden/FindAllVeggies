using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Stages.MenuMB {
    public class MenuMB : MonoBehaviour {
        public GameObject questionMenu;
        public Fader fader;
        public Animator pauseMenu;
        public Animator pauseButton;

        public Animator questionMenuAnim, overMenuAnim;

        public void LoadMarket() {
            PersistentGameMasterMB.GameMasterInstance.SumScore(false);
            StartCoroutine(ChangeSceneRoutine(6));
        }

        public void AskAboutAd() {
            StartCoroutine(AskAboutAdRoutine());
        }

        private IEnumerator AskAboutAdRoutine() {
            overMenuAnim.SetTrigger("disappear");

            yield return new WaitForSecondsRealtime(0.3f);
            
            questionMenu.SetActive(true);
            questionMenuAnim.SetTrigger("appear");
        }

        public void MenuScene() {
            StartCoroutine(ChangeSceneRoutine(0));
        }

        public void Pause() {
            StartCoroutine(HideAndShowElementsWhilePausing());
        }

        private IEnumerator HideAndShowElementsWhilePausing() {
            pauseButton.SetTrigger("shrink");
            pauseButton.gameObject.SetActive(false);

            yield return new WaitForSecondsRealtime(0.1f);

            pauseMenu.gameObject.SetActive(true);
            pauseMenu.SetTrigger("appear");
            Time.timeScale = 0;
        }

        public void Restart() {
            StartCoroutine(ChangeSceneRoutine(PlayerPrefs.GetInt("SceneUpgrade")));
        }

        public void Resume() {
            StartCoroutine(HideAndShowElementsWhileResuming());
        }

        private IEnumerator HideAndShowElementsWhileResuming() {
            pauseMenu.SetTrigger("disappear");

            yield return new WaitForSecondsRealtime(0.2f);

            pauseMenu.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            pauseButton.SetTrigger("scale");
            Time.timeScale = 1;
        }

        private bool ReadyToChange() {
            return fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadeIn") && fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1f;
        }

        private IEnumerator ChangeSceneRoutine(int sceneIndex) {
            fader.FadeIn();

            yield return new WaitUntil(() => ReadyToChange());

            SceneManager.LoadScene(sceneIndex);
        }
    }
}