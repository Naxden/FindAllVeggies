using System;
using Assets.Code.Stages.Player;
using UnityEngine;

namespace Assets.Code {
    public class PersistentGameMasterMB : MonoBehaviour {

        public static PersistentGameMasterMB GameMasterInstance { get; private set; }

        #region Statistics variables

        [Header("Upgrades")]
        [Range(0, 5)] public int timeUpgrades;
        [Range(0, 5)] public int valueUpgrades;
        [Range(0, 5)] public int luckUpgrades;
        [Range(0, 5)] public int strengthUpgrades;
        [Range(0, 3)] public int spraysUpgrades;

        [Header("Stats")]
        public float upTime;

        public float valueMultiplier;

        public float spawnOffsetChance;
        public float pullSpeedMultiplier;

        public int sprays;

        #endregion

        #region Score variables

        [Header("Score")]
        public int totalScore;

        #endregion

        private PlayerMB hook;

        private void Awake() {
            SingletonSetup(); //from Singleton
            LoadStats(); //from Statistics            
        }

        private void Start() {

            #region Try to get player component

            try {
                hook = FindObjectOfType<PlayerMB>();
            } catch {
                print("No player in this scene");
            }

            #endregion

            SetTotalScore(); //from Score
            SetStats(); //from Statistics
        }

        private void Update() {
            Restrictions(); //from Restrict upgrades
        }

        #region Score

        public void SumScore(bool watchedAdd) {
            if(watchedAdd) {
                try { hook = FindObjectOfType<PlayerMB>(); } catch { }
                totalScore += (int)(hook.score * 1.2f);
                PlayerPrefs.SetInt("Money", totalScore);
            } else {
                try { hook = FindObjectOfType<PlayerMB>(); } catch { }
                totalScore += hook.score;
                PlayerPrefs.SetInt("Money", totalScore);
            }            
        }

        public void SetTotalScore() {
            totalScore = PlayerPrefs.GetInt("Money");
        }

        #endregion

        #region Restrict upgrades

        private void Restrictions() {
            TimeRest();
            BargainingRest();
            LuckRest();
            StrengthRest();
            SpraysRest();
        }

        private void TimeRest() {
            if (timeUpgrades > 5) {
                timeUpgrades = 5;
            } else if (timeUpgrades < 0) {
                timeUpgrades = 0;
            }
        }

        private void BargainingRest() {
            if (valueUpgrades > 5) {
                valueUpgrades = 5;
            } else if (valueUpgrades < 0) {
                valueUpgrades = 0;
            }
        }

        private void LuckRest() {
            if (luckUpgrades > 5) {
                luckUpgrades = 5;
            } else if (luckUpgrades < 0) {
                luckUpgrades = 0;
            }
        }

        private void StrengthRest() {
            if (strengthUpgrades > 5) {
                strengthUpgrades = 5;
            } else if (strengthUpgrades < 0) {
                strengthUpgrades = 0;
            }
        }

        private void SpraysRest() {
            if (spraysUpgrades > 3) {
                spraysUpgrades = 3;
            } else if (spraysUpgrades < 0) {
                spraysUpgrades = 0;
            }
        }

        #endregion

        #region Statistics

        public void LoadStats() {
            LoadTimeUpgrades();
            LoadBargainingUpgrades();
            LoadLuckUpgrades();
            LoadStrengthUpgrades();
            LoadSpraysUpgrades();
        }

        private void LoadTimeUpgrades() {
            timeUpgrades = PlayerPrefs.GetInt("timeUpgrades");
        }

        private void LoadBargainingUpgrades() {
            valueUpgrades = PlayerPrefs.GetInt("valueUpgrades");
        }

        private void LoadLuckUpgrades() {
            luckUpgrades = PlayerPrefs.GetInt("luckUpgrades");
        }

        private void LoadStrengthUpgrades() {
            strengthUpgrades = PlayerPrefs.GetInt("strengthUpgrades");
        }

        private void LoadSpraysUpgrades() {
            spraysUpgrades = PlayerPrefs.GetInt("spraysUpgrades");
        }

        public void SetStats() {
            SwitchTimeUpgrades();
            SwitchBargainingUpgrades();
            SwitchLuckUpgrades();
            SwitchStrengthUpgrades();
            SwitchSpraysUpgrades();
        }

