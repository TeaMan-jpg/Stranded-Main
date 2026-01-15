using Platformers;
using UnityEngine;
using UnityEngine.AI;

namespace Platformers 
{
    public class EnemyFleeState : EnemyBaseState
    {
        readonly EnemyAI enemy;
        readonly NavMeshAgent agent;
        readonly Transform player;

        public EnemyFleeState(EnemyAI enemy, NavMeshAgent agent, Transform player) : base(enemy)
        {
            this.enemy = enemy;
            this.agent = agent;
            this.player = player;
        }

        public override void OnEnter()
        {
            agent.speed = 6f; // Run faster when scared
        }

        public override void Update()
        {
            if (player == null) return;
            Vector3 fleeDir = (enemy.transform.position - player.position).normalized;
            Vector3 destination = enemy.transform.position + fleeDir * 10f;
            agent.SetDestination(destination);
        }

        public override void OnExit() // Updated to override
        {
            agent.speed = 3.5f; // Reset speed
        }

        
    }


}