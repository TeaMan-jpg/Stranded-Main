
using KBCore.Refs;
using System;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Utilities;
//using static Strategies;
using Random = UnityEngine.Random;

namespace Platformers
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
    public class EnemyAI : MonoBehaviour
    {

        // Placeholder for EnemyAI implementation
        [SerializeField] NavMeshAgent navAgent;
        StateMachine stateMachine;
        
        [SerializeField] PlayerDetector playerDetector;
        private bool _isDestroyed = false;
        CountdownTimer attackTimer;
        [SerializeField] float attackRange = 2f;
        private AudioManager audioManager;

        [SerializeField] List<Transform> waypoints = new();

        private IObjectPool<EnemyAI> enemyPool;

        [Header("References")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject projectilePrefab;

        [Header("Knockback Settings")]
        [SerializeField] private float mass = 3.0f;
        [SerializeField] private float knockbackDecay = 5.0f;
        private Vector3 knockbackImpact = Vector3.zero;

        // Layer masks for terrain and player detection for navmesh surface
        [Header("Layers")]
        [SerializeField] private LayerMask terrainLayer;
        [SerializeField] private LayerMask playerLayerMask;
        [SerializeField] private ParticleSystem damageParticleSystem;

        private ParticleSystem damageParticleSystemInstance;

        public bool once = true;





        [Header("Combat Settings")]
        [SerializeField] private float attackCooldown = 1f;
        private bool isOnAttackCooldown;
        [SerializeField] private float forwardShotForce = 10f;
        [SerializeField] private float verticalShotForce = 5f;
        [SerializeField] private float wanderRadius = 5f;
        [SerializeField] private FloatingHealthbar healthbar;
        [SerializeField] private XPTracker xpTracker;
        [SerializeField] float fleeHealthThreshold = 30f; // Flee when health < 30
        [SerializeField] float stunDuration = 0.5f;
        public CountdownTimer stunTimer;
        private float defenseMultiplier = 1.0f;

        
        [Header("Detection Ranges")]
        [SerializeField] private float visionRange = 20f;
 

        // Internal state variables

        public float health;
        public float maxHealth = 120f;
        void OnValidate() => this.ValidateRefs();

        private void Start()
        {
            LoadDifficultySettings();
            //attackTimer = new CountdownTimer(timeBetweenAttacks);
            //stunTimer = new CountdownTimer(stunDuration);
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();


            stateMachine = new StateMachine();

            var wanderState = new EnemyWanderState(this, navAgent, wanderRadius);
            var chaseState = new EnemyChaseState(this, navAgent, playerDetector.playerTransform);
            var attackState = new EnemyAttackState(this, navAgent, playerDetector.playerTransform);
            var fleeState = new EnemyFleeState(this, navAgent, playerDetector.playerTransform);
            var stunnedState = new EnemyStunnedState(this, navAgent,stunDuration);
            At(wanderState,chaseState,new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            At(chaseState, attackState, new FuncPredicate(() => CanAttackPlayer()));
            At(attackState, chaseState, new FuncPredicate(() => (playerTransform.position - transform.position).magnitude > attackRange + 1.0f));

            Any(fleeState, new FuncPredicate(() => health < fleeHealthThreshold && health > 0 && playerDetector.CanDetectPlayer()));
            At(fleeState, wanderState, new FuncPredicate(() => health >= fleeHealthThreshold || !playerDetector.CanDetectPlayer()));

            // 5. Stun Logic
            Any(stunnedState, new FuncPredicate(() => stunTimer.IsRunning));
            At(stunnedState, wanderState, new FuncPredicate(() => !stunTimer.IsRunning));
            stateMachine.SetState(wanderState);
        }
        void At(IState from,IState to,IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

        public void SetPool(IObjectPool<EnemyAI> pool) {
            enemyPool = pool;

        }

        
        void Update()
        {
            stateMachine.Update();
            attackTimer.Tick(Time.deltaTime);

    
            // Apply Knockback Logic
            if (knockbackImpact.magnitude > 0.2f)
            {
                // Move the agent manually by the knockback vector
                navAgent.Move(knockbackImpact * Time.deltaTime);

            }
            // Decay the knockback force over time
            knockbackImpact = Vector3.Lerp(knockbackImpact, Vector3.zero, knockbackDecay * Time.deltaTime);

            
        }

        private void LoadDifficultySettings()
        {
            // 1. Get the number from PlayerPrefs (Default to 1 if not found)
            int level = PlayerPrefs.GetInt("SelectedDifficulty", 1);

            // 2. Load the file from the Resources folder using that number
            // This looks for "Difficulty_0", "Difficulty_1", or "Difficulty_2"
            DifficultySettings s = Resources.Load<DifficultySettings>("Difficulty_" + level);

            if (s == null)
            {
                Debug.LogError("Could not find Difficulty_" + level + " in Resources folder!");
                return;
            }

            // 3. Apply the variables exactly as before
            maxHealth = 120f * s.enemyHealthMultiplier;
            health = maxHealth;

            if (navAgent != null)
            {
                navAgent.speed = s.velocity * s.enemySpeedMultiplier;
            }

            attackRange = s.attackRange;
            attackCooldown = s.attackCooldown;


            Console.WriteLine($"Enemy {gameObject.name} loaded Difficulty {level} from PlayerPrefs/Resources.");
            Debug.Log(attackCooldown);
            // Re-initialize your timers
            float finalAttackTime = s.attackCooldown * s.timeBetweenAttacksMultiplier;
            attackTimer = new CountdownTimer(finalAttackTime);
            stunTimer = new CountdownTimer(s.stunDuration);

            defenseMultiplier = s.enemyDamageMultiplier;
            visionRange = s.detectionRange;

            Debug.Log($"Enemy {gameObject.name} loaded Difficulty {level} directly from PlayerPrefs/Resources.");
        }
        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }
        

        // Initialization if playerTransform or navAgent are not assigned in the inspector
        private void Awake()
        {
            if (playerTransform == null)
            {
                GameObject playerObj = GameObject.Find("FPSController");
                if (playerObj != null)
                {
                    playerTransform = playerObj.transform;
                }
            }


            if (navAgent == null)
            {
                navAgent = GetComponent<NavMeshAgent>();
            }

        }


        // Method to destroy the enemy game object normally called upon death when invoked
        private void DestroyEnemy()
        {
            if (_isDestroyed) return;

            // Mark as destroyed so this block cannot run again for this enemy
            _isDestroyed = true;

            // ... any other death logic (play sound, spawn loot) ...

            // Now release to the pool safely
            if (enemyPool != null)
            {
                enemyPool.Release(this);
            }
        }
        private void OnEnable()
        {
            _isDestroyed = false; // Reset the flag so it can be killed again later
                                  // Reset health, animations, etc.
        }


        


        // Method to handle taking damage from the player
        public void TakeDamage(int damage)
        {

            health -= (damage * defenseMultiplier);

            SpawnDamageParticles();
            if (healthbar != null) healthbar.UpdateHealthbar(health, maxHealth);

            Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));

            DamagePopUp.instance.CreatePopUp(transform.position + randomness, damage.ToString(), Color.red);

            // Calculate Knockback Direction (Away from attacker)
            Vector3 difference = new(0, 0, 6500);
            Vector3 knockbackDir = transform.position - difference;
            knockbackDir.y = 0; // Keep knockback horizontal

            // Apply a force (e.g., 15f). You can make this a parameter if needed.
            AddKnockback(knockbackDir, 105f);

            if (health <= 0)
            {
                audioManager.PlaySFX("enemyDeath");
                Invoke(nameof(DestroyEnemy), 0.5f);
                if (xpTracker != null) xpTracker.AddXP(400);
            }
        }

        public void AddKnockback(Vector3 direction, float force)
        {
            direction.Normalize();
            // Add to current impact (allows for multiple hits to stack)
            knockbackImpact += direction * force / mass;
        }

        public bool CanAttackPlayer() {
            if (playerTransform == null) return false;
            var directionToPlayer = (playerTransform.position - transform.position);
            return directionToPlayer.magnitude <= attackRange;
        }


        // Method to fire a projectile towards the player - by creating a rigidbody and applying forces
        private void FireProjectile()
        {
            if (projectilePrefab == null || firePoint == null) return;


            Rigidbody projectileRb = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            projectileRb.AddForce(transform.forward * forwardShotForce, ForceMode.Impulse);
            projectileRb.AddForce(transform.up * verticalShotForce, ForceMode.Impulse);

            // destroy the projectile after 3 seconds to prevent clutter
            Destroy(projectileRb.gameObject, 3f);
        }

        public void StartStunEffects()
        {
            // 1. Visual Feedback: Flash Red or White
            
            // 4. Logic: Stop the agent immediately
            navAgent.isStopped = true;
            navAgent.velocity = Vector3.zero;
        }


        private void SpawnDamageParticles() {
            
            damageParticleSystemInstance = Instantiate(damageParticleSystem, transform.position, Quaternion.identity);
        }
        public void StopStunEffects()
        {
            
            
            // 4. Logic: Resume Agent
            navAgent.isStopped = false;
        }

        // Method to find a random patrol point within the patrol radius


        // Coroutine to handle attack cooldown timing to prevent continuous attacks
        private System.Collections.IEnumerator AttackCooldownRoutine()
        {
            isOnAttackCooldown = true;
            yield return new WaitForSeconds(attackCooldown);
            isOnAttackCooldown = false;
        }


      
        public void Attack() {
            if (attackTimer.IsRunning) return;
            attackTimer.Start();
            if (playerTransform != null)
            {
                transform.LookAt(playerTransform);
            }


            if (!isOnAttackCooldown)
            {
                FireProjectile();
                StartCoroutine(AttackCooldownRoutine());
            }
        }
    }
}
//namespace Platformers
//{
//    [RequireComponent(typeof(NavMeshAgent))]
//    public class EnemyAI : MonoBehaviour
//    {

