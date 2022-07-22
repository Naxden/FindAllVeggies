using UnityEngine;

namespace Assets.Code.Menu.Logo {
    public class LogoShake : MonoBehaviour {
        public Animator animator;
        
        public void Hover() {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hover") || animator.GetCurrentAnimatorStateInfo(0).IsName("Visable")) {
                animator.SetTrigger("hover");
            }
        }
    }
}
