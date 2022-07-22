using TMPro;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Code.Market {
    public class MoneyMB : MonoBehaviour {
        public TextMeshProUGUI display;
        public Fader fader;
        public AudioSource source;
        public AudioClip notEnoughMoneySound, upgradeSound;
        public Image progressBar;
        public int[] upgradeCosts;

        private void Start() {
            PersistentGameMasterMB.GameMasterInstance.SetTotalScore();
        }

        private void Update() {
            DisplayMoney();
            ProgressBarFill();
        }

        private void DisplayMoney() {
            display.text = PersistentGameMasterMB.GameMasterInstance.totalScore.ToString() + "$";
        }

        private void ProgressBarFill() {
            progressBar.fillAmount = Mathf.Clamp01(1f - 
                (float)PersistentGameMasterMB.GameMasterInstance.totalScore / upgradeCosts[PlayerPrefs.GetInt("SceneUpgrade")]);
        }

        public void Upgrade() {
            if(Int32.Parse(display.text.Substring(0, display.text.Length - 1)) >= upgradeCosts[PlayerPrefs.GetInt("SceneUpgrade")] && PlayerPrefs.GetInt("SceneUpgrade") < 5) {
                StartCoroutine(UpgradeRoutine());
            } else {
                source.PlayOneShot(notEnoughMoneySound);
            }            
        }

        private int GetStagePriceMultip() {
            if(PlayerPrefs.GetInt("SceneUpgrade") == 2) return 3;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 3) return 8;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 4) return 13;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 5) return 20;
            return 1;
        }

        private IEnumerator UpgradeRoutine() {
            fader.FadeIn();
            PersistentGameMasterMB.GameMasterInstance.totalScore -= upgradeCosts[PlayerPrefs.GetInt("SceneUpgrade")];
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - upgradeCosts[PlayerPrefs.GetInt("SceneUpgrade")]);

            yield return new WaitUntil(() => ReadyToPlay());
            
            source.PlayOneShot(upgradeSound);        
            PlayerPrefs.SetInt("SceneUpgrade", PlayerPrefs.GetInt("SceneUpgrade") + 1);
            PlayerPrefs.SetInt("timeUpgrades", 0);
            PlayerPrefs.SetInt("valueUpgrades", 0);
            PlayerPrefs.SetInt("luckUpgrades", 0);
            PlayerPrefs.SetInt("strengthUpgrades", 0);
            PlayerPrefs.SetInt("spraysUpgrades", 0);
            PlayerPrefs.SetInt("extendTimePrice", 400 * GetStagePriceMultip());
            PlayerPrefs.SetInt("upgradeBargainingPrice", 1000 * GetStagePriceMultip());
            PlayerPrefs.SetInt("betterLuckPrice", 500 * GetStagePriceMultip());
            PlayerPrefs.SetInt("moreStrengthPrice", 200 * GetStagePriceMultip());
            PlayerPrefs.SetInt("buySpraysPrice", 600 * GetStagePriceMultip());


            PersistentGameMasterMB.GameMasterInstance.LoadStats();
            PersistentGameMasterMB.GameMasterInstance.SetStats();
            PersistentGameMasterMB.GameMasterInstance.SetTotalScore();

            yield return new WaitForSecondsRealtime(4.5f);

            SceneManager.LoadScene(6);
        }

        private bool ReadyToPlay() {
            return fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadeIn") && fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1f;
        }
    }
}