//        [SerializeField] private NavMeshAgent agent;


//        StateMachine stateMachine;

//        // References to the NavMeshAgent, player transform, and projectile prefab
//        [Header("References")]
//        [SerializeField] private Transform playerTransform;
//        [SerializeField] private Transform firePoint;
//        [SerializeField] private GameObject projectilePrefab;

//        // Layer masks for terrain and player detection for navmesh surface
//        [Header("Layers")]
//        [SerializeField] private LayerMask terrainLayer;
//        [SerializeField] private LayerMask playerLayerMask;


//        [Header("Patrol Settings")]
//        [SerializeField] private float patrolRadius = 10f;
//        private Vector3 currentPatrolPoint;
//        private bool hasPatrolPoint;


//        [Header("Combat Settings")]
//        [SerializeField] private float attackCooldown = 1f;
//        private bool isOnAttackCooldown;
//        [SerializeField] private float forwardShotForce = 10f;
//        [SerializeField] private float verticalShotForce = 5f;

//        [SerializeField] private FloatingHealthbar healthbar;
//        [SerializeField] private XPTracker xpTracker;


//        [Header("Detection Ranges")]
//        [SerializeField] private float visionRange = 20f;
//        [SerializeField] private float engagementRange = 10f;

//        // Internal state variables
//        private bool isPlayerVisible;
//        private bool isPlayerInRange;
//        public float health;
//        public float maxHealth = 120f;

