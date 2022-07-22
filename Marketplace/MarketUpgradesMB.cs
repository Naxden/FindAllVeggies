using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Market {
    public class MarketUpgradesMB : MonoBehaviour {

        public AudioClip buySound, clickSound;
        public AudioSource source;

        #region Price variables

        public TextMeshProUGUI[] priceTexts;

        public Color avileable, tooExpensive;

        [Header("Prices")]
        public int extendTimePrice;
        public int upgradeBargainingPrice;
        public int betterLuckPrice;
        public int moreStrengthPrice;
        public int buySpraysPrice;

        #endregion

        #region Images variables

        public Sprite upgradedImg, defImg;

        [Header("Images")]
        public Image[] timeUpgrades;
        public Image[] bargainingUpgrades;
        public Image[] luckUpgrades;
        public Image[] strengthUpgrades;
        public Image[] spraysUpgrades;

        #endregion

        private void Awake() {
            GetPrices(); //from Price
        }

        private void Update() {
            DisplayImages(); //from ManageImages
            DisplayPrices(); //from Price
        }

        #region ManageImages

        private void DisplayImages() {
            TimeIm();
            BargainingIm();
            LuckIm();
            StrengthIm();
            SpraysIm();
        }

        private void TimeIm() {
            for (int x = 0; x < PersistentGameMasterMB.GameMasterInstance.timeUpgrades; x++) {
                timeUpgrades[x].sprite = upgradedImg;
            }

            for (int y = PersistentGameMasterMB.GameMasterInstance.timeUpgrades; y < timeUpgrades.Length; y++) {
                timeUpgrades[y].sprite = defImg;
            }
        }

        private void BargainingIm() {
            for (int x = 0; x < PersistentGameMasterMB.GameMasterInstance.valueUpgrades; x++) {
                bargainingUpgrades[x].sprite = upgradedImg;
            }

            for (int y = PersistentGameMasterMB.GameMasterInstance.valueUpgrades; y < bargainingUpgrades.Length; y++) {
                bargainingUpgrades[y].sprite = defImg;
            }
        }

        private void LuckIm() {
            for (int x = 0; x < PersistentGameMasterMB.GameMasterInstance.luckUpgrades; x++) {
                luckUpgrades[x].sprite = upgradedImg;
            }

            for (int y = PersistentGameMasterMB.GameMasterInstance.luckUpgrades; y < luckUpgrades.Length; y++) {
                luckUpgrades[y].sprite = defImg;
            }
        }

        private void StrengthIm() {
            for (int x = 0; x < PersistentGameMasterMB.GameMasterInstance.strengthUpgrades; x++) {
                strengthUpgrades[x].sprite = upgradedImg;
            }

            for (int y = PersistentGameMasterMB.GameMasterInstance.strengthUpgrades; y < strengthUpgrades.Length; y++) {
                strengthUpgrades[y].sprite = defImg;
            }
        }

        private void SpraysIm() {
            for (int x = 0; x < PersistentGameMasterMB.GameMasterInstance.spraysUpgrades; x++) {
                spraysUpgrades[x].sprite = upgradedImg;
            }

            for (int y = PersistentGameMasterMB.GameMasterInstance.spraysUpgrades; y < spraysUpgrades.Length; y++) {
                spraysUpgrades[y].sprite = defImg;
            }
        }

        #endregion

        #region Buttons

        public void ExtendTime() {
            if(PersistentGameMasterMB.GameMasterInstance.totalScore >= extendTimePrice
                &&PersistentGameMasterMB.GameMasterInstance.timeUpgrades < 5) {

                source.PlayOneShot(buySound);
                PersistentGameMasterMB.GameMasterInstance.timeUpgrades++;
                TakeMoney(extendTimePrice);
                extendTimePrice = CalculatePrice(extendTimePrice);
                SavePrice("extendTimePrice", extendTimePrice);
                SaveUpgrade("timeUpgrades", PersistentGameMasterMB.GameMasterInstance.timeUpgrades);
                GetPrices();
            } else {
                source.PlayOneShot(clickSound);
            }
        }

        public void UpgradeBargaining() {
            if (PersistentGameMasterMB.GameMasterInstance.totalScore >= upgradeBargainingPrice
                && PersistentGameMasterMB.GameMasterInstance.valueUpgrades < 5) {

                source.PlayOneShot(buySound);
                PersistentGameMasterMB.GameMasterInstance.valueUpgrades++;
                TakeMoney(upgradeBargainingPrice);
                upgradeBargainingPrice = CalculatePrice(upgradeBargainingPrice);
                SavePrice("upgradeBargainingPrice", upgradeBargainingPrice);
                SaveUpgrade("valueUpgrades", PersistentGameMasterMB.GameMasterInstance.valueUpgrades);
                GetPrices();
            } else {
                source.PlayOneShot(clickSound);
            }
        }

        public void BetterLuck() {
            if (PersistentGameMasterMB.GameMasterInstance.totalScore >= betterLuckPrice
                && PersistentGameMasterMB.GameMasterInstance.luckUpgrades < 5) {

                source.PlayOneShot(buySound);
                PersistentGameMasterMB.GameMasterInstance.luckUpgrades++;
                TakeMoney(betterLuckPrice);
                betterLuckPrice = CalculatePrice(betterLuckPrice);
                SavePrice("betterLuckPrice", betterLuckPrice);
                SaveUpgrade("luckUpgrades", PersistentGameMasterMB.GameMasterInstance.luckUpgrades);
                GetPrices();
            } else {
                source.PlayOneShot(clickSound);
            }
        }

        public void MoreStrength() {
            if (PersistentGameMasterMB.GameMasterInstance.totalScore >= moreStrengthPrice
                && PersistentGameMasterMB.GameMasterInstance.strengthUpgrades < 5) {

                source.PlayOneShot(buySound);
                PersistentGameMasterMB.GameMasterInstance.strengthUpgrades++;
                TakeMoney(moreStrengthPrice);
                moreStrengthPrice = CalculatePrice(moreStrengthPrice);
                SavePrice("moreStrengthPrice", moreStrengthPrice);
                SaveUpgrade("strengthUpgrades", PersistentGameMasterMB.GameMasterInstance.strengthUpgrades);
                GetPrices();
            } else {
                source.PlayOneShot(clickSound);
            }
        }

        public void BuySprays() {
            if (PersistentGameMasterMB.GameMasterInstance.totalScore >= buySpraysPrice
                && PersistentGameMasterMB.GameMasterInstance.spraysUpgrades < 3) {

                source.PlayOneShot(buySound);
                PersistentGameMasterMB.GameMasterInstance.spraysUpgrades++;
                TakeMoney(buySpraysPrice);
                buySpraysPrice = CalculatePrice(buySpraysPrice);
                SavePrice("buySpraysPrice", buySpraysPrice);
                SaveUpgrade("spraysUpgrades", PersistentGameMasterMB.GameMasterInstance.spraysUpgrades);
                GetPrices();
            } else {
                source.PlayOneShot(clickSound);
            }
        }

        private void TakeMoney(int price) {
            PersistentGameMasterMB.GameMasterInstance.totalScore -= price;
            PlayerPrefs.SetInt("Money", PersistentGameMasterMB.GameMasterInstance.totalScore);
        }

        private void SaveUpgrade(string upgradeName, int value) {
            PlayerPrefs.SetInt(upgradeName, value);
        }

        #endregion

        #region Price

        private int CalculatePrice(float price) {
            price *= 2f;

            return (int)price;
        }

        private void SavePrice(string name, int value) {
            PlayerPrefs.SetInt(name, value);
        }

        private void GetPrices() {
            extendTimePrice = PlayerPrefs.GetInt("extendTimePrice");
            upgradeBargainingPrice = PlayerPrefs.GetInt("upgradeBargainingPrice");
            betterLuckPrice = PlayerPrefs.GetInt("betterLuckPrice");
            moreStrengthPrice = PlayerPrefs.GetInt("moreStrengthPrice");
            buySpraysPrice = PlayerPrefs.GetInt("buySpraysPrice");
        }

        private void DisplayPrices() {
            PriceTexts(0, extendTimePrice );
            PriceTexts(1, upgradeBargainingPrice);
            PriceTexts(2, betterLuckPrice);
            PriceTexts(3, moreStrengthPrice);
            PriceTexts(4, buySpraysPrice );

            foreach (var price in priceTexts) {
                if(price.text == "MAX") {
                    price.color = avileable;
                } else {
                    string priceString = price.text;
                    string unwantedCharacters = "$";

                    string priceToPass = priceString.Replace(unwantedCharacters, "");

                    if (Int64.Parse(priceToPass) <= PersistentGameMasterMB.GameMasterInstance.totalScore) {
                        price.color = avileable;
                    } else {
                        price.color = tooExpensive;
                    }
                }                
            }
        }

        private void PriceTexts(int index, int price) {
            if (index == 0) {
                if (PersistentGameMasterMB.GameMasterInstance.timeUpgrades > 4) {
                    priceTexts[index].text = "MAX";
                } else {
                    priceTexts[index].text = price.ToString() + "$";
                }
            } else if (index == 1) {
                if (PersistentGameMasterMB.GameMasterInstance.valueUpgrades > 4) {
                    priceTexts[index].text = "MAX";
                } else {
                    priceTexts[index].text = price.ToString() + "$";
                }
            } else if (index == 2) {
                if (PersistentGameMasterMB.GameMasterInstance.luckUpgrades > 4) {
                    priceTexts[index].text = "MAX";
                } else {
                    priceTexts[index].text = price.ToString() + "$";
                }
            } else if (index == 3) {
                if (PersistentGameMasterMB.GameMasterInstance.strengthUpgrades > 4) {
                    priceTexts[index].text = "MAX";
                } else {
                    priceTexts[index].text = price.ToString() + "$";
                }
            } else if (index == 4) {
                if(PersistentGameMasterMB.GameMasterInstance.spraysUpgrades > 2) {
                    priceTexts[index].text = "MAX";
                } else {
                    priceTexts[index].text = price.ToString() + "$";
                }
            }
        }

        #endregion
    }
}