using TMPro;
using UnityEngine;
using Assets.Code.Stages.Collectible;
using Assets.Code.Stages.Timer;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using Assets.Code.Stages.Tasks;
using UnityEngine.EventSystems;

namespace Assets.Code.Stages.Player {
    public class PlayerMB : MonoBehaviour {
        #region Rotator variables

        [Header("Rotator")]
        public float period = 1f; //higher value = slower
        public Vector3 movementVector; //eg. 140 on z axis means -70, +70

        [Range(-.5f, .5f)] public float movementFactor;

        private Vector3 startingRotation;
        private float rotationTime = 0f;
        
        #endregion

        #region MoveHook variables

        [Header("SendHook")]
        [Range(0, 100)] public float sendHookSpeed;
        [Range(0, 100)] public float pullHookSpeed;
        public SpriteRenderer hookSprite;

        private float pullHookDefaultSpeed;

        public bool isHookOut = false;

        public Vector3 startingPos;

        #endregion

        #region CheckWorldBorders variables

        Camera cam;

        #endregion

        #region HookObjectInteraction variables

        public TasksMB taskMB;

        UnityEvent catchEvent;

        CollectibleMB.VegieType catchedVeggie;

        private bool isPullingObject = false;

        private int scoreToAdd;

        #endregion

        #region Score variables

        [Header("Score")]
        public int score;
        public Animator changeScoreAnimator;
        public TextMeshProUGUI scoreDisplay;
        public TextMeshProUGUI changeScoreDisplayl;

        #endregion

        #region Sprays variables

        [Header("Sprays")]
        public Image[] spraysImages;

        public Animator sprayAnimator;

        public Color activeSprayColor, deactivatedSprayColor;

        private int spraysNumber;

        private bool dealsDamage = false;

        #endregion

        #region Spawner variables

        [Header("Spawner")]
        public GameObject collectiblePrefab;

        public int collectibleCountSpawn = 30;

        public List<CollectibleMB> spawnedObjects = new List<CollectibleMB>();

        private float minX, maxX, minY, maxY, offsetMinX, offsetMaxX, offsetMinY, offsetMaxY, camDistance;
        
        Vector2 bottomCorner, topCorner;

        [HideInInspector]
        public bool canStart = false;

        #endregion

        #region Other variables

        [Header("Other")]
        [Space(20)]
        public TimerMB timer;
        public Animator machineAnimator;
        public AudioSource myAudioSource;
        public AudioClip collectGoodClip, collectBadClip, collectBadWithSprayClip, sendHookClip;

        public List<Sprite> collectedObjects = new List<Sprite>();

        #endregion

        private void Start() {
            PersistentGameMasterMB.GameMasterInstance.SetStats(); //Not in its own function becouse I want it to be visable

            GetStartRotation(); //from Rotator

            GetMainCamera(); //from CheckWorldBorders

            SetCatchEvent(); //from HookObjectInteraction

            GetStartPos(); //from MoveHook

            GetSpawnedData(); //from Spawner

            GetHookDefaultSpeed(); //from MoveHook

            Spawn(); //from Spawner

            SetSprays(); //from Helath

        }

        private void OnTriggerEnter2D(Collider2D collision) {
            GetCollision(collision); //from HookObjectInteraction
        }

        private void Update() {
            if(!canStart && collectibleCountSpawn == spawnedObjects.Count) CheckIfCanStart(); //from Spawner

            DoRotation(); //from Rotator

            SendHook(); //from MoveHook

            CheckWorldBorders(); //from CheckWorldBorders

            GetInput(); //from GetInput

            CheckIfObjectWasPulled(); //from HookObjectInteraction

            DisplayScore(); //from Score

            DisplaySprays(); //from Health
        }

        #region Rotator

        private void GetStartRotation() {
            startingRotation = transform.rotation.eulerAngles;
        }

        private float Cycles() {
            float cycles = 0f;
            
            if (period != 0) {
                rotationTime += Time.deltaTime;
                cycles = rotationTime / period;
            }

            return cycles;
        }

