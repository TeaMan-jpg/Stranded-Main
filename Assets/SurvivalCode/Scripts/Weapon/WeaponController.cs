using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformers {

    public class WeaponController:MonoBehaviour
    {

        public GameObject Sword;
        public bool CanAttack = true;
        public float AttackCooldown = 1f;
        public bool isAttacking = false;

        public InputActionAsset weaponActions;
        public StaminaManagerActual staminaManager;
        public Camera shakeCamera; // Assigned from 'mains' in your previous script
        private CharacterStatss characterStats;

        private InputAction attackAction;
        private InputAction defendAction;
        private InputAction mineAction;


        private void OnEnable()
        {
            // FIX: Only enable if the actions actually exist
            attackAction?.Enable();
            defendAction?.Enable();
            mineAction?.Enable();
        }

        private void OnDisable()
        {
            attackAction?.Disable();
            defendAction?.Disable();
            mineAction?.Disable();
        }
        void Update()
        {
            if (attackAction == null) return;

            // Prevent actions if UI is open
            if (GameManager.isInventoryOpen || GameManager.IsShopOpen || GameManager.enables) return;
            //if (player.GetComponent<FirstPersonController>().isPauseOpen || player.GetComponent<FirstPersonController>().isPauseOptionsOpen) return;

            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (CanAttack)
            //    {
            //        SwordAttack();


            //    }
            //}
        }
        public void Awake()
        {
            if (weaponActions != null)
            {
                var playerActionMap = weaponActions.FindActionMap("Player");
                if (playerActionMap != null)
                {
                    attackAction = playerActionMap.FindAction("Attack");
                    defendAction = playerActionMap.FindAction("Defend");
                    mineAction = playerActionMap.FindAction("Mine");
                }
            }
            else
            {
                Debug.LogWarning($"WeaponActions not assigned on {gameObject.name}. Input will be disabled.");
            }
        }
        public void SwordAttack()
        {
            Debug.Log(isAttacking);
            CanAttack = false;
            isAttacking = true;
            Animator anim = Sword.GetComponent<Animator>();
            anim.SetTrigger("Attack");
            StartCoroutine(ResetAttackCooldown());
        }
        private IEnumerator ResetAttackCooldown()
        {
            StartCoroutine(ResetAttackBool());
            yield return new WaitForSeconds(AttackCooldown);
            CanAttack = true;
        }
        IEnumerator ResetAttackBool()
        {
            yield return new WaitForSeconds(AttackCooldown);
            isAttacking = false;

        }

    }


}
//namespace Platformers
//{
//    public class WeaponController : MonoBehaviour
//    {
//        public GameObject Sword; 

//        // --- Action Flags ---
//        public bool CanAttack = true;
//        public bool CanDefend = true;
//        public bool CanMine = true; 

//        public Slider staminaSlider;

//        public Camera mains;

//        // --- Cooldowns ---
//        public float AttackCooldown = 1f;
//        public float MineCooldown = 1.2f; 

//        // --- Input Actions ---
//        public InputActionAsset weaponActions;
//        private InputAction attackAction;
//        private InputAction defendAction;
//        private InputAction mineAction;

//        public StaminaManagerActual StaminaManager;

//        [Header("Slow Motion Settings")]
//        [Range(0.1f, 1f)]
//        public float slowMotionTimeScale = 0.2f; // 0.2 means the game runs at 20% speed
//        private float defaultFixedDeltaTime; // To fix physics jitter

//        [SerializeField] private Camera mainCamera;
//        [SerializeField] private GameObject attackCamera;
//        [SerializeField] private float attackCamDuration = 0.8f; // How long the camera stays switched
//        // --- State Booleans ---
//        public bool isAttacking = false;
//        public bool isDefending = false;
//        public bool isMining = false; 

//        public void Awake()
//        {
//            // Find all the actions from the Input Action Asset
//            var playerActionMap = weaponActions.FindActionMap("Player");
//            attackAction = playerActionMap.FindAction("Attack");
//            defendAction = playerActionMap.FindAction("Defend");
//            mineAction = playerActionMap.FindAction("Mine");
//            defaultFixedDeltaTime = Time.fixedDeltaTime;
//        }

//        // Enable and Disable Input Actions
//        private void OnEnable()
//        {
//            attackAction.Enable();
//            defendAction.Enable();
//            mineAction.Enable(); // NEW: Enable the mine action
//        }

//        private void OnDisable()
//        {
//            attackAction.Disable();
//            defendAction.Disable();
//            mineAction.Disable(); // NEW: Disable the mine action
//        }

//        // attacks and defends based on player input and bools for cooldowns and triggered states
//        private void Update()
//        {

//            if (attackAction.triggered && CanAttack)
//            {
//                SwordAttack();

//            }

//            if (defendAction.triggered && CanDefend)
//            {
//                SwordDefend();
//            }


//            if (mineAction.triggered && CanMine)
//            {
//                MineAction();
//            }

//        }

//        // --- Sword Attack Logic ---
//        private void SwordAttack()
//        {
//            if (GameManager.isInventoryOpen || GameManager.IsShopOpen) return;
//            if (GameManager.enables) return;
//            GameObject player = GameObject.FindWithTag("FPSController");
//            //if (player.GetComponent<FirstPersonController>().isPauseOpen || player.GetComponent<FirstPersonController>().isPauseOptionsOpen) return;


