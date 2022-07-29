using Assets.Code.Stages.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code {
    public class Fader : MonoBehaviour {
        public Animator faderAnimator;

        private void Awake() {
            FadeOut();
        }

        public void FadeIn() {
            faderAnimator.SetTrigger("fadein");
            MusicMaster.musicMasterInstance.StopMusic();
        }

        private void FadeOut() {
            if (SceneManager.GetActiveScene().buildIndex == 0) MusicMaster.musicMasterInstance.PlayMusicInMenu();
            else if(SceneManager.GetActiveScene().buildIndex == 6) MusicMaster.musicMasterInstance.PlayMusicInMarket();
            if (SceneManager.GetActiveScene().buildIndex > 0 && SceneManager.GetActiveScene().buildIndex < 4 || SceneManager.GetActiveScene().buildIndex == 8) {
                StartCoroutine(WaitForRightMomentToFadeOut());
            } else if(SceneManager.GetActiveScene().buildIndex == 0) {
                StartCoroutine(WaitForVideoToLoad());
            } else {
                faderAnimator.SetTrigger("fadeout");
            }
        }

        private IEnumerator WaitForVideoToLoad() {
            yield return new WaitForSeconds(1f);

            faderAnimator.SetTrigger("fadeout");
        }

        private IEnumerator WaitForRightMomentToFadeOut() {
            PlayerMB player = FindObjectOfType<PlayerMB>();
            
            yield return new WaitUntil(() => player.canStart);

            faderAnimator.SetTrigger("fadeout");
        }
    }
}