        private float RawSinWave() {
            const float tau = Mathf.PI / 2f;
            
            return Mathf.Sin(Cycles() * tau);
        }

        private Vector3 Offset() {
            movementFactor = RawSinWave() / 2f;
            return movementVector * movementFactor;
        }

        private Quaternion FinalRotation() {
            return Quaternion.Euler(Offset() + startingRotation);
        }

        private void DoRotation() {
            if(timer.gameInProgress) {
                if (!isHookOut && transform.position == startingPos) {
                    transform.rotation = FinalRotation();
                }
            }
        }

        #endregion

        #region MoveHook

        private void SendHook() {
            if (timer.gameInProgress) {
                if (isHookOut == true) {
                    transform.Translate(Vector3.down * sendHookSpeed * Time.deltaTime);
                } else if (!isHookOut && transform.position != startingPos && isPullingObject) {
                    PullHook(PersistentGameMasterMB.GameMasterInstance.pullSpeedMultiplier);
                } else {
                    PullHook(3f);
                }
            }            
        }

        private void PullHook(float multiply) {
            transform.position = Vector3.MoveTowards(transform.position, startingPos, pullHookSpeed * multiply * Time.deltaTime);
        }

        private void GetStartPos() {
            startingPos = transform.position;
        }

        private void GetHookDefaultSpeed() {
            pullHookDefaultSpeed = pullHookSpeed;
        }

        #endregion

        #region CheckWorldBorders

        private void GetMainCamera() {
            cam = Camera.main;
        }

        private void CheckWorldBorders() {
            Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0) {
                return;
            } else {
                pullHookSpeed = pullHookDefaultSpeed;

                isHookOut = false;
            }
        }

        #endregion

        #region GetInput

        private void GetInput() {
            if(timer.gameInProgress) {
                if (Input.GetMouseButtonDown(0) && transform.position == startingPos && !EventSystem.current.IsPointerOverGameObject()) {
                    isHookOut = true;
                    machineAnimator.SetTrigger("throw");
                    myAudioSource.PlayOneShot(sendHookClip);
                }
            }
        }

        #endregion

        #region HookObjectInteraction

        //In start
        private void SetCatchEvent() {
            if (catchEvent == null) catchEvent = new UnityEvent();
            catchEvent.AddListener(PassVeggie);
        }

        private void PassVeggie() {
            taskMB.CheckCatchedVeggie(catchedVeggie);
            catchedVeggie = CollectibleMB.VegieType.WAITER;
        }

        private void GetCollision(Collider2D col) {
            if (col.gameObject.CompareTag("collectible") && !isPullingObject) {
                //Flag state as pulling
                isPullingObject = true;

                //Start machine pulling animation
                machineAnimator.SetBool("pull", true);

                CollectibleMB ObjInfo = col.gameObject.GetComponent<CollectibleMB>();

                GetHookInteractedData(ObjInfo);

                HookObjectInteraction(ObjInfo);
            }
        }

        private void GetHookInteractedData(CollectibleMB obj) {
            //Here we take object values such as its pull speed, how much score it is worth etc.
            catchedVeggie = obj.vegieType;

            pullHookSpeed = obj.GetPullSpeed();

            scoreToAdd = obj.GetValue();
            
            dealsDamage = obj.CheckDealsDamage();

            //Hide hook
            hookSprite.enabled = false;
        }

        private void HookObjectInteraction(CollectibleMB obj) {
            obj.StickToHook(gameObject);
                
            spawnedObjects.Remove(obj);
            
            isHookOut = false;
        }

