using Assets.Code.Stages.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Assets.Code.Stages.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Stages.Collectible
{
    public class CollectibleMB : MonoBehaviour
    {
        private CollectibleMB hitCollectible;
        public enum VegieType
        {
            ONIONT1, ONIONT2, ONIONT3,
            BEETROOTT1, BEETROOTT2, BEETROOTT3,
            CARROTT1, CARROTT2, CARROTT3,
            RUDISHT1, RUDISHT2, RUDISHT3,
            TOMATOET1, TOMATOET2, TOMATOET3,
            CABBAGET1, CABBAGET2, CABBAGET3,
            CORNT1, CORNT2, CORNT3,
            EGGPLANTT1, EGGPLANTT2, EGGPLANTT3,
            PUMPKIN,
            CALAREPAT1, CALAREPAT2,
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
            M_WORM,
            WAITER
        }
        public VegieType vegieType;
        [HideInInspector]
        public bool hasGoodCordinates = false;
        
        #region Logic Values

        [Header("LogicData")]
        public bool dealsDamage, isMoving;
        public CollectibleSO drawnCollectibleSO;
        private float pullSpeed = 1f;
        private int value = 1;

        #endregion

        #region Physical Values

        [Header("PhysicalData")]
        public SpriteRenderer spriteRenderer;
        public EdgeCollider2D myCollider2D;
        public LayerMask collisionMask;
        public Animator animator;
        private Collider2D myCollider;

        #endregion

        #region External
        [Header("External")]

        public PlayerMB player;
        public GameObject particle;
        public CollectibleSO[] collectibleSOTable = new CollectibleSO[12];
        public ParticleSystem chewingParticleSystem;
        public GameObject infectedIndicator;

        #endregion

        #region Worm Values

        Vector3 startPatrolPosition, endPatrolPosition;
        bool goRight, isHooked, isWaiting;
        float speed, spawnYPosition, lerpDuration = 4f, timeElapsed;
        #endregion

        private void Start() 
        {
            myCollider = GetComponent<Collider2D>();
            player = GameObject.FindObjectOfType<PlayerMB>();
            
            SelectVegetableFunction();
            
            if (IsWorm() && isMoving) //if it is a worm
            {
                SelectPropperWormValues();
            }

        }
        private void Update()
        {
            if (IsWorm() || !player.canStart) CheckCollision();
            if (player.canStart) WormMovement();
        }

        #region Functions On Start

        private void SelectVegetableFunction()
        {
            SelectVegetable(UnityEngine.Random.Range(0f, 100f)); //TODO apply upgrade offset !
            SelectPropperPhysicalValues();
            SelectPropperLogicValues();
        }

        private void SelectPropperWormValues()
        {
            startPatrolPosition = transform.position - new Vector3(1.5f, 0, 0);
            endPatrolPosition = transform.position + new Vector3(1.5f, 0, 0);
            spawnYPosition = transform.position.y;
            speed = UnityEngine.Random.Range(0.5f, 1f);
            float randomizer = UnityEngine.Random.Range(-100, 100);
            if (randomizer <= 0) goRight = false;
            else goRight = true;

            spriteRenderer.sortingOrder = 10;
            
            if (SceneManager.GetActiveScene().buildIndex == 4)
                animator.runtimeAnimatorController = Resources.Load("Animations/wormCosmic") as RuntimeAnimatorController;
            else if (SceneManager.GetActiveScene().buildIndex == 5)
                animator.runtimeAnimatorController = Resources.Load("Animations/wormMagic") as RuntimeAnimatorController;
            else
                animator.runtimeAnimatorController = Resources.Load("Animations/worm1") as RuntimeAnimatorController;
        }
        private void SelectPropperLogicValues()
        {
            pullSpeed = drawnCollectibleSO.pullSpeed;
            value = drawnCollectibleSO.value;
            dealsDamage = drawnCollectibleSO.dealsDamage;
            isMoving = drawnCollectibleSO.isMoving;
            vegieType = (VegieType)drawnCollectibleSO.vegieType;
        }
        private void SelectPropperPhysicalValues()
        {
            transform.name += drawnCollectibleSO.name;
            spriteRenderer.sprite = drawnCollectibleSO.sprite;

            myCollider2D.points = drawnCollectibleSO.colliderPoints;
        }
        #endregion

        #region In Game Behaviour
        public void StickToHook(GameObject parent)
        {
            Instantiate(particle, transform.position, quaternion.identity);
            GetComponent<AudioSource>().Play();
            isHooked = true;
            myCollider2D.enabled = false;
            transform.parent = parent.transform;
            transform.rotation = parent.transform.rotation;
            transform.localPosition = new Vector3(0, 0, 0);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Infection")) animator.SetTrigger("interrupt");
            else animator.SetTrigger("pick");
        }

        private void CheckCollision() {
            Collider2D[] hits = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, collisionMask);
            foreach (Collider2D hit in hits) {
                if (!hasGoodCordinates) { //selecting proper coordinates
                    if (hits.Length == 1) {
                        hasGoodCordinates = true;
                        spawnYPosition = transform.position.y;
                    } else {
                        hasGoodCordinates = false;
                        ChangePosition();
                    }
                }
                if (player.canStart) try { hitCollectible = hit.GetComponent<CollectibleMB>(); } catch { } //infection of collectible
                if (hits.Length > 1 && IsWorm() && player.canStart && hit.gameObject.tag != "Player")
                {
                    if (hitCollectible.name != this.name
                        && !hitCollectible.dealsDamage
                        && !hitCollectible.isMoving
                        && CanBeInfected(hitCollectible.vegieType)
                        && !CurrentAnimation(hitCollectible.animator, "Infection"))
                    {
                        StartInfectedAnimation(hitCollectible.animator);
                        StartCoroutine(FreezeWormRoutine(hitCollectible));
                    }
                }
            }
        }

        public void ChangePosition()
        {
            float minX, maxX, minY, maxY, offsetMinX, offsetMaxX, offsetMinY, offsetMaxY, camDistance;

            camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);

            Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
            Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

            minX = bottomCorner.x;
            maxX = topCorner.x;
            minY = bottomCorner.y;
            maxY = topCorner.y;

            offsetMinX = (minX / 100) * -15;
            offsetMaxX = (maxX / 100) * -15;
            offsetMinY = (minY / 100) * -5;
            offsetMaxY = (maxY / 100) * -55;

            transform.position = new Vector3(UnityEngine.Random.Range(minX + offsetMinX, maxX + offsetMaxX), UnityEngine.Random.Range(minY + offsetMinY, maxY + offsetMaxY), 0f);
        }

        public IEnumerator FreezeWormRoutine(CollectibleMB collectible)
        {
            PauseWorm(collectible);
            if (CurrentAnimation(collectible.animator, "Static"))
            {
                yield return new WaitUntil(() => !CurrentAnimation(collectible.animator, "Static"));
            }
            yield return new WaitWhile(() => CurrentAnimation(collectible.animator, "Infection"));
            
            ResumeWorm();
        }

        private void PauseWorm(CollectibleMB collectible)
        {
            myCollider2D.enabled = false;
            isMoving = false;
            spriteRenderer.enabled = false;
            transform.position = collectible.transform.position;
        }

        private void ResumeWorm()
        {
            myCollider2D.enabled = true;
            isMoving = true;
            spriteRenderer.enabled = true;
        }

        private void WormMovement()
        {
            if (isMoving && !isHooked)
            {
                if(transform.position.y != spawnYPosition)
                {
                    if(timeElapsed < lerpDuration)
                    {
                        Vector3 pos1 = transform.position, pos2 = new Vector3(transform.position.x, spawnYPosition, 0f);
                        transform.position = Vector3.Lerp(pos1, pos2, timeElapsed / lerpDuration);
                        timeElapsed += Time.deltaTime;
                    }
                    else
                    {
                        timeElapsed = 0f;
                    }
                }
                else
                {
                    timeElapsed = 0;
                    if (goRight && !isWaiting)
                    {
                        transform.Translate(Vector3.right * Time.deltaTime * speed);
                        transform.rotation = Quaternion.Euler(Vector3.zero);
                        if (transform.position.x >= endPatrolPosition.x) StartCoroutine(WaitMarchingCorourinte());
                    } else if (!goRight && !isWaiting)
                    {
                        transform.Translate(Vector3.right * Time.deltaTime * speed);
                        transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                        if (transform.position.x <= startPatrolPosition.x) StartCoroutine(WaitMarchingCorourinte());
                    }
                }
            }
        }

        private IEnumerator WaitMarchingCorourinte()
        {
            isWaiting = true;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 1f));
            speed = UnityEngine.Random.Range(0.5f, 1f);
            goRight = !goRight;
            isWaiting = false;
        }

        public void StartInfectedAnimation(Animator collectibleAnimator)
        {
            collectibleAnimator.SetTrigger("infect");
        }

        public void Infection() //CALLED FROM ANIMATION INFECTION
        {
            dealsDamage = true;
            infectedIndicator.SetActive(true);
            TasksMB tasks = GameObject.FindObjectOfType<TasksMB>();
            tasks.RemoveFromList(vegieType);
        }

        public void ControlParticles(int isPlaying) //Is called by animation. It is INT bcs bool is somehow problem for animation
        {
            if (isPlaying == 0)
            {
                var ma = chewingParticleSystem.main; // (╯°□°）╯︵ ┻━┻
                ma.startColor = drawnCollectibleSO.tint; // bezsens ale innaczej sie pluje ze "przestarzałe" ¯\_(ツ)_/¯ 
                chewingParticleSystem.Play();
            } else
            {
                chewingParticleSystem.Stop();
            }
        }
        #endregion

        #region Selecting Random Collectible
        private CollectibleSO DecideForS1(float n) {
            if(n >= 99.5f) return collectibleSOTable[14]; //potato
            else if(n >= 96f) return collectibleSOTable[13]; //callarepa t1
            else if(n >= 91f) return collectibleSOTable[15]; //worm
            else if(n >= 86f) return collectibleSOTable[12]; //eggplant t1
            else if(n >= 81f) return collectibleSOTable[9]; //tomatoe t2
            else if(n >= 76f) return collectibleSOTable[7]; //ruddish t2
            else if(n >= 71f) return collectibleSOTable[5]; //carrot t2
            else if(n >= 66f) return collectibleSOTable[3]; //beetroot t2
            else if(n >= 58.8f) return collectibleSOTable[10]; //cabbage t1
            else if(n >= 51.6f) return collectibleSOTable[8]; //tomatoe t1
            else if(n >= 44.4f) return collectibleSOTable[6]; //ruddish t1
            else if(n >= 37.2f) return collectibleSOTable[4]; //carrot t1
            else if(n >= 30f) return collectibleSOTable[1]; //onion t2
            else if(n >= 20f) return collectibleSOTable[2]; //beetroot t1
            else if(n >= 10f) return collectibleSOTable[11]; //corn t1
            else if(n >= 0f) return collectibleSOTable[0]; //onion t1

            return collectibleSOTable[0]; //onion t1
        }

        private CollectibleSO DecideForS2(float n) {
            if(n >= 99.5f) return collectibleSOTable[22]; //potato
            else if(n >= 98f) return collectibleSOTable[19]; //pumpkin
            else if(n >= 96.5f) return collectibleSOTable[21]; //calarepa t2
            else if(n >= 93f) return collectibleSOTable[18]; //eggplant t2
            else if(n >= 89.5f) return collectibleSOTable[20]; //calarepa t1
            else if(n >= 85.5f) return collectibleSOTable[23]; //worm
            else if(n >= 82f) return collectibleSOTable[5]; //beetroot t3
            else if(n >= 78.5f) return collectibleSOTable[8]; //carrot t3
            else if(n >= 74.5f) return collectibleSOTable[17]; //eggplant t1
            else if(n >= 70.5f) return collectibleSOTable[16]; //corn t2
            else if(n >= 66.5f) return collectibleSOTable[14]; //cabbage t2
            else if(n >= 62.5f) return collectibleSOTable[12]; //tomatoe t2
            else if(n >= 58.5f) return collectibleSOTable[10]; //rudish t2
            else if(n >= 54.5f) return collectibleSOTable[7]; //carrot t2
            else if(n >= 50.5f) return collectibleSOTable[4]; //beetroot t2
            else if(n >= 46.5f) return collectibleSOTable[2]; //onion t3
            else if(n >= 41.1f) return collectibleSOTable[13]; //cabbage t1
            else if(n >= 35.7f) return collectibleSOTable[11]; //tomatoe t1
            else if(n >= 30.3f) return collectibleSOTable[9]; //ruddish t1
            else if(n >= 24.9f) return collectibleSOTable[6]; //carrot t1
            else if(n >= 19.5f) return collectibleSOTable[1]; //onion t2
            else if(n >= 13f) return collectibleSOTable[3]; //beetroot t1
            else if(n >= 6.5f) return collectibleSOTable[15]; //corn t1
            else if(n >= 0f) return collectibleSOTable[0]; //onion t1
            
            return collectibleSOTable[0]; //onion t1
        }

        private CollectibleSO DecideForS3(float n) {
            if(n >= 99.5f) return collectibleSOTable[27]; //potato
            else if(n >= 98.25f) return collectibleSOTable[26]; //calarepa t2
            else if(n >= 97f) return collectibleSOTable[24]; //pumpkin
            else if(n >= 95.75f) return collectibleSOTable[23]; //eggplant t3
            else if(n >= 94.5f) return collectibleSOTable[17]; //cabbage t3
            else if(n >= 91.9375f) return collectibleSOTable[25]; //calarepa t1
            else if(n >= 89.375f) return collectibleSOTable[22]; //eggplant t2
            else if(n >= 86.8125f) return collectibleSOTable[14]; //tomatoe t3
            else if(n >= 84.25f) return collectibleSOTable[28]; //worm
            else if(n >= 81.6875f) return collectibleSOTable[20]; //corn t3
            else if(n >= 79.125f) return collectibleSOTable[5]; //beetroot t3
            else if(n >= 76.5625f) return collectibleSOTable[8]; //carrot t3
            else if(n >= 74f) return collectibleSOTable[11]; //ruddish t3
            else if(n >= 70f) return collectibleSOTable[21]; //eggplant t1
            else if(n >= 66f) return collectibleSOTable[19]; //corn t2
            else if(n >= 62f) return collectibleSOTable[16]; //cabbage t2
            else if(n >= 58f) return collectibleSOTable[13]; //tomatoe t2
            else if(n >= 54f) return collectibleSOTable[10]; //rudish t2
            else if(n >= 50f) return collectibleSOTable[7]; //carrot t2
            else if(n >= 46f) return collectibleSOTable[4]; //beetroot t2
            else if(n >= 42f) return collectibleSOTable[2]; //onion t3
            else if(n >= 37.2f) return collectibleSOTable[15]; //cabbage t1
            else if(n >= 32.4f) return collectibleSOTable[12]; //tomatoe t1
            else if(n >= 27.6f) return collectibleSOTable[9]; //ruddish t1
            else if(n >= 22.8f) return collectibleSOTable[6]; //carrot t1
            else if(n >= 18f) return collectibleSOTable[1]; //onion t2
            else if(n >= 12f) return collectibleSOTable[3]; //beetroot t1
            else if(n >= 6f) return collectibleSOTable[18]; //corn t1
            else if(n >= 0f) return collectibleSOTable[0]; //onion t1

            return collectibleSOTable[0]; //onion t1
        }

        private CollectibleSO DecideForS4(float n) {
            if(n >= 99.5f) return collectibleSOTable[27]; //c_potato
            else if(n >= 98.25f) return collectibleSOTable[26]; //c_calarepa t2
            else if(n >= 97f) return collectibleSOTable[24]; //c_pumpkin
            else if(n >= 95.75f) return collectibleSOTable[23]; //c_eggplant t3
            else if(n >= 94.5f) return collectibleSOTable[17]; //c_cabbage t3
            else if(n >= 91.9375f) return collectibleSOTable[25]; //c_calarepa t1
            else if(n >= 89.375f) return collectibleSOTable[22]; //c_eggplant t2
            else if(n >= 86.8125f) return collectibleSOTable[14]; //c_tomatoe t3
            else if(n >= 84.25f) return collectibleSOTable[28]; //c_worm
            else if(n >= 81.6875f) return collectibleSOTable[20]; //c_corn t3
            else if(n >= 79.125f) return collectibleSOTable[5]; //c_beetroot t3
            else if(n >= 76.5625f) return collectibleSOTable[8]; //c_carrot t3
            else if(n >= 74f) return collectibleSOTable[11]; //c_ruddish t3
            else if(n >= 70f) return collectibleSOTable[21]; //c_eggplant t1
            else if(n >= 66f) return collectibleSOTable[19]; //c_corn t2
            else if(n >= 62f) return collectibleSOTable[16]; //c_cabbage t2
            else if(n >= 58f) return collectibleSOTable[13]; //c_tomatoe t2
            else if(n >= 54f) return collectibleSOTable[10]; //c_rudish t2
            else if(n >= 50f) return collectibleSOTable[7]; //c_carrot t2
            else if(n >= 46f) return collectibleSOTable[4]; //c_beetroot t2
            else if(n >= 42f) return collectibleSOTable[2]; //c_onion t3
            else if(n >= 37.2f) return collectibleSOTable[15]; //c_cabbage t1
            else if(n >= 32.4f) return collectibleSOTable[12]; //c_tomatoe t1
            else if(n >= 27.6f) return collectibleSOTable[9]; //c_ruddish t1
            else if(n >= 22.8f) return collectibleSOTable[6]; //c_carrot t1
            else if(n >= 18f) return collectibleSOTable[1]; //c_onion t2
            else if(n >= 12f) return collectibleSOTable[3]; //c_beetroot t1
            else if(n >= 6f) return collectibleSOTable[18]; //c_corn t1
            else if(n >= 0f) return collectibleSOTable[0]; //c_onion t1

            return collectibleSOTable[0]; //c_onion t1
        }

        private CollectibleSO DecideForS5(float n) {
            if(n >= 99.5f) return collectibleSOTable[27]; //m_potato
            else if(n >= 98.25f) return collectibleSOTable[26]; //m_calarepa t2
            else if(n >= 97f) return collectibleSOTable[24]; //m_pumpkin
            else if(n >= 95.75f) return collectibleSOTable[23]; //m_eggplant t3
            else if(n >= 94.5f) return collectibleSOTable[17]; //m_cabbage t3
            else if(n >= 91.9375f) return collectibleSOTable[25]; //m_calarepa t1
            else if(n >= 89.375f) return collectibleSOTable[22]; //m_eggplant t2
            else if(n >= 86.8125f) return collectibleSOTable[14]; //m_tomatoe t3
            else if(n >= 84.25f) return collectibleSOTable[28]; //m_worm
            else if(n >= 81.6875f) return collectibleSOTable[20]; //m_corn t3
            else if(n >= 79.125f) return collectibleSOTable[5]; //m_beetroot t3
            else if(n >= 76.5625f) return collectibleSOTable[8]; //m_carrot t3
            else if(n >= 74f) return collectibleSOTable[11]; //m_ruddish t3
            else if(n >= 70f) return collectibleSOTable[21]; //m_eggplant t1
            else if(n >= 66f) return collectibleSOTable[19]; //m_corn t2
            else if(n >= 62f) return collectibleSOTable[16]; //m_cabbage t2
            else if(n >= 58f) return collectibleSOTable[13]; //m_tomatoe t2
            else if(n >= 54f) return collectibleSOTable[10]; //m_rudish t2
            else if(n >= 50f) return collectibleSOTable[7]; //m_carrot t2
            else if(n >= 46f) return collectibleSOTable[4]; //m_beetroot t2
            else if(n >= 42f) return collectibleSOTable[2]; //m_onion t3
            else if(n >= 37.2f) return collectibleSOTable[15]; //m_cabbage t1
            else if(n >= 32.4f) return collectibleSOTable[12]; //m_tomatoe t1
            else if(n >= 27.6f) return collectibleSOTable[9]; //m_ruddish t1
            else if(n >= 22.8f) return collectibleSOTable[6]; //m_carrot t1
            else if(n >= 18f) return collectibleSOTable[1]; //m_onion t2
            else if(n >= 12f) return collectibleSOTable[3]; //m_beetroot t1
            else if(n >= 6f) return collectibleSOTable[18]; //m_corn t1
            else if(n >= 0f) return collectibleSOTable[0]; //m_onion t1

            return collectibleSOTable[0]; //m_onion t1
        }

        private void SelectVegetable(float n) {
            n = Mathf.Clamp(n + PersistentGameMasterMB.GameMasterInstance.spawnOffsetChance, 0.0f, 100.0f);

            if(SceneManager.GetActiveScene().buildIndex == 1) drawnCollectibleSO = DecideForS1(n);
            else if(SceneManager.GetActiveScene().buildIndex == 2) drawnCollectibleSO = DecideForS2(n);
            else if(SceneManager.GetActiveScene().buildIndex == 3) drawnCollectibleSO = DecideForS3(n);
            else if(SceneManager.GetActiveScene().buildIndex == 4) drawnCollectibleSO = DecideForS4(n);
            else if(SceneManager.GetActiveScene().buildIndex == 5) drawnCollectibleSO = DecideForS5(n);
        }

        #endregion

        #region Returning functions
        public float GetPullSpeed()
        {
            return pullSpeed;
        }

        public int GetValue()
        {
            return value;
        }

        public bool CheckDealsDamage()
        {
            return dealsDamage;
        }

        public bool CheckIsMoving()
        {
            return isMoving;
        }

        public bool CurrentAnimation(Animator collectibleAnimator, string animationName)
        {
            return collectibleAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
        }

        private bool CanBeInfected(CollectibleMB.VegieType vegieType)
        {
            return (vegieType != VegieType.POTATO && vegieType != VegieType.C_POTATO && vegieType != VegieType.M_POTATO);
        }

        private bool IsWorm()
        {
            return (vegieType == VegieType.WORM || vegieType == VegieType.C_WORM || vegieType == VegieType.M_WORM);
        }
        #endregion

    }
}
