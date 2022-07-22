using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Market {
    public class LoadVeggiePedia : MonoBehaviour {
        public Fader fader;

        public void LoadPedia() {
            StartCoroutine(ChangeSceneRoutine(7));
        }

        private bool ReadyToChange() {
            return fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadeIn") && fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1f;
        }

        private IEnumerator ChangeSceneRoutine(int sceneIndex) {
            fader.FadeIn();

            Time.timeScale = 1f;

            yield return new WaitUntil(() => ReadyToChange());

            SceneManager.LoadScene(sceneIndex);
        }
    }
}