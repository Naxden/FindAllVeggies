using UnityEngine;

namespace Assets.Code.Menu {
    public class CheckSetup : MonoBehaviour {
        private void Start() {
            if(PlayerPrefs.GetInt("timesOpened") != 0) {
                PlayerPrefs.SetInt("timesOpened", PlayerPrefs.GetInt("timesOpened") + 1);
            } else if (!PlayerPrefs.HasKey("timesOpened")) {
                PlayerPrefs.SetInt("timeUpgrades", 0);
                PlayerPrefs.SetInt("valueUpgrades", 0);
                PlayerPrefs.SetInt("luckUpgrades", 0);
                PlayerPrefs.SetInt("strengthUpgrades", 0);
                PlayerPrefs.SetInt("spraysUpgrades", 0);
                PlayerPrefs.SetInt("Money", 0);
                PlayerPrefs.SetInt("extendTimePrice", 400);
                PlayerPrefs.SetInt("upgradeBargainingPrice", 1000);
                PlayerPrefs.SetInt("betterLuckPrice", 500);
                PlayerPrefs.SetInt("moreStrengthPrice", 200);
                PlayerPrefs.SetInt("buySpraysPrice", 600);


                PlayerPrefs.SetInt("timesOpened", PlayerPrefs.GetInt("timesOpened") + 1);
            } else {
                PlayerPrefs.SetInt("timeUpgrades", 0);
                PlayerPrefs.SetInt("valueUpgrades", 0);
                PlayerPrefs.SetInt("luckUpgrades", 0);
                PlayerPrefs.SetInt("strengthUpgrades", 0);
                PlayerPrefs.SetInt("spraysUpgrades", 0);
                PlayerPrefs.SetInt("Money", 0);
                PlayerPrefs.SetInt("extendTimePrice", 400);
                PlayerPrefs.SetInt("upgradeBargainingPrice", 1000);
                PlayerPrefs.SetInt("betterLuckPrice", 500);
                PlayerPrefs.SetInt("moreStrengthPrice", 200);
                PlayerPrefs.SetInt("buySpraysPrice", 600);


                PlayerPrefs.SetInt("timesOpened", PlayerPrefs.GetInt("timesOpened") + 1);
            }
        }
    }
}