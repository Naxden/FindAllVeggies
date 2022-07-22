using Assets.Code.Stages.Collectible;
using Assets.Code.Stages.Player;
using Assets.Code.Stages.Timer;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Stages.Tasks {
    public class TasksMB : MonoBehaviour {
        [Range(1f, 20f)] public float taskLength = 10f;

        public Animator houseAnim;
        public Animator converastionAnim;
        public CollectibleMB mb;
        public TimerMB timer;
        public PlayerMB player;
        public VeggieCounterMB veggieCounter;

        public SpriteRenderer iconSpriteRenderer;
        public CollectibleMB.VegieType type;

        public CollectibleSO[] collectibleSOArray;
        public System.Collections.Generic.List<CollectibleSO> collectiblesOnSceneList = new System.Collections.Generic.List<CollectibleSO>();

        private void Update() {
            if(collectiblesOnSceneList.Count <= 0 && timer.gameInProgress) timer.StopGame();
        }

        public void RemoveFromList(CollectibleMB.VegieType veggie) {
            for(int i = 0; i < collectiblesOnSceneList.Count; i++) {
                if(veggie == (CollectibleMB.VegieType)collectiblesOnSceneList[i].vegieType) {
                    collectiblesOnSceneList.RemoveAt(i);
                    break;
                }
            }
        }

        public void CheckCatchedVeggie(CollectibleMB.VegieType veggie) {
            for(int i = 0; i < 86; i++) {
                if(i == (int)veggie) {
                    PlayerPrefs.SetInt("veggie" + i, PlayerPrefs.GetInt("veggie" + i) + 1);
                    break;
                }
            }

            RemoveFromList(veggie);
            
            if (veggie == type) {
                StopAllCoroutines();

                int ammount = 0;
                for(int i = 0; i < collectibleSOArray.Length; i++) {
                    if (veggie == (CollectibleMB.VegieType)collectibleSOArray[i].vegieType) {
                        ammount = collectibleSOArray[i].value;
                        break;
                    } 
                }

                if(timer.gameInProgress) {
                    StartCoroutine(WaitScore(ammount));
                } else {
                    player.AddToScore(ammount);
                }

                StartCoroutine(ChooseVegie());
            }
        }

        private IEnumerator WaitScore(int ammount) {
            yield return new WaitForSecondsRealtime(.5f);
            
            player.AddToScore(ammount);
        }

        private int CheckIfRightIndex(int index) {
            if (collectiblesOnSceneList.Count > 0) { //If List is not empty, use it
                index = Random.Range(0, collectiblesOnSceneList.Count);
                return index;
            }
            
            index = Random.Range(0, collectibleSOArray.Length);

            if (index == collectibleSOArray.Length - 1) return CheckIfRightIndex(index);
            else return index;
        }

        private IEnumerator ChooseVegie() {
            int index = 0;
            
            if(!timer.gameInProgress) yield return new WaitForSeconds(.5f);

            while (true) {
                index = CheckIfRightIndex(index);

                if (collectiblesOnSceneList.Count > 0) {
                    iconSpriteRenderer.sprite = collectiblesOnSceneList[index].sprite;
                    type = (CollectibleMB.VegieType)collectiblesOnSceneList[index].vegieType;
                } else { 
                    iconSpriteRenderer.sprite = collectibleSOArray[index].sprite;
                    type = (CollectibleMB.VegieType)collectibleSOArray[index].vegieType;
                }

                houseAnim.SetTrigger("react");
                converastionAnim.SetTrigger("show");
                yield return new WaitForSeconds(taskLength);
            }
        }

        public void Begin() {
            collectibleSOArray = mb.collectibleSOTable;
            collectiblesOnSceneList = veggieCounter.GetGoodVegetablesList();

            StartCoroutine(ChooseVegie());
        }
    }
}