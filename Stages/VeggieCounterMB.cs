using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Code.Stages.Player;

namespace Assets.Code.Stages.Collectible
{
    public class VeggieCounterMB : MonoBehaviour
    {

        public PlayerMB player;
        
        public int VegetablesCount()
        {
            int i = 0;
            foreach(CollectibleMB collectible in player.spawnedObjects)
            {
                if (!WormCheck(collectible)) i++;
            }
            return i;
        }

        public int WormsCount()
        {
            int i = 0;
            foreach(CollectibleMB collectible in player.spawnedObjects)
            {
                if (WormCheck(collectible)) i++;
            }
            return i;
        }

        public int GoodVegetablesCount()
        {
            int i = 0;
            foreach(CollectibleMB collectible in player.spawnedObjects)
            {
                if (!WormCheck(collectible) && !collectible.CheckDealsDamage()) i++;
            }
            return i;
        }

        public int BadVegetablesCount()
        {
            int i = 0;
            foreach(CollectibleMB collectible in player.spawnedObjects)
            {
                if (!WormCheck(collectible) && collectible.CheckDealsDamage()) i++;
            }
            return i;
        }
        public List<CollectibleSO> GetGoodVegetablesList()
        {
            List<CollectibleSO> returningList = new List<CollectibleSO>();
            foreach (CollectibleMB collectible in player.spawnedObjects)
            {
                if (!WormCheck(collectible) && !collectible.CheckDealsDamage()) returningList.Add(collectible.drawnCollectibleSO);
            }
            return returningList;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) print(VegetablesCount());
            if (Input.GetKeyDown(KeyCode.Alpha2)) print(WormsCount());
            if (Input.GetKeyDown(KeyCode.Alpha3)) print(GoodVegetablesCount());
            if (Input.GetKeyDown(KeyCode.Alpha4)) print(BadVegetablesCount());
        }

        private bool WormCheck(CollectibleMB collectible)
        {
            return collectible.vegieType == CollectibleMB.VegieType.C_WORM || collectible.vegieType == CollectibleMB.VegieType.M_WORM
                                || collectible.vegieType == CollectibleMB.VegieType.WORM;
        }
    }
}
