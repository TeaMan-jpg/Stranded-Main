using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Platformers {

    public class EnemyChaseState: EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Transform player;

        public EnemyChaseState(EnemyAI enemyAI,NavMeshAgent agent,Transform player) : base(enemyAI)
        {
            this.agent = agent;
            this.player = player;


        }

        public override void OnEnter()
        {
            Debug.Log("Entering Chase State");
        }

        public override void Update() {
          
       
            agent.SetDestination(player.position);
        }

    }

}