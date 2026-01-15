using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Platformers
{
    public class SwordCollisionDetector : MonoBehaviour
    {
        [Header("Combat & Mining Stats")]
        public int attackDamage = 10;
        public int miningDamage = 5;
        public float attackCooldown = 1f;
        public float mineCooldown = 1.2f;
        public float attackCamDuration = 0.8f;
        public float AttackCooldown = 1f;
        private AudioManager audioManager;
        public WeaponController weaponController;
        public GameObject Sword;
        [Header("References")]
        public Animator animator;
        public InputActionAsset weaponActions;
        public StaminaManagerActual staminaManager;
        public Camera shakeCamera; // Assigned from 'mains' in your previous script
        private CharacterStatss characterStats;
        public GameObject player;

        [Header("State Flags")]
        public bool isAttacking = false;
        public bool isDefending = false;
        public bool isMining = false;

        private bool canAttack = true;
        private bool canDefend = true;
        private bool canMine = true;

        private InputAction attackAction;
        private InputAction defendAction;
        private InputAction mineAction;

        //public WeaponController weaponController;
        private void Awake()
        {
            //if (animator == null) animator = GetComponent<Animator>();
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            // FIX: Check if weaponActions is assigned before trying to use it
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
            player = GameObject.FindWithTag("FPSController");
        }

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

        private void Update()
        {
            //// FIX: If actions are null (dropped item), don't run the update logic
            if (attackAction == null) return;

            // Prevent actions if UI is open
            if (GameManager.isInventoryOpen || GameManager.IsShopOpen || GameManager.enables) return;
            if (player.GetComponent<FirstPersonController>().isPauseOpen || player.GetComponent<FirstPersonController>().isPauseOptionsOpen) return;

            if (attackAction.triggered)
            {
                if (canAttack)
                    SwordAttack();

                //if (canMine)
                //{
                //    SwordMine();

                //}

            }

                if (defendAction.triggered)
            {
                if (canDefend) {
                    SwordDefend();
                
                }


            }

            //if (mineAction.triggered)
            //{
            //    if (canMine) {
            //        SwordMine();
                
            //    }
            
            
            //}
        }

        private void Start()
        {
            // Find stats from the scene
            GameObject charactersGO = GameObject.Find("Characters");
            if (charactersGO != null)
                characterStats = charactersGO.GetComponent<CharacterStatss>();
        }
        public void SwordAttack()
        {
            if (staminaManager.currentStamina < 5f)
            {
                Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));

                Popup.instance.CreatePopUp(transform.position + randomness, "Not enough stamina for attack!", Color.red);

                Debug.Log("Not enough stamina to attack!");
                isAttacking = false;
                canAttack = true;
                return;
            }
            staminaManager.currentStamina -= 5f;
            Debug.Log(isAttacking);
            canAttack = false;
            isAttacking = true;
            Animator anim = Sword.GetComponent<Animator>();
            anim.SetTrigger("Attack");
            audioManager.PlaySFX("slash");
            StartCoroutine(ResetAttackCooldown());
        }
        private IEnumerator ResetAttackCooldown()
        {
            StartCoroutine(ResetAttackBool());
            yield return new WaitForSeconds(AttackCooldown);
            canAttack = true;
        }
        IEnumerator ResetAttackBool()
        {
            yield return new WaitForSeconds(AttackCooldown);
            isAttacking = false;

        }
        public void SwordDefend()
        {
            if (staminaManager.currentStamina < 2f)
            {
                Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));

                Popup.instance.CreatePopUp(transform.position + randomness, "Not enough stamina for defend!", Color.red);

                Debug.Log("Not enough stamina to defend!");
                isDefending = false;
                canDefend = true;
                return;
            }
            staminaManager.currentStamina -= 2f;
            canDefend = false;
            isDefending = true;
            Animator anim = Sword.GetComponent<Animator>();
            anim.SetTrigger("Defend");
            audioManager.PlaySFX("defend");
            StartCoroutine(ResetDefendCooldown());
        }
        private IEnumerator ResetDefendCooldown()
        {
            StartCoroutine(ResetDefendBool());
            yield return new WaitForSeconds(AttackCooldown);
            canDefend = true;
        }
        IEnumerator ResetDefendBool()
        {
            yield return new WaitForSeconds(AttackCooldown);
            isDefending = false;

        }
        public void SwordMine()
        {
            //if (staminaManager.currentStamina < 50f)
            //{
            //    Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));

            //    Popup.instance.CreatePopUp(transform.position + randomness, "Not enough stamina for mine!", Color.red);

            //    Debug.Log("Not enough stamina to mine!");
            //    isMining = false;
            //    canMine = true;
            //    return;
            //}
            //staminaManager.currentStamina -= 50f;
            
            //canMine = false;
            //isMining = true;
            canAttack = false;
            isAttacking = true;
            Animator anim = Sword.GetComponent<Animator>();
            anim.SetTrigger("Attack");
            audioManager.PlaySFX("slash");
            StartCoroutine(ResetMineCooldown());

        }
        private IEnumerator ResetMineCooldown()
        {
            StartCoroutine(ResetMineBool());
            yield return new WaitForSeconds(AttackCooldown);
            canAttack = true;
        }
        IEnumerator ResetMineBool()
        {
            yield return new WaitForSeconds(AttackCooldown);
            isAttacking = false;
            
        }
        
        // ------------------- //
        // ACTION LOGIC        //
        // ------------------- //

        //private void WeaponAttack()
        //{
        //    Debug.Log(staminaManager.currentStamina);

        //    if (!HasStamina(50f, "Not enough stamina for attackss!")) return;

        //    GameObject player = GameObject.FindWithTag("FPSController");
        //    //if (player.GetComponent<FirstPersonController>().isPauseOpen || player.GetComponent<FirstPersonController>().isPauseOptionsOpen) return;


        //    if (canAttack)
        //    {
        //        isAttacking = true;
        //        canAttack = false;


        //        if (staminaManager.currentStamina < 50f)
        //        {
        //            Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));

        //            Popup.instance.CreatePopUp(transform.position + randomness, "Not enough stamina for attack!", Color.red);

        //            Debug.Log("Not enough stamina to attack!");
        //            isAttacking = false;
        //            canAttack = true;
        //            return;
        //        }


        //        staminaManager.currentStamina -= 50f;

        //        Debug.Log("Current Stamina after attack: " + staminaManager.currentStamina);

        //        Animator anim = GetComponentInChildren<Animator>();

        //        Debug.Log("Animator found: " + (anim != null));
        //        anim.SetTrigger("Attack");


        //        StartCoroutine(ResetAttack());
        //    }
        //}

        //private IEnumerator ResetAttack()
        //{
        //    StartCoroutine(ResetAttackBool());
        //    ; // Decrease stamina by 10 on attack
        //    yield return new WaitForSeconds(AttackCooldown);
        //    canAttack = true;
        //}
        //IEnumerator ResetAttackBool()
        //{


        //    //mainCamera.gameObject.SetActive(false);
        //    //if (attackCamera != null) attackCamera.SetActive(true);

        //    // 2. Play Animation (Trigger logic)
        //    // You can alternate animations here if you want
        //    Animator anim = GetComponentInChildren<Animator>();


        //    if (anim != null)
        //    {
        //        anim.SetTrigger("Attack");
        //        Debug.Log("3");
        //    }

        //    if (shakeCamera != null)
        //    {
        //        shakeCamera.GetComponent<Shake>().start = true;
        //    }
        //    //Time.timeScale = slowMotionTimeScale;
        //    //// Adjust physics so movement stays smooth even in slow mo
        //    //Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

        //    // 3. Wait for the attack to finish
        //    yield return new WaitForSecondsRealtime(attackCamDuration);

        //    //Time.timeScale = 1f;
        //    //Time.fixedDeltaTime = defaultFixedDeltaTime;

        //    // 4. Switch back to Main Camera
        //    //if (attackCamera != null) attackCamera.SetActive(false);
        //    //mainCamera.gameObject.SetActive(true);
        //    yield return new WaitForSeconds(AttackCooldown);
        //    isAttacking = false;

        //    if (shakeCamera != null)
        //    {
        //        shakeCamera.GetComponent<Shake>().start = false;
        //    }
        //}

        private IEnumerator PerformDefend()
        {
            if (!HasStamina(50f, "Not enough stamina to defend!")) yield break;

            isDefending = true;
            canDefend = false;
            staminaManager.currentStamina -= 50f;

            animator.SetTrigger("Defend");

            yield return new WaitForSeconds(0.5f);
            isDefending = false;
            canDefend = true;
        }

        private IEnumerator PerformMine()
        {
            if (!HasStamina(30f, "Not enough stamina to mine!")) yield break;

            isMining = true;
            canMine = false;
            staminaManager.currentStamina -= 30f;

            animator.SetTrigger("Attack"); // Using attack anim for mining as per your code
            Debug.Log("2");
            yield return new WaitForSeconds(mineCooldown);
            isMining = false;
            canMine = true;
        }

        private bool HasStamina(float amount, string message)
        {
            if (staminaManager.currentStamina < amount)
            {
                Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
                Popup.instance.CreatePopUp(transform.position + randomness, message, Color.red);
                return false;
            }
            return true;
        }

        // ------------------- //
        // COLLISION LOGIC     //
        // ------------------- //

        private void OnTriggerEnter(Collider other)
        {
            // FIX: Check if we are currently being "held" and attacking. 
            // If weaponController is null, this is a dropped item and shouldn't deal damage.
            //if (weaponController == null) return;

            // 1. Handle Mining

            if (other.CompareTag("Mineable") && isAttacking)
            {

                if (other.TryGetComponent<MineableResource>(out var resource))
                {
                    resource.TakeDamage(miningDamage);
                }
                
            }
            // 2. Handle Combat
            else if (other.CompareTag("Enemy") && isAttacking)
            {
                Debug.Log("Hit Enemy");
                Debug.Log("Enemy Object: " + other.gameObject.name);
                if (other.TryGetComponent<EnemyAI>(out var enemy))
                {
                    int totalDamage = attackDamage + (characterStats != null ? characterStats.BaseStrength : 0);
                    Debug.Log("Dealt Damage: " + enemy.health);
                    enemy.TakeDamage(totalDamage);
                }
            }
        }
    }
}

