using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Code.Menu {
    public class MenuButtons : MonoBehaviour {
        public Fader fader;

        public Animator[] elementsToShrink;
        public Animator[] settingsElementsToShow;
        public Animator[] creditsElementsToShow;

        private void Awake() {
            Application.targetFrameRate = 60;

            if (SceneManager.GetActiveScene().buildIndex == 0 && PlayerPrefs.GetInt("SceneUpgrade") == 0) {
                    SetFirstSettings();
            }
        }

        public void StartGame() {
            //TODO change to proper scene
            StartCoroutine(ChangeSceneRoutine(PlayerPrefs.GetInt("SceneUpgrade")));
        }

        public void Settings() {
            StartCoroutine(GoToSetting());
        }

        private IEnumerator GoToSetting() {
            foreach (var element in elementsToShrink) {
                try { element.gameObject.GetComponent<Button>().interactable = false; } catch { }
            }

            foreach (Animator element in elementsToShrink) {
                element.SetTrigger("shrink");
                yield return new WaitForSeconds(.02f);
            }

            //Logic gap

            foreach (var element in settingsElementsToShow) {
                try { element.gameObject.GetComponent<Button>().interactable = true; } catch { }
            }

            foreach (Animator element in settingsElementsToShow) {
                element.SetTrigger("scale");
                yield return new WaitForSeconds(.02f);
            }
        }

        private IEnumerator GoToCredits() {
            foreach (var element in elementsToShrink) {                
                try { element.gameObject.GetComponent<Button>().interactable = false;  } catch { }
            }

            foreach (Animator element in elementsToShrink) {
                element.SetTrigger("shrink");
                yield return new WaitForSeconds(.02f);
            }

            //Logic gap

            foreach (var element in creditsElementsToShow) {
                try { element.gameObject.GetComponent<Button>().interactable = true; } catch { }
            }

            foreach (Animator element in creditsElementsToShow) {
                element.SetTrigger("scale");
                yield return new WaitForSeconds(.02f);
            }
        }

        public void Credits() {
            StartCoroutine(GoToCredits());
        }

        public void QuitGame() {
            Application.Quit();
        }

        private bool ReadyToChange() {
            return fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadeIn") && fader.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1f;
        }

        private void SetFirstSettings() {
            PlayerPrefs.SetInt("timeUpgrades", 0);
            PlayerPrefs.SetInt("valueUpgrades", 0);
            PlayerPrefs.SetInt("luckUpgrades", 0);
            PlayerPrefs.SetInt("strengthUpgrades", 0);
            PlayerPrefs.SetInt("spraysUpgrades", 0);
            PlayerPrefs.SetInt("Money", 0);
            PlayerPrefs.SetInt("extendTimePrice", 400);
            PlayerPrefs.SetInt("upgradeBargainingPrice", 1000);
            PlayerPrefs.SetInt("betterLuckPrice", 500);
            PlayerPrefs.SetInt("moreStrengthPrice", 200);
            PlayerPrefs.SetInt("buySpraysPrice", 600);

            PlayerPrefs.SetInt("SceneUpgrade", 1);

            for (int i = 0; i < 86; i++)
            {
                PlayerPrefs.SetInt("veggie" + i, 0);
            }

            PersistentGameMasterMB.GameMasterInstance.LoadStats();
            PersistentGameMasterMB.GameMasterInstance.SetStats();
            PersistentGameMasterMB.GameMasterInstance.SetTotalScore();
        }

        private IEnumerator ChangeSceneRoutine(int sceneIndex) {
            fader.FadeIn();

            Time.timeScale = 1f;

            yield return new WaitUntil(() => ReadyToChange());

            SceneManager.LoadScene(sceneIndex);
        }
    }
}