        private void CheckIfObjectWasPulled() {
            if(transform.position == startingPos && isPullingObject) {
                catchEvent.Invoke();

                //Show hook
                hookSprite.enabled = true;

                //Stop machine pulling animation
                machineAnimator.SetBool("pull", false);

                if (dealsDamage && spraysNumber > 0) {
                    TakeSpray(); //from Sprays
                    AddToScore(scoreToAdd); //from Score //scoreToAdd is taken from object the moment hook hit its collider

                    myAudioSource.PlayOneShot(collectBadWithSprayClip);
                } else if (!dealsDamage) {
                    AddToScore(scoreToAdd); //from Score //scoreToAdd is taken from object the moment hook hit its collider

                    myAudioSource.PlayOneShot(collectGoodClip);
                } else if(dealsDamage && spraysNumber == 0) {
                    TakeFromScore(15); //this function takes a % of score and subtract if from score

                    myAudioSource.PlayOneShot(collectBadClip);
                }

                isPullingObject = false;

                //Get collectible that is about to be removed from scene
                GameObject collectibleToRemove = transform.GetChild(1).gameObject;

                //Get collected item sprite and add it to list to later display on end screen
                collectedObjects.Add(collectibleToRemove.GetComponent<SpriteRenderer>().sprite);

                //Remove pulled collectible from scene
                Destroy(collectibleToRemove);
            }
        }

        #endregion

        #region Score

        public void AddToScore(int ammount) {
            if(timer.gameInProgress) {
                StartCoroutine(AddScoreRoutine(ammount));
            } else {
                score += scoreToAdd;
            }
        }

        private IEnumerator AddScoreRoutine(int ammount) {
            int scoreToAdd = (int)(ammount * PersistentGameMasterMB.GameMasterInstance.valueMultiplier);

            changeScoreDisplayl.text = "+" + scoreToAdd.ToString() + "$";
            changeScoreAnimator.SetTrigger("add");

            yield return new WaitForSecondsRealtime(.5f);

            score += scoreToAdd;
        }

        private void TakeFromScore(float percentage) {            
            StartCoroutine(TakeScoreRoutine(percentage));
        }

        private IEnumerator TakeScoreRoutine(float percentage) {
            int scoreToTake = (int)((percentage / 100) * score);

            changeScoreDisplayl.text = "-" + (scoreToTake).ToString() + "$";
            changeScoreAnimator.SetTrigger("take");

            yield return new WaitForSecondsRealtime(.5f);

            score -= scoreToTake;
        }

        private void DisplayScore() {
            scoreDisplay.text = score.ToString() + "$";
        }

        public float GetScore() {
            return score;
        }

        #endregion

        #region Sprays

        private void TakeSpray() {
            spraysNumber--;
            sprayAnimator.SetTrigger("use");
        }

        private void SetSprays() {
            spraysNumber = PersistentGameMasterMB.GameMasterInstance.sprays;
        }

        private void DisplaySprays() {
            for(int x = 0; x < spraysNumber; x++) { //Cycles through all health images with index less than health and sets their color to activeHealthColor
                spraysImages[x].color = activeSprayColor;
            }

            for(int y = spraysNumber; y < spraysImages.Length; y++) { //Cycles through all health images with index greater than health and sets their color to deactivatedHealthColor
                spraysImages[y].color = deactivatedSprayColor;
            }
        }

        #endregion

        #region Spawner

        private void GetSpawnedData() {
            camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);

            bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
            topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

            minX = bottomCorner.x;
            maxX = topCorner.x;
            minY = bottomCorner.y;
            maxY = topCorner.y;

            offsetMinX = (minX / 100) * -15;
            offsetMaxX = (maxX / 100) * -15;
            offsetMinY = (minY / 100) * -5;
            offsetMaxY = (maxY / 100) * -55;
        }

        private void Spawn() {
            for (int i = 0; i < collectibleCountSpawn; i++) {
                Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(minX + offsetMinX, maxX + offsetMaxX), UnityEngine.Random.Range(minY + offsetMinY, maxY + offsetMaxY), 0f);

                CollectibleMB spawned = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity).GetComponent<CollectibleMB>();
                spawned.name = "Collectible nr: " + i + "   ";

                spawnedObjects.Add(spawned);
            }
        }

        private void CheckIfCanStart() {
            int objCount = 0;

            foreach(CollectibleMB obj in spawnedObjects) {
                if(obj.hasGoodCordinates) {
                    objCount++;
                } else {
                    objCount = 0;
                }
            }

            if(objCount == spawnedObjects.Count && spawnedObjects.Count == collectibleCountSpawn) {
                canStart = true;
            } else {
                canStart = false;
            }
        }

        #endregion
    }
}