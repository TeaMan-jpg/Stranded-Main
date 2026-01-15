using Controller;
			 
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // Not explicitly used, can remove if not needed
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Platformers
{
    public class FirstPersonController : MonoBehaviour
    {
        private CharacterController characterController;
        [SerializeField] private SettingsMenu settingsMenu;
        private float movementSpeed = 15f; // Or any default value
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private float verticalRotationLimit = 80f;
        [SerializeField] private float sprintMultiplier = 1.2f;
        [SerializeField] private float jumpHeight = 5.0f;
        [SerializeField] private float gravity = 9.81f;
        private int count = 0;
        [SerializeField] private InputActionAsset playerActions;
        [Header("Attack Camera Settings")]
        [SerializeField] private GameObject attackCamera; // Drag the 3rd person camera here
        [SerializeField] private float attackCamDuration = 0.8f; // How long the camera stays switched
        private bool isAttacking = false;
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction sprintAction;
        private InputAction lookAction;
        private InputAction mineAction;
        private InputAction dashAction;
        private Vector2 moveInput;
        private Vector2 lookInput;
        [SerializeField] GameObject weaponHolder;
        private GameObject inventoryMenu;
        private InputAction attackAction;
        private InputAction defendAction;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private ParticleSystem running;
        private ParticleSystem runningInstance;
        private InputAction interaction;
        private InputAction crouchAction;
        public GameObject optionMenuPause;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private HealthManager healthManager;
        private readonly Animator animator;
        [SerializeField] private ShopManager shopManager;
        [HideInInspector] public static bool isInventoryOpen = false;
        public float mineDistance = 3f; // How far the player can mine
        public LayerMask mineableLayer;
        public float currentHeight;
        public float crouchHeight = 1f;
        public float crouchTransitionSpeed = 10f;
        public bool isPauseOpen = false;
        public bool isPauseOptionsOpen = false;
        private bool canMine = true; // To prevent spamming mining
        public float mineRate = 1f; // How often the player can mine (seconds between mines)
        public Button closeButton;

        public float turnSmoothVelocity;
        public float Height;
        [SerializeField] private Transform cameraPivot; // Drag the Pivot here

        public GameObject pauseMenu;
        public GameObject optionsPauseMenus;
        public StaminaManagerActual StaminaManager;

        private bool isCrouching => standingHeight - currentHeight > 0.1f;

        private float verticalRotation;
        [SerializeField] private Camera mainCamera;
        private Vector3 currentMovement = Vector3.zero;

        public float attackDamage = 1f;
        public float attackDistance = 2f;
        public float attackRate = 1f;
        public float attackDelay = 1f;
        //public WeaponController weaponController;

        public LayerMask npcLayer;

        public LayerMask attackLayer;
        public GameObject hitEffect;
        public Slider staminaSlider;
        public float standingHeight;

        public float interactionDistance = 4f;
        Vector3 initialCameraPosition;
        private bool isTryingToCrouch = false;

        public float turnSmoothTime = 0.1f;

        public const string ANIMATION_ATTACK_01 = "Attack_01";
        public const string ANIMATION_ATTACK_02 = "Attack_02";

        [SerializeField] private StaminaManagerActual staminaManager;

        public bool controlsEnabled = true;

        private bool dashing = false;
        private float dashingPower = 20f;
        private float dashingTime = 0.2f;
        private float dashingCooldown = 0.75f;


        public void SpawnRunningParticles() {
        
            runningInstance = Instantiate(running, transform.position + new Vector3(0, -1f, 0), Quaternion.identity);
        }
        
        private void Start()
        {
            standingHeight = currentHeight = Height;
            initialCameraPosition = mainCamera.transform.localPosition;
            Debug.Log($"Height: {characterController.height} | Radius: {characterController.radius} | Step Offset: {characterController.stepOffset}");
            attackCamera.SetActive(false);
            //staminaManager = GetComponent<StaminaManagerActual>();
            if (staminaManager == null)
            {
                Debug.LogError("StaminaManager not found on this GameObject!");
            }
        }

        public void SavePlayer()
        {
            SaveSystem.SavePlayer(this);
        }
        public void LoadPlayer()
        {
            PlayerData data = SaveSystem.LoadPlayer();

        }






        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventoryMenu = GameObject.Find("MainInvenGroup");
            inventoryMenu.SetActive(false);
									 
			 
															
			 
            inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

            shopManager.HideCoinsText();
            shopManager.gameObject.SetActive(false);
            shopManager.CloseShop();

            closeButton.gameObject.SetActive(false);


            characterController = GetComponent<CharacterController>();
            healthManager = GameObject.Find("HealthManager").GetComponent<HealthManager>();


            mainCamera = Camera.main;

            // Initial cursor state for gameplay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Ensure playerActions is assigned in the Inspector
            if (playerActions == null)
            {
                Debug.LogError("Player Actions Asset not assigned in FirstPersonController!", this);
                return;
            }

            moveAction = playerActions.FindActionMap("Player").FindAction("Move");
            jumpAction = playerActions.FindActionMap("Player").FindAction("Jump");
            sprintAction = playerActions.FindActionMap("Player").FindAction("Sprint");
            lookAction = playerActions.FindActionMap("Player").FindAction("Look");
            attackAction = playerActions.FindActionMap("Player").FindAction("Attack");
            defendAction = playerActions.FindActionMap("Player").FindAction("Defend");
            mineAction = playerActions.FindActionMap("Player").FindAction("Mine");
            interaction = playerActions.FindActionMap("Player").FindAction("Interact");
            crouchAction = playerActions.FindActionMap("Player").FindAction("Crouch");
            dashAction = playerActions.FindActionMap("Player").FindAction("Dash");

            // Input action callbacks
            moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            moveAction.canceled += ctx => moveInput = Vector2.zero;

            lookAction.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            lookAction.canceled += ctx => lookInput = Vector2.zero;


        }


        private IEnumerator Dash(Vector3 dashDir)
        {
            dashing = true;

            // Ensure the dash is purely horizontal
            dashDir.y = 0;
            dashDir.Normalize();

            float startTime = Time.time;
            while (Time.time < startTime + dashingTime)
            {
                // Use the direction passed from the double-tap check
                characterController.Move(dashDir * dashingPower * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(dashingCooldown);
            dashing = false;
        }



        public void TryToCrouch()
        {




            if (crouchAction.WasPressedThisFrame())
            {
                isTryingToCrouch = !isTryingToCrouch;
            }

            float targetHeight = isTryingToCrouch ? standingHeight / 2f : standingHeight;

            if (isCrouching && !isTryingToCrouch)
            {
                var castOrigin = transform.position + new Vector3(0, currentHeight / 2, 0);
                if (Physics.Raycast(castOrigin, Vector3.up, out RaycastHit hit, 0.2f))
                {
                    var distanceToCeiling = hit.point.y - castOrigin.y;
                    targetHeight = Mathf.Max(currentHeight + distanceToCeiling - 0.1f, crouchHeight);

                }
            }


            if (!Mathf.Approximately(targetHeight, currentHeight))
            {
                var crouchDelta = crouchTransitionSpeed * Time.deltaTime;
                currentHeight = Mathf.Lerp(currentHeight, targetHeight, crouchDelta);
                var halfHeightDifference = new Vector3(0, (characterController.height - currentHeight) / 2f, 0);
                var newCameraPosition = isTryingToCrouch ? initialCameraPosition - halfHeightDifference : initialCameraPosition;
                mainCamera.transform.localPosition = newCameraPosition;
                Height = characterController.height;

                characterController.height = currentHeight;

            }






        }
        public void PauseMenuUpdates()
        {
            if (Keyboard.current.zKey.wasPressedThisFrame)
            {


                // Check if ANY menu is currently open
                if (pauseMenu.activeSelf)
                {



                    CloseAllMenus();
                    //Cursor.visible = false;
                    //Cursor.lockState = CursorLockMode.Locked;

                }
                else if (optionsPauseMenus.activeSelf) {
                    OpenPauseOptionsMenu();
                }
                else
                {
                    OpenMainPauseMenu();
                }
            }
        }

        public void OpenMainPauseMenu()
        {
            pauseMenu.SetActive(true);
            optionMenuPause.SetActive(true); // Ensure options are closed

            isPauseOpen = true;
            isPauseOptionsOpen = false;

            SetControlsEnabled(false);

            // Optional: Stop time and show cursor
            // Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void OpenPauseOptionsMenu()
        {
            pauseMenu.SetActive(false);
            optionMenuPause.SetActive(true);
            isPauseOpen = false;
            isPauseOptionsOpen = true;
            SetControlsEnabled(false);
        }

        public void CloseAllMenus()
        {
            pauseMenu.SetActive(false);
            optionMenuPause.SetActive(false);

            isPauseOpen = false;
            isPauseOptionsOpen = false;

            SetControlsEnabled(true);

            // Optional: Resume time and hide cursor
            // Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDashPerformed(InputAction.CallbackContext context)
        {
            if (dashing) return;

            // 1. Identify which specific key triggered the multi-tap
            string keyName = context.control.name;

            // 2. Determine direction based on the key name
            Vector3 tapDir = Vector3.zero;

            switch (keyName)
            {
                case "w":
                case "upArrow":
                    tapDir = transform.forward;
                    break;
                case "s":
                case "downArrow":
                    tapDir = -transform.forward;
                    break;
                case "a":
                case "leftArrow":
                    tapDir = -transform.right;
                    break;
                case "d":
                case "rightArrow":
                    tapDir = transform.right;
                    break;
            }

            // 3. Trigger the dash with this specific direction
            if (tapDir != Vector3.zero && staminaManager.currentStamina >= 30f)
            {
                staminaManager.currentStamina -= 30f;
                StartCoroutine(Dash(tapDir));
            }
        }

        private IEnumerator AttackSequence()
        {
            isAttacking = true;

            // 1. Switch to Attack Camera
            mainCamera.gameObject.SetActive(false);
            if (attackCamera != null) attackCamera.SetActive(true);

            // 2. Play Animation (Trigger logic)
            // You can alternate animations here if you want
            if (animator != null)
            {
                animator.SetTrigger(ANIMATION_ATTACK_01);
            }

            // 3. Wait for the attack to finish
            yield return new WaitForSeconds(attackCamDuration);

            // 4. Switch back to Main Camera
            if (attackCamera != null) attackCamera.SetActive(false);
            mainCamera.gameObject.SetActive(true);

            isAttacking = false;
        }



        // Mining method based on raycasting from the camera						 
        public void Mine()
        {
            if (!canMine || GameManager.isInventoryOpen) return; // Don't mine if not ready or inventory is open



            canMine = false;
            Invoke(nameof(ResetMine), mineRate);

            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, mineDistance, mineableLayer))
            {
                Debug.Log("Hit something mineable: " + hit.collider.name);



                // checks if the hit object has a MineableResource component
                if (hit.collider.TryGetComponent<MineableResource>(out var mineable))
                {
                    mineable.TakeDamage(attackDamage); // Assuming mining does "damage" to the resource
                }

            }

        }

        // Resets the mining ability after cooldown
        private void ResetMine()
        {
            canMine = true;
        }

        // Handles right-click interaction with NPCs
        private void HandleRightClickInteract()
        {
            if (GameManager.isInventoryOpen || !controlsEnabled)
            {
                Debug.Log("Cannot interact right now.");
                // Don't interact if inventory is open, game is paused, or controls are disabled
                return;
            }

            // Perform a raycast from the center of the screen
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            // Check if the ray hits an enemy object within a certain distance
            if (Physics.Raycast(ray, out hit, attackDistance, npcLayer)) // Reuse attackDistance for interaction range
            {
                Debug.Log("Interacted with: " + hit.collider.name);
                // Check if the hit object has a NormalAI component
                NormalAI enemyHealth = hit.collider.GetComponent<NormalAI>();
                if (enemyHealth != null)
                {
                    Debug.Log("Interacted with NPC: " + enemyHealth);
                    shopManager.gameObject.SetActive(true);
                    shopManager.ShowCoinsText();
                    shopManager.OpenShop();
                    closeButton.gameObject.SetActive(true);


                }

            }

        }

        // Handles all the movement and rotation each frame
        private void Update()
        {

            if (characterController == null)
            {
                Debug.LogError("Missing CharacterController", this.gameObject);
            }
            Debug.Log("Update method called." + Cursor.lockState);
            HandleMovement();
            HandleRotation();
            TryToCrouch();
            InventoryState();
            PauseMenuUpdates();

            // 1. Define your base speeds
            float walkSpeed = 15f;
            float sprintSpeed = walkSpeed * sprintMultiplier; // e.g. 15 * 1.2 = 18
            float crouchSpeed = 5f;

            // 2. Check inputs
            bool isSprintingInput = sprintAction.ReadValue<float>() > 0;
            bool canSprint = isSprintingInput && staminaManager.CanPerformAction();
            mouseSensitivity = SettingsMenu.MouseSensitivity;

            // ... existing null checks ...

            // ADD THIS BLOCK HERE
            // Check if Attack button pressed AND we aren't already attacking
            if (attackAction.triggered && !isAttacking)
            {
                // Check stamina (optional, keeping your existing logic style)
                if (staminaManager.CanPerformAction())
                {
                    staminaManager.StartStaminaDrain(); // Consume stamina for attack
                    StartCoroutine(AttackSequence());
                }
            }

            // ... existing HandleMovement, HandleRotation ...


            // 3. Determine the correct speed based on priority (Crouch > Sprint > Walk)
            if (isCrouching)
            {

                // Force drain stop if crouching
                if (isSprintingInput)
                {

                    if (staminaManager.currentStamina < 1f)
                    {
                        Debug.Log("Out of Stamina, cannot sprint while crouching.");
                        movementSpeed = crouchSpeed;
                    }

                    else
                    {
                        Debug.Log("Out of Stamina, cannot sprint.");
                        movementSpeed = walkSpeed * 0.5f;
                    }
                }
                else
                {
                    Debug.Log("Out of Stamina, cannot sprint while crouching.");
                    movementSpeed = crouchSpeed;

                }
                staminaManager.StopStaminaDrain();

            }
            else if (canSprint)
            {

                if (staminaManager.currentStamina < 1f)
                {
                    Debug.Log("Out of Stamina, cannot sprint.");

                    movementSpeed = walkSpeed;
                }
                // Apply Sprint
                else
                {
                    staminaManager.StartStaminaDrain();
                    movementSpeed = sprintSpeed;
                }
            }
            else
            {
                // Normal Walk
                staminaManager.StopStaminaDrain();
                movementSpeed = walkSpeed;
            }


            // 4. Handle Direction and final movement

            //float horizontal = Input.GetAxisRaw("Horizontal");
            //float vertical = Input.GetAxisRaw("Vertical");
            //Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

            //float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            ////transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            Vector3 moveDirections = transform.right * moveInput.x + transform.forward * moveInput.y;
            moveDirections.Normalize(); // Normalize to prevent faster diagonal movement

            //moveDir.Normalize();

            currentMovement.x = moveDirections.x * movementSpeed; // updated to use moveDirections
            currentMovement.z = moveDirections.z * movementSpeed; // updated to use moveDirections
            SpawnRunningParticles();


            if (staminaManager.CanPerformAction() && jumpAction.triggered)
            {
                staminaManager.StartStaminaDrain();
            }
        }

        // Enable and disable input actions 
        private void OnEnable()
        {

            if (controlsEnabled)
            {
                EnableInputActions();
            }
        }
        private void OnDisable()
        {
            DisableInputActions();
        }

        // Enable and disable all input actions
        private void EnableInputActions()
        {
            moveAction.Enable();
            jumpAction.Enable();
            sprintAction.Enable();
            lookAction.Enable();

            mineAction.Enable();
            interaction.Enable();
            interaction.performed += ctx => HandleRightClickInteract();
            crouchAction.Enable();
            dashAction.Enable();
            dashAction.performed += OnDashPerformed;
        }

        private void DisableInputActions()
        {
            moveAction.Disable();
            jumpAction.Disable();
            sprintAction.Disable();
            lookAction.Disable();

            mineAction.Disable();
            interaction.Disable();
            interaction.performed -= ctx => HandleRightClickInteract();
            crouchAction.Disable();
            dashAction.performed -= OnDashPerformed;
            dashAction.Disable();

        }


        public const string IDLE = "Idle";
        public const string WALK = "Walk";
        public const string ATTACK1 = "Attack 1";
        public const string ATTACK2 = "Attack 2";

        string currentAnimationState;

        public void ChangeAnimationState(string newState)
        {
            // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
            if (currentAnimationState == newState) return;

            // PLAY THE ANIMATION //
            currentAnimationState = newState;
            animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
        }

        void SetAnimations()
        {
            // If player is not attacking
            if (!attacking)
            {

            }
        }

        // ------------------- //
        // ATTACKING BEHAVIOUR //
        // ------------------- //

        [Header("Attacking")]
       
        public float attackSpeed = 1f;
        


        public AudioClip swordSwing;
        public AudioClip hitSound;

        bool attacking = false;
        bool readyToAttack = true;
        int attackCount;

        public void Attack()
        {
            if (!readyToAttack || attacking) return;

            readyToAttack = false;
            attacking = true;

            Invoke(nameof(ResetAttack), attackSpeed);
            Invoke(nameof(AttackRaycast), attackDelay);

            
            
            ChangeAnimationState(ATTACK1);
                
            
            
        }

        void ResetAttack()
        {
            attacking = false;
            readyToAttack = true;
        }

        void AttackRaycast()
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, 3, attackLayer))
            {


                if (hit.transform.TryGetComponent<EnemyAI>(out EnemyAI T))
                { T.TakeDamage(10); }
            }
        }

        // Handle collisions with projectiles so that the player can take damage or block them
        private void OnCollisionEnter(Collision collision)
        {
            // We only care about objects tagged "Projectile".
            if (collision.gameObject.CompareTag("Projectile"))
            {

                Debug.Log("hit");
                // --- THIS IS THE NEW LOGIC ---

                if (weaponHolder != null)
                {
                    // --- DEFENDING LOGIC ---
                    for (int i = 0; i < weaponHolder.transform.childCount; i++) {
                        SwordCollisionDetector weapon = weaponHolder.transform.GetChild(i).GetComponent<SwordCollisionDetector>();
                        if (weapon != null && weapon.isDefending)
                        {
                            Debug.Log("Blocked! Player defended against a projectile.");
                            // Optionally, you can add effects or sounds for blocking here.
                            return; // Exit the method since the attack was blocked.

                        }

                    }
                }
                else
                {
                    // --- NOT DEFENDING LOGIC (Take Damage) ---
                    Debug.Log("HIT! Player took damage from a projectile.");
                    Debug.Log(healthManager.health);
                    Debug.Log(healthManager.maxHealth);
                    // Try to get the Projectile component to access damage value.

                    if (collision.gameObject.TryGetComponent<Projectile>(out var projectile))
                    {

                        if (healthManager != null)
                        {
                            healthManager.TakeDamage(0);

                        }
                    }
                }


            }

        }

        // Handles gravity and jumping mechanics
        private void HandleGravityAndJumping()
        {
            if (characterController.isGrounded)
            {
                currentMovement.y = -0.5f;
                if (jumpAction.triggered)
                {
                    currentMovement.y = jumpHeight;
                }
            }
            else
            {
                currentMovement.y -= gravity * Time.deltaTime;
            }
        }

        private void HandleMovement()
        {
            // Read sprint input
            float speedModifier = 1f;

            
            Vector3 moveDirections = transform.right * moveInput.x + transform.forward * moveInput.y;

            moveDirections.Normalize();

            // Apply speed and modifier
            currentMovement.x = moveDirections.x * movementSpeed * speedModifier;
            currentMovement.z = moveDirections.z * movementSpeed * speedModifier;



            HandleGravityAndJumping(); // Apply gravity and jump to currentMovement.y

            characterController.Move(currentMovement * Time.deltaTime);
        }



        // This method is called when the CharacterController hits a collider while moving

        private void OnControllerColliderHit(ControllerColliderHit hit) // 
        {

            if (hit.gameObject.CompareTag("DroppedItem"))
            {
                Debug.Log("Collided with DroppedItem");
                if (hit.gameObject.TryGetComponent<ItemObjects>(out var itemObject))
                {
                    Debug.Log("Picking up item");
                    count++;
                    if (count == 1) {
                        inventoryManager.AddItems(itemObject.item);
                    }
                    count = 0;


                    // Add item to player's inventory
                    Destroy(hit.gameObject);
                }
            }

        }


        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LevelLoading")) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);            
            
            }
        }



        // Manages the inventory menu state and input
        public void InventoryState()
        {

													   

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                // Prevent opening inventory if shop is open
                if (GameManager.IsShopOpen) return;

                if (inventoryMenu.activeSelf)
                {
                    
                    inventoryMenu.SetActive(false);
                    SetControlsEnabled(true); // Re-enable controls when closing inventory
                    isInventoryOpen = false;
                    GameManager.isInventoryOpen = false;

                }
                else
                {
                    if (isPauseOpen || isPauseOptionsOpen) return;
                        inventoryMenu.SetActive(true);
                    SetControlsEnabled(false); // Disable controls when opening inventory
                    isInventoryOpen = true;

                    GameManager.isInventoryOpen = true;
                }
            }



        }

        private void HandleRotation()
        {
            // Apply horizontal rotation to the player body (around Y-axis)
            float mouseX = lookInput.x * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseX);

            // Apply vertical rotation to the camera (around X-axis)
																 
													
												   


            float mouseY = lookInput.y * mouseSensitivity;



            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

            mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }


        // Enable or disable player controls normally when opening UI menus
        public void SetControlsEnabled(bool enabled = true)
        {

            controlsEnabled = enabled;
																						 

            if (enabled)
            {

                playerActions.Enable();

                // 2. Lock and hide the cursor for first-person gameplay.
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;



            }
            else
            {

                playerActions.Disable();

													  

                Cursor.lockState = CursorLockMode.None;


                Cursor.visible = true;
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																					


                moveInput = Vector2.zero;
                lookInput = Vector2.zero;

            }
        }
    }
}
