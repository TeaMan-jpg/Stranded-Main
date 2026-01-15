using Platformers;
using UnityEngine;
using UnityEngine.AI;

// TASK: Move to the Player
public class TaskChase : Node
{
    private NavMeshAgent _agent;
    private Transform _target;

    public TaskChase(NavMeshAgent agent, Transform target)
    {
        _agent = agent;
        _target = target;
    }

    public override NodeState Evaluate()
    {
        if (_target == null) return NodeState.FAILURE;

        _agent.SetDestination(_target.position);

        if (_agent.pathPending) return NodeState.RUNNING;

        // Return SUCCESS if we are close enough
        if (_agent.remainingDistance <= _agent.stoppingDistance)
            return state = NodeState.SUCCESS;

        return state = NodeState.RUNNING;
    }
}

// TASK: Pick a random point on NavMesh and go there
