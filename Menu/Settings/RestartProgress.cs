using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Menu.Settings {
    public class RestartProgress : MonoBehaviour {
        public Animator[] elementsToShrinkWhileShowingQuestionMenu;
        public Animator[] elementsToShowWhileShowingQuestionMenu;

        public Animator[] elementsToShrinkWhileHidingQuestionMenu;
        public Animator[] elementsToShowWhileHidingQuestionMenu;

        public void BringQuestionMenuOutButton() {
            StartCoroutine(ShowQuestionMenu());
        }

        public void HideQuestionMenuButton() {
            StartCoroutine(HideQuestionMenu());
        }

        private IEnumerator ShowQuestionMenu() {
            foreach (var element in elementsToShrinkWhileShowingQuestionMenu) {
                try { element.gameObject.GetComponent<Button>().interactable = false; } catch { }
            }

            foreach (Animator element in elementsToShrinkWhileShowingQuestionMenu) {
                element.SetTrigger("shrink");
                yield return new WaitForSeconds(.02f);
            }

            //Logic gap

            foreach (var element in elementsToShowWhileShowingQuestionMenu) {
                try { element.gameObject.GetComponent<Button>().interactable = true; } catch { }
            }

            foreach (Animator element in elementsToShowWhileShowingQuestionMenu) {
                try { element.gameObject.GetComponent<Button>().interactable = true; } catch { }
                element.SetTrigger("scale");
                yield return new WaitForSeconds(.02f);
            }
        }

        private IEnumerator HideQuestionMenu() {
            foreach (var element in elementsToShrinkWhileHidingQuestionMenu) {
                try { element.gameObject.GetComponent<Button>().interactable = false; } catch { }
            }

            foreach (Animator element in elementsToShrinkWhileHidingQuestionMenu) {
                element.SetTrigger("shrink");
                yield return new WaitForSeconds(.02f);
            }

            //Logic gap

            foreach (var element in elementsToShowWhileHidingQuestionMenu) {
                try { element.gameObject.GetComponent<Button>().interactable = true; } catch { }
            }

            foreach (Animator element in elementsToShowWhileHidingQuestionMenu) {
                try { element.gameObject.GetComponent<Button>().interactable = true; } catch { }
                element.SetTrigger("scale");
                yield return new WaitForSeconds(.02f);
            }
        }

        public void RestartProgressButton() {
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

            for(int i = 0; i < 86; i++) {
                PlayerPrefs.SetInt("veggie" + i, 0);
            }

            PersistentGameMasterMB.GameMasterInstance.LoadStats();
            PersistentGameMasterMB.GameMasterInstance.SetStats();
            PersistentGameMasterMB.GameMasterInstance.SetTotalScore();

            StartCoroutine(HideQuestionMenu());
        }
    }
}
