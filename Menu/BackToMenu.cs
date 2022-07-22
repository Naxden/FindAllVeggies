using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Menu { 
    public class BackToMenu : MonoBehaviour {
        public Animator[] elementsToShrink;
        public Animator[] elementsToShow;

        public void Back() {
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

            foreach (var element in elementsToShow) {
                try { element.gameObject.GetComponent<Button>().interactable = true; } catch { }
            }

            foreach (Animator element in elementsToShow) {
                try { element.gameObject.GetComponent<Button>().interactable = true; } catch { }
                element.SetTrigger("scale");
                yield return new WaitForSeconds(.02f);
            }
        }
    }
}