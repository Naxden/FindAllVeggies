using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Code.Menu.Buttons {
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(EventTrigger))]
    [RequireComponent(typeof(Animator))]
    public class ButtonEffects : MonoBehaviour {
        public AudioSource source;
        public AudioClip hoverEffect, clickEffect;
        public Animator animator;

        public void HoverSound() {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Hover") || animator.GetCurrentAnimatorStateInfo(0).IsName("Visable")) {
                source.PlayOneShot(hoverEffect);
                animator.SetTrigger("hover");
            }
        }

        public void ClickSound() {
            source.PlayOneShot(clickEffect);
        }
    }
}