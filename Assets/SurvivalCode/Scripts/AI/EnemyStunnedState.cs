using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Platformers {
    public class EnemyStunnedState : EnemyBaseState
    {
        readonly float stunnedDuration;
        float stunnedTimer;
        readonly Transform player;
        public EnemyStunnedState(EnemyAI enemy,NavMeshAgent navAgent, float stunnedDuration) : base(enemy)
        {
            this.stunnedDuration = stunnedDuration;
        }
        public override void OnEnter()
        {
            Debug.Log("Entered Stunned State");
            stunnedTimer = 0f;
            enemyAI.StartStunEffects();
        }
        public override void OnExit()
        {
            enemyAI.StopStunEffects();
        }
        public override void Update()
        {
            stunnedTimer += Time.deltaTime;
            if (stunnedTimer >= stunnedDuration)
            {
                // Transition back to chase state after being stunned
            }
        }
    }
}