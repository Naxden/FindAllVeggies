using Assets.Code.Market;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Code.Stages {
    public class AdsMenagerMB : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener {

        public static AdsMenagerMB AdvertisementMenagerInstance { get; private set; }

        private string rewardedAd = "Rewarded_Android", normalAd = "Interstitial_Android";

        public bool adsEnabled = false;

        public void WatchRewardedAd() {
            Advertisement.Show(rewardedAd, this);
        }

        public void WatchNormalAd() {
            Advertisement.Show(normalAd, this);
        }

        private void Awake() {
            AdvertisementSingletonSetup();
            Advertisement.Initialize("4837511", true, this);
        }

        private void Start() {
            //There is need to be a time delay between Initialization, Loading and Showing ad.
            Invoke("FirstLoadAds", 5f);
            SceneManager.activeSceneChanged += LoadAds;
        }
        private void FirstLoadAds()
        {
            Advertisement.Load(rewardedAd, this);
        }
        private void LoadAds(Scene before, Scene Loaded)
        {
            if (WhichScene(Loaded.buildIndex) == 0)
                Advertisement.Load(normalAd, this);
            if (WhichScene(Loaded.buildIndex) == 1)
                Advertisement.Load(rewardedAd, this);
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch (WhichScene(SceneManager.GetActiveScene().buildIndex))
            {
                case 0:
                    UnityAdAfterMarket(showCompletionState);
                    break;
                case 1:
                    UnityAdAfterStage(showCompletionState);
                    break;
                default:
                    break;
            }
        }

        public void OnUnityAdsDidFinish(string placementId, UnityAdsShowCompletionState showCompletionState) {
            switch (WhichScene(SceneManager.GetActiveScene().buildIndex)) {
                case 0:
                    UnityAdAfterMarket(showCompletionState);
                    break;
                case 1:
                    UnityAdAfterStage(showCompletionState);
                    break;
                default:
                    break;
            }
        }

        private void UnityAdAfterStage(UnityAdsShowCompletionState showCompletionState) {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED) {
                PersistentGameMasterMB.GameMasterInstance.SumScore(true);
                ChangeScene(6);
            } else if(showCompletionState == UnityAdsShowCompletionState.UNKNOWN || 
                      showCompletionState == UnityAdsShowCompletionState.SKIPPED) {
                ChangeScene(6);
            }
        }

        private void UnityAdAfterMarket(UnityAdsShowCompletionState showCompletionState) {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED || 
                showCompletionState == UnityAdsShowCompletionState.SKIPPED || 
                showCompletionState == UnityAdsShowCompletionState.UNKNOWN) {
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

        private void AdvertisementSingletonSetup() {         //If there is no other instance of this object set the instance to this one else destroy this game object
            if (AdvertisementMenagerInstance == null) {      //the result is that there is always only one instance of this script and it keeps its values whan changing
                AdvertisementMenagerInstance = this;         //scenes.
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {placementId} - {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) 
        {
            Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId) { }

        public void OnUnityAdsShowClick(string placementId) { }

        public void OnUnityAdsAdLoaded(string placementId) { Debug.Log($"AdLoaded:  {placementId}"); }

        public void OnDisable()
        {
            SceneManager.activeSceneChanged -= LoadAds;
        }

    }
}
