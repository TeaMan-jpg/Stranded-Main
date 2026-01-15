using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

using Platformers; // Your namespace for DifficultySettings

public class GuardBT : MonoBehaviour
{
    private Node _root;
    private NavMeshAgent _agent;
    private Transform _player;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("FPSController").transform;

        SetupTree();
    }

    void SetupTree()
    {
        // 1. Get the current difficulty settings
        var settings = DifficultyManager.Instance.currentDifficulty;

        // 2. Apply speed from difficulty
        _agent.speed = settings.velocity * settings.enemySpeedMultiplier;
        _agent.stoppingDistance = settings.attackRange;

        // 3. Construct the Tree Logic
        // Priority 1: Chase Player (If visible/near)
        // Priority 2: Patrol (Fallback)
        _root = new Selector(new List<Node>
        {
            new TaskChase(_agent, _player),
            new TaskPatrol(_agent, settings.patrolRadius)
        });
    }

    void Update()
    {
        if (_root != null)
            _root.Evaluate();
    }
}