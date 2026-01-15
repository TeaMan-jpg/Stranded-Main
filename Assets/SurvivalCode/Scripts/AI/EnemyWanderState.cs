using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Platformers {


    public class EnemyWanderState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Vector3 startPoint;
        readonly float wanderRadius;


        // Start is called before the first frame update
        public EnemyWanderState(EnemyAI enemyAI,NavMeshAgent agent,float wanderRadius) : base(enemyAI)
        {
            this.agent = agent;
            this.startPoint = enemyAI.transform.position;
            this.wanderRadius = wanderRadius;
        }

        public override void OnEnter()
        {
            Debug.Log("Entering Wander State");
        }

        // Inside EnemyWanderState.cs

        public override void Update()
        {
            // SAFETY CHECK: If the agent isn't on the mesh yet, don't do anything
            if (!agent.isOnNavMesh) return;

            if (HasReachedDestination())
            {
                var randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += startPoint;

                NavMeshHit navHit;
                // Check if SamplePosition actually found a valid spot
                if (NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(navHit.position);
                }
            }
        }

        bool HasReachedDestination()
        {
            // CRITICAL FIX: Ensure the agent is on the mesh before checking distance
            if (!agent.isOnNavMesh) return false;

            return !agent.pathPending &&
                   agent.remainingDistance <= agent.stoppingDistance &&
                   (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
        }

        //bool HasReachedDestination() {
        //    return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
        //}
    }


}