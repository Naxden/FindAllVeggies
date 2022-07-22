using Assets.Code.Market;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Code.Stages {
    public class AdsMenagerMB : MonoBehaviour, IUnityAdsListener {

        public static AdsMenagerMB AdvertisementMenagerInstance { get; private set; }

        private string rewardedAd = "rewardedVideo", normalAd = "video";

        public bool adsEnabled { get; private set; } = false;

        private void Awake() {
            AdvertisementSingletonSetup();
        }

        private void Start() {
            Advertisement.AddListener(this);
            Advertisement.Initialize("3831159", true);
        }

        public void WatchRewardedAd() {
            Advertisement.Show(rewardedAd);
        }       

        public void WatchNormalAd() {
            Advertisement.Show(normalAd);
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
            switch (WhichScene(SceneManager.GetActiveScene().buildIndex)) {
                case 0:
                    UnityAdAfterMarket(showResult);
                    break;
                case 1:
                    UnityAdAfterStage(showResult);
                    break;
                default:
                    break;
            }
        }

        private void UnityAdAfterStage(ShowResult showResult) {
            if (showResult == ShowResult.Finished) {
                PersistentGameMasterMB.GameMasterInstance.SumScore(true);
                ChangeScene(6);
            } else if(showResult == ShowResult.Failed) {
                ChangeScene(6);
            }
        }

        private void UnityAdAfterMarket(ShowResult showResult) {
            if (showResult == ShowResult.Failed || showResult == ShowResult.Skipped || showResult == ShowResult.Finished) {
                try {
                    GameObject.FindObjectOfType<BackButtonMB>().adWatched = true;
                } catch { }
            }
        }

        private int WhichScene(int sceneIndex) {
            switch (sceneIndex) {
                case 0: return -1;
                case 1: 
                case 2: 
                case 3:
                case 4:
                case 5: return 1;
                case 6: return 0;
                case 7: return -1;
                case 8: return 1;
                default: return -1;
            }
        }

        private void ChangeScene(int sceneIndex) {
            Time.timeScale = 1f;

            SceneManager.LoadScene(sceneIndex);
        }

        public void OnUnityAdsDidStart(string placementId) {
        }

        public void OnUnityAdsReady(string placementId) {
        }

        public void OnUnityAdsDidError(string message) {
        }

        private void AdvertisementSingletonSetup() {         //If there is no other instance of this object set the instance to this one else destroy this game object
            if (AdvertisementMenagerInstance == null) {      //the result is that there is always only one instance of this script and it keeps its values whan changing
                AdvertisementMenagerInstance = this;         //scenes.
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }
    }
}
