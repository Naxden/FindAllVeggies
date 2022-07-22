using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Stages.Collectible;

namespace Assets.Code.VeggiePedia
{
    public class VeggieButtonMB : MonoBehaviour
    {
        [SerializeField]
        private int catchQuantity;
        public DescriptionPanelMB descriptionPanelMB;
        public float scaleMultiplier;
        public CollectibleSO collectibleSO;
        public string veggieName;
        public Button button;
        public Animator animator;
        public Image image;

        private void UnlockButton()
        {
            button.enabled = true;
            image.color = new Color(255, 255, 255);
        }

        private void SetCachtQuantity()
        {
            try
            {
                string playerPrefsKey = "veggie" + ((int)collectibleSO.vegieType).ToString();
                catchQuantity = PlayerPrefs.GetInt(playerPrefsKey, 0);
            }
            catch
            {
                catchQuantity = 0;
            }
        }
        private void Awake()
        {
            SetCachtQuantity();
            if (catchQuantity > 0)
                UnlockButton();
        }
        
        public void PassInformationToPanel()
        {
            scaleMultiplier = image.transform.localScale.x;
            descriptionPanelMB.ShowPanelProcedure(collectibleSO, catchQuantity, scaleMultiplier, veggieName);
        }
        
        public void HideButton()
        {
            animator.SetTrigger("Hide");
            button.enabled = false;
        }
        public void ShowButton()
        {
            animator.SetTrigger("Show");
            if (catchQuantity > 0)
                button.enabled = true;
        }
        
    }
}
