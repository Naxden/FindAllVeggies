using UnityEngine;
using System.Collections;
using Assets.Code.Stages.Player;
using UnityEngine.UI;
using TMPro;

namespace Assets.Code.Stages {
    public class ShowWhatPlayerCollectedMB : MonoBehaviour {
        public PlayerMB player;

        public Image[] images;

        public TextMeshProUGUI earnedScoreDisplay;

        private int totalScoreCollectedThisRound = 0,
        timesScoreShouldBeAdded = 0,
        reminder = 0,
        growthRate = 0;

        public int shakeBarrier = 50;
        
        public void StartShowingWhatPlayerCollected() {
            if(player.collectedObjects.Count > 0) {
                Animator textAnimator = earnedScoreDisplay.gameObject.GetComponent<Animator>();

                StartCoroutine(DisplayScoreRoutine(textAnimator));
                StartCoroutine(ShowCollectedRoutine());
            }            
        }

        private IEnumerator DisplayScoreRoutine(Animator textAnimator) {
            yield return new WaitForSecondsRealtime(.2f);

            totalScoreCollectedThisRound = player.score;
            growthRate = player.score / 100;
            if(growthRate < 1) growthRate = 1;
            timesScoreShouldBeAdded = totalScoreCollectedThisRound / growthRate;
            reminder = totalScoreCollectedThisRound - (timesScoreShouldBeAdded * growthRate);

            int currentDisplay = 0;

            for(int i = 0; i < timesScoreShouldBeAdded; i++) {
                if(currentDisplay >= shakeBarrier && textAnimator.GetCurrentAnimatorStateInfo(0).IsName("Static")) { //And not already shaking
                    textAnimator.SetTrigger("shake");
                }

                earnedScoreDisplay.text = (currentDisplay + growthRate).ToString() + '$';
                currentDisplay += growthRate;

                yield return new WaitForSecondsRealtime(1.0f / timesScoreShouldBeAdded);
            }

            earnedScoreDisplay.text = (currentDisplay + reminder).ToString() + '$';
            textAnimator.SetTrigger("stop");
        }

        private IEnumerator ShowCollectedRoutine() {
            int i = 0;

            foreach(var p_sprite in player.collectedObjects) {
                images[i].color = Color.white;

                images[i].sprite = p_sprite;

                images[i].gameObject.GetComponent<Animator>().SetTrigger("pop");

                yield return new WaitForSecondsRealtime(.1f);
                
                i++;
            }
        }
    }
}
