using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers {
    [CreateAssetMenu(fileName = "Difficulty_Settings", menuName = "Difficulty Settings", order = 0)]
    public class DifficultySettings : ScriptableObject
    {
        // Start is called before the first frame update
        [SerializeField] public float enemyHealthMultiplier = 1.0f;
        [SerializeField] public float enemyDamageMultiplier = 1.0f;
        [SerializeField] public float timeBetweenAttacksMultiplier = 1.0f;
        [SerializeField] public float health = 100f;
        [SerializeField] public float attackRange = 2f;
        [SerializeField] public float velocity = 1f;
        [SerializeField] public float attackCooldown = 1f;
        [SerializeField] public float detectionRange = 5f;
        [SerializeField] public float patrolRadius = 3f;
        [SerializeField] public float chaseSpeedMultiplier = 1.0f;
        [SerializeField] public float stunDuration = 1.0f;
        [SerializeField] public float knockbackForce = 5f;
        [SerializeField] public float detectionAngle = 60f;
        [SerializeField] public float innerDetectionRadius = 2f;
        [SerializeField] public float detectionCooldown = 1f;




        [SerializeField] public float enemySpeedMultiplier = 1.0f;
    }


}