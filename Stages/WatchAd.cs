using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Stages.MenuMB;

namespace Assets.Code.Stages.WatchAd {
    public class WatchAd : MonoBehaviour {
        public Fader fader;

        public void WatchRewarded() {
            if (AdsMenagerMB.AdvertisementMenagerInstance.adsEnabled == false)
                GameObject.FindObjectOfType<MenuMB.MenuMB>().LoadMarket();
            else 
                StartCoroutine(WatchRewardedOne());
        }

        private bool ReadyToChange() {
            return fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadeIn") && fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1f;
        }

        private IEnumerator WatchRewardedOne() {
            fader.FadeIn();
            yield return new WaitUntil(() => ReadyToChange());
            AdsMenagerMB.AdvertisementMenagerInstance.WatchRewardedAd();
        }
    }
}