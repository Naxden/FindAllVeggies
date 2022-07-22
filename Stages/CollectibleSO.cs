using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Code.Stages.Collectible
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "Collectible/Make New Collectible", order = 0)]
    public class CollectibleSO : ScriptableObject
    {
        public enum VegieType
        {
            ONIONT1,ONIONT2,ONIONT3,
            BEETROOTT1,BEETROOTT2,BEETROOTT3,
            CARROTT1,CARROTT2,CARROTT3,
            RUDISHT1,RUDISHT2,RUDISHT3,
            TOMATOET1,TOMATOET2,TOMATOET3,
            CABBAGET1,CABBAGET2,CABBAGET3,
            CORNT1,CORNT2,CORNT3,
            EGGPLANTT1,EGGPLANTT2,EGGPLANTT3,
            PUMPKIN,
            CALAREPAT1,CALAREPAT2,
            POTATO,
            WORM,
            C_ONIONT1, C_ONIONT2, C_ONIONT3,
            C_BEETROOTT1, C_BEETROOTT2, C_BEETROOTT3,
            C_CARROTT1, C_CARROTT2, C_CARROTT3,
            C_RUDISHT1, C_RUDISHT2, C_RUDISHT3,
            C_TOMATOET1, C_TOMATOET2, C_TOMATOET3,
            C_CABBAGET1, C_CABBAGET2, C_CABBAGET3,
            C_CORNT1, C_CORNT2, C_CORNT3,
            C_EGGPLANTT1, C_EGGPLANTT2, C_EGGPLANTT3,
            C_PUMPKIN,
            C_CALAREPAT1, C_CALAREPAT2,
            C_POTATO,
            C_WORM,
            M_ONIONT1, M_ONIONT2, M_ONIONT3,
            M_BEETROOTT1, M_BEETROOTT2, M_BEETROOTT3,
            M_CARROTT1, M_CARROTT2, M_CARROTT3,
            M_RUDISHT1, M_RUDISHT2, M_RUDISHT3,
            M_TOMATOET1, M_TOMATOET2, M_TOMATOET3,
            M_CABBAGET1, M_CABBAGET2, M_CABBAGET3,
            M_CORNT1, M_CORNT2, M_CORNT3,
            M_EGGPLANTT1, M_EGGPLANTT2, M_EGGPLANTT3,
            M_PUMPKIN,
            M_CALAREPAT1, M_CALAREPAT2,
            M_POTATO,
            M_WORM
        }

        [Header("Logic Data")]
        public float pullSpeed = 1f;
        public int value = 1;
        public VegieType vegieType;
        public bool dealsDamage = false;
        public bool isMoving = false;
        
        [Header("Physicial Data")]
        public Sprite sprite = null;
        public Vector2[] colliderPoints;
        public Color tint = Color.white;
    }
}