//            if (CanAttack)
//            {
//                isAttacking = true;
//                CanAttack = false;


//                //if (StaminaManager.currentStamina < 50f)
//                //{
//                //    Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));

//                //    Popup.instance.CreatePopUp(transform.position + randomness, "Not enough stamina for attack!", Color.red);

//                //    Debug.Log("Not enough stamina to attack!");
//                //    isAttacking = false;
//                //    CanAttack = true;
//                //    return;
//                //}


//                //StaminaManager.currentStamina -= 50f; 

//                Debug.Log("Current Stamina after attack: " + StaminaManager.currentStamina);

//                Animator anim = GetComponentInChildren<Animator>();

//                Debug.Log("Animator found: " + (anim != null));
//                anim.SetTrigger("Attack");
//                Debug.Log("5");

//                StartCoroutine(ResetAttack());
//            }
//        }

//        // Resets the ability to attack after cooldown
//        private IEnumerator ResetAttack()
//        {
//            StartCoroutine(ResetAttackBool());
//            ; // Decrease stamina by 10 on attack
//            yield return new WaitForSeconds(AttackCooldown);
//            CanAttack = true;
//        }

//        // Resets the isAttacking flag after the attack animation is likely finished
//        IEnumerator ResetAttackBool()
//        {


//            //mainCamera.gameObject.SetActive(false);
//            //if (attackCamera != null) attackCamera.SetActive(true);

//            // 2. Play Animation (Trigger logic)
//            // You can alternate animations here if you want
//            Animator anim = GetComponentInChildren<Animator>();


//            if (anim != null)
//            {
//                anim.SetTrigger("Attack");
//                Debug.Log("3");
//            }

//            if (mains != null)
//            {
//                mains.GetComponent<Shake>().start = true;
//            }
//            //Time.timeScale = slowMotionTimeScale;
//            //// Adjust physics so movement stays smooth even in slow mo
//            //Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

//            // 3. Wait for the attack to finish
//            yield return new WaitForSecondsRealtime(attackCamDuration);

//            //Time.timeScale = 1f;
//            //Time.fixedDeltaTime = defaultFixedDeltaTime;

//            // 4. Switch back to Main Camera
//            //if (attackCamera != null) attackCamera.SetActive(false);
//            //mainCamera.gameObject.SetActive(true);
//            yield return new WaitForSeconds(AttackCooldown);
//            isAttacking = false;

//            if (mains != null)
//            {
//                mains.GetComponent<Shake>().start = false;
//            }
//        }



//        // --- Sword Defend Logic ---
//        private void SwordDefend()
//        {
//            if (GameManager.isInventoryOpen || GameManager.IsShopOpen) return;
//            if (GameManager.enables) return;

//            if (CanDefend)
//            {

//                //isDefending = true;
//                //CanDefend = false;

//                //if (StaminaManager.currentStamina < 50f)
//                //{
//                //    Debug.Log("Not enough stamina to defend!");
//                //    Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));

//                //    Popup.instance.CreatePopUp(transform.position + randomness, "Not enough stamina to defend!", Color.red);

//                //    isDefending = false;
//                //    CanDefend = true;
//                //    return;
//                //}


//                //StaminaManager.currentStamina -= 50f;

//                Animator anim = GetComponentInChildren<Animator>();

//                anim.SetTrigger("Defend");
//                StartCoroutine(ResetDefend());
//            }
//        }

//        // Resets the ability to defend after cooldown
//        private IEnumerator ResetDefend()
//        {
//            StartCoroutine(ResetDefendBool());
//            yield return new WaitForSeconds(0.5f);

//            CanDefend = true;
//        }

//        // Resets the isDefending flag after the defend animation is likely finished
//        IEnumerator ResetDefendBool()
//        {
//            yield return new WaitForSeconds(0.5f);
//            isDefending = false;
//        }

//        // --- NEW: MINING LOGIC ---
//        private void MineAction()
//        {
//            // These checks prevent mining while a menu is open
//            if (GameManager.isInventoryOpen || GameManager.IsShopOpen) return;
//            if (GameManager.enables) return;


//            //if (StaminaManager.currentStamina < 30f) {

//            //    Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));

//            //    Popup.instance.CreatePopUp(transform.position + randomness, "Not enough stamina to mine!", Color.red);

//            //    isMining = false;
//            //    CanMine = true;
//            //    return;

//            //}


//            //if (CanMine)
//            //{
//            //    StaminaManager.currentStamina -= 30f;
//            //    isMining = true;
//            //    CanMine = false;
//            //    Animator anim = GetComponentInChildren<Animator>();

//            //    anim.SetTrigger("Attack"); 
//            //    Debug.Log("4");
//            //    StartCoroutine(ResetMine());
//            //}
//        }

//        // Resets the ability to mine after cooldown
//        private IEnumerator ResetMine()
//        {
//            StartCoroutine(ResetMineBool());
//            yield return new WaitForSeconds(MineCooldown);
//            CanMine = true;
//        }

//        // Resets the isMining flag after the mining animation is likely finished
//        IEnumerator ResetMineBool()
//        {
//            // This coroutine resets the isMining flag after the animation is likely finished
//            yield return new WaitForSeconds(MineCooldown);
//            isMining = false;
//        }
//    }
//}