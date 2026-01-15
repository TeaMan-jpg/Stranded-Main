using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Platformers {
    public class EnemyAttackState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Transform player;

        public EnemyAttackState(EnemyAI enemy,NavMeshAgent agent,Transform player) : base(enemy)
        {
            this.agent = agent;
            this.player = player;
        }

        public override void OnEnter()
        {
            Debug.Log("Entered Attack State");
        }

        public override void Update()
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(player.position);
            }
            enemyAI.Attack();
        }
    }



}