//        // Initialization if playerTransform or navAgent are not assigned in the inspector
//        private void Awake()
//        {
//            if (playerTransform == null)
//            {
//                GameObject playerObj = GameObject.Find("FPSController");
//                if (playerObj != null)
//                {
//                    playerTransform = playerObj.transform;
//                }
//            }


//            if (navAgent == null)
//            {
//                navAgent = GetComponent<NavMeshAgent>();
//            }
//        }

//        // Main update loop to handle player detection and behavior updates
//        private void Update()
//        {
//            DetectPlayer();
//            UpdateBehaviourState();
//        }

//        // Method to destroy the enemy game object normally called upon death when invoked
//        private void DestroyEnemy()
//        {
//            Destroy(gameObject);
//        }


//        private void OnDrawGizmosSelected()
//        {
//            Gizmos.color = Color.red;
//            Gizmos.DrawWireSphere(transform.position, engagementRange);


//            Gizmos.color = Color.yellow;
//            Gizmos.DrawWireSphere(transform.position, visionRange);
//        }

//        // Method to detect the player within vision and engagement ranges
//        private void DetectPlayer()
//        {
//            isPlayerVisible = Physics.CheckSphere(transform.position, visionRange, playerLayerMask);
//            isPlayerInRange = Physics.CheckSphere(transform.position, engagementRange, playerLayerMask);
//        }
//        // Method to handle taking damage from the player
//        public void TakeDamage(int damage)
//        {
//            health -= damage;
//            healthbar.UpdateHealthbar(health, maxHealth);

