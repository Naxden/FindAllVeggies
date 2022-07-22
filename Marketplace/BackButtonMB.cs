using Assets.Code.Stages;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Market {
    public class BackButtonMB : MonoBehaviour {
        public Fader fader;
        public bool adWatched = false;

        public void Back() {
            StartCoroutine(ChangeSceneRoutine(PlayerPrefs.GetInt("SceneUpgrade")));
        }

        public void BackMarket() {
            StartCoroutine(ChangeSceneRoutine(6, 1));
        }

        private bool ReadyToChange() {
            return fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadeIn") && fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1f;
        }

        private IEnumerator ChangeSceneRoutine(int sceneIndex, int option = 0) {
            fader.FadeIn();

            Time.timeScale = 1f;

            yield return new WaitUntil(() => ReadyToChange());

            if(option == 0 && AdsMenagerMB.AdvertisementMenagerInstance.adsEnabled) {
                int decision = UnityEngine.Random.Range(0, 1000);
                if(decision > 500) {
                    AdsMenagerMB.AdvertisementMenagerInstance.WatchNormalAd();

                    yield return new WaitUntil(() => adWatched);
                    adWatched = false;
                }
            }                        

            SceneManager.LoadScene(sceneIndex);
        }
    }
}