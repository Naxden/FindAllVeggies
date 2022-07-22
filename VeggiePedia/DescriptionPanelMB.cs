using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Stages.Collectible;
using System;
using UnityEngine.UI;
using TMPro;

namespace Assets.Code.VeggiePedia
{
    public class DescriptionPanelMB : MonoBehaviour
    {
        public Animator descriptionPanelAnimator;
        public Button descriptionPanelButton;

        public TextMeshProUGUI nameText, statsText;
        public Image veggieImage;
        
        private int catchQuantity, veggieValue;
        private float veggieWeight;
        private CollectibleSO.VegieType vegieType;
        private string veggieName;
        private Sprite veggieSprite;
        private float imageScaleMultiplier;

        public VeggieButtonMB[] veggieButtons;
        
        private string GetVeggieStatsString()
        {
            return "Catch Count   " + catchQuantity.ToString() + "\n" +
                "Base Price " + veggieValue.ToString() + "       " +
                "Weight " + veggieWeight.ToString() + " g";
        }

        private void WriteDownOnPanel()
        {
            nameText.text = veggieName;
            veggieImage.sprite = veggieSprite;
            veggieImage.transform.localScale = Vector3.one * imageScaleMultiplier / 2;
            statsText.text = GetVeggieStatsString();
        }
        private void SetPanelsVariables(CollectibleSO collectibleSO, int catchQuantity, float imageScaleMultiplier, string veggieName)
        {
            this.catchQuantity = catchQuantity;
            vegieType = collectibleSO.vegieType;

            this.veggieName = veggieName;
            veggieSprite = collectibleSO.sprite;
            this.imageScaleMultiplier = imageScaleMultiplier;
            veggieValue = collectibleSO.value;
            veggieWeight = Mathf.Round(700 / collectibleSO.pullSpeed);
        }
        public void ShowPanelProcedure(CollectibleSO collectibleSO, int catchQuantity, float imageScaleMultiplier, string veggieName)
        {
            SetPanelsVariables(collectibleSO, catchQuantity, imageScaleMultiplier,veggieName);
            WriteDownOnPanel();
            ShowPanel();
            HideButtons();
        }

        private void ShowPanel()
        {
            descriptionPanelAnimator.SetTrigger("StartSlidingIn");
            descriptionPanelButton.enabled = true;
        }
        private void HideButtons()
        {
            for (int i = 0; i < veggieButtons.Length; i++)
                veggieButtons[i].HideButton();
        }
        public void HidePanel()
        {
            descriptionPanelAnimator.ResetTrigger("StartSlidingIn");
            descriptionPanelAnimator.SetTrigger("StrartSlidingOut");
            descriptionPanelButton.enabled = false;

            ShowButtons();
        }
        private void ShowButtons()
        {
            for (int i = veggieButtons.Length - 1; i >= 0; i--)
                veggieButtons[i].ShowButton();
        }
    }
}