//            if (health <= 0)
//            {
//                Invoke(nameof(DestroyEnemy), 0.5f);
//                if (xpTracker != null)
//                {
//                    xpTracker.AddXP(400); // Use the instance to call AddXP
//                }
//            }
//        }


//        // Method to fire a projectile towards the player - by creating a rigidbody and applying forces
//        private void FireProjectile()
//        {
//            if (projectilePrefab == null || firePoint == null) return;


//            Rigidbody projectileRb = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
//            projectileRb.AddForce(transform.forward * forwardShotForce, ForceMode.Impulse);
//            projectileRb.AddForce(transform.up * verticalShotForce, ForceMode.Impulse);

//            // destroy the projectile after 3 seconds to prevent clutter
//            Destroy(projectileRb.gameObject, 3f);
//        }

//        // Method to find a random patrol point within the patrol radius
//        private void FindPatrolPoint()
//        {
//            float randomX = Random.Range(-patrolRadius, patrolRadius);
//            float randomZ = Random.Range(-patrolRadius, patrolRadius);


//            Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


//            if (Physics.Raycast(potentialPoint, -transform.up, 2f, terrainLayer))
//            {
//                currentPatrolPoint = potentialPoint;
//                hasPatrolPoint = true;
//            }
//        }

//        // Coroutine to handle attack cooldown timing to prevent continuous attacks
//        private System.Collections.IEnumerator AttackCooldownRoutine()
//        {
//            isOnAttackCooldown = true;
//            yield return new WaitForSeconds(attackCooldown);
//            isOnAttackCooldown = false;
//        }




//        private void PerformPatrol()
//        {
//            if (!hasPatrolPoint)
//                FindPatrolPoint();


//            if (hasPatrolPoint)
//                navAgent.SetDestination(currentPatrolPoint);


//            if (Vector3.Distance(transform.position, currentPatrolPoint) < 1f)
//                hasPatrolPoint = false;
//        }


//        private void PerformChase()
//        {
//            if (playerTransform != null)
//            {
//                navAgent.SetDestination(playerTransform.position);
//            }
//        }


//        private void PerformAttack()
//        {
//            navAgent.SetDestination(transform.position);


//            if (playerTransform != null)
//            {
//                transform.LookAt(playerTransform);
//            }


//            if (!isOnAttackCooldown)
//            {
//                FireProjectile();
//                StartCoroutine(AttackCooldownRoutine());
//            }
//        }

//        // Method to update the enemy's behavior based on player detection
//        private void UpdateBehaviourState()
//        {
//            if (!isPlayerVisible && !isPlayerInRange)
//            {
//                PerformPatrol();
//            }
//            else if (isPlayerVisible && !isPlayerInRange)
//            {
//                PerformChase();
//            }
//            else if (isPlayerVisible && isPlayerInRange)
//            {
//                PerformAttack();
//            }
//        }
//    }
//}