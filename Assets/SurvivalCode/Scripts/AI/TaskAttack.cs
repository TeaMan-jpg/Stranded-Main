using Platformers;
using UnityEngine;
using UnityEngine.AI;


public class TaskPatrol : Node
{
    private readonly NavMeshAgent _navMeshAgent;
    private readonly float _patrolRadius;

    public TaskPatrol(NavMeshAgent agent, float radius)
    {
        _navMeshAgent = agent;
        _patrolRadius = radius;
    }

    public override NodeState Evaluate()
    {
        if (!_navMeshAgent.hasPath || _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * _patrolRadius;
            randomDirection += _navMeshAgent.transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _patrolRadius, 1))
            {
                _navMeshAgent.SetDestination(hit.position);
                return state = NodeState.SUCCESS;
            }
        }

        return state = NodeState.RUNNING;
    }
}