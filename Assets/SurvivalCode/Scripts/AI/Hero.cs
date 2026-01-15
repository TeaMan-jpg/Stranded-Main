using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Hero : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints = new();

    private NavMeshAgent agent;

    // Start is called before the first frame update
    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void OnClick(RaycastHit hit)
    {
        if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas)) {
            agent.SetDestination(navHit.position);
        }
    }
}
