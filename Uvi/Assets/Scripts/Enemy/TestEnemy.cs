using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] private Transform testPos;
    private NavMeshAgent Agent;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();

        Agent.SetDestination(testPos.position);
    }
}