        private float GetStageTime() {
            if(PlayerPrefs.GetInt("SceneUpgrade") == 1) return 20f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 2) return 30f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 3) return 45f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 4) return 68f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 5) return 103f;
            return 1f;
        }

        private float GetStrengthStageMultip() {
            if(PlayerPrefs.GetInt("SceneUpgrade") == 1) return 0.5f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 2) return 1f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 3) return 1.25f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 4) return 4f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 5) return 8f;
            return 1f;
        }

        private float GetLuckStageAddition() {
            if(PlayerPrefs.GetInt("SceneUpgrade") == 1) return 0f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 2) return 0.1f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 3) return 0.5f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 4) return 1f;
            if(PlayerPrefs.GetInt("SceneUpgrade") == 5) return 1.25f;
            return 1f;
        }

        private void SwitchTimeUpgrades() {
            switch (timeUpgrades) {
                case 0:
                    upTime = (float)Math.Round(1f * GetStageTime(), 0, MidpointRounding.AwayFromZero);
                    break;
                case 1:
                    upTime = (float)Math.Round(1.1f * GetStageTime(), 0, MidpointRounding.AwayFromZero);
                    break;
                case 2:
                    upTime = (float)Math.Round(1.2f * GetStageTime(), 0, MidpointRounding.AwayFromZero);
                    break;
                case 3:
                    upTime = (float)Math.Round(1.3f * GetStageTime(), 0, MidpointRounding.AwayFromZero);
                    break;
                case 4:
                    upTime = (float)Math.Round(1.4f * GetStageTime(), 0, MidpointRounding.AwayFromZero);
                    break;
                case 5:
                    upTime = (float)Math.Round(1.5f * GetStageTime(), 0, MidpointRounding.AwayFromZero);
                    break;
            }
        }

        private void SwitchBargainingUpgrades() {
            switch (valueUpgrades) {
                case 0:
                    valueMultiplier = 1 * GetStageTime();
                    break;
                case 1:
                    valueMultiplier = 3 * GetStageTime();
                    break;
                case 2:
                    valueMultiplier = 6 * GetStageTime();
                    break;
                case 3:
                    valueMultiplier = 9 * GetStageTime();
                    break;
                case 4:
                    valueMultiplier = 12 * GetStageTime();
                    break;
                case 5:
                    valueMultiplier = 15 * GetStageTime();
                    break;
            }
        }

        private void SwitchLuckUpgrades() {
            switch (luckUpgrades) {
                case 0:
                    spawnOffsetChance = 0f + GetLuckStageAddition();
                    break;
                case 1:
                    spawnOffsetChance = 0.1f + GetLuckStageAddition();
                    break;
                case 2:
                    spawnOffsetChance = 0.25f + GetLuckStageAddition();
                    break;
                case 3:
                    spawnOffsetChance = 0.5f + GetLuckStageAddition();
                    break;
                case 4:
                    spawnOffsetChance = 0.75f + GetLuckStageAddition();
                    break;
                case 5:
                    spawnOffsetChance = 1f + GetLuckStageAddition();
                    break;
            }
        }

        private void SwitchStrengthUpgrades() {
            switch (strengthUpgrades) {
                case 0:
                    pullSpeedMultiplier = 1f * GetStrengthStageMultip();
                    break;
                case 1:
                    pullSpeedMultiplier = 1.25f * GetStrengthStageMultip();
                    break;
                case 2:
                    pullSpeedMultiplier = 1.5f * GetStrengthStageMultip();
                    break;
                case 3:
                    pullSpeedMultiplier = 1.75f * GetStrengthStageMultip();
                    break;
                case 4:
                    pullSpeedMultiplier = 2.5f * GetStrengthStageMultip();
                    break;
                case 5:
                    pullSpeedMultiplier = 3.5f * GetStrengthStageMultip();
                    break;
            }
        }

        private void SwitchSpraysUpgrades() {
            switch (spraysUpgrades) {
                case 0:
                    sprays = 0;
                    break;
                case 1:
                    sprays = 1;
                    break;
                case 2:
                    sprays = 2;
                    break;
                case 3:
                    sprays = 3;
                    break;
            }
        }

        #endregion

        #region Singleton 

        private void SingletonSetup() {         //If there is no other instance of this object set the instance to this one else destroy this game object
            if (GameMasterInstance == null) {   //the result is that there is always only one instance of this script and it keeps its values whan changing
                GameMasterInstance = this;      //scenes.
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}