using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Market {
    public class BuyButtonsEffects : MonoBehaviour {
        public AudioSource source, source2;
        public AudioClip hoverEffect;
        public Animator animator;

        public void HoverSound() {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Hover") || animator.GetCurrentAnimatorStateInfo(0).IsName("Visable")) {
                source2.PlayOneShot(hoverEffect);
                animator.SetTrigger("hover");
            }
        }
    }
}
