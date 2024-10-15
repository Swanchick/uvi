using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour, IHealth
{
    [SerializeField] public float health { get; set; } = 100f;
    public bool IsAlive { get; set; } = true;

    [SerializeField] private UnityEvent DeathEvent;

    private Rigidbody Rigidbody;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();

        if (Rigidbody != null)
            Rigidbody.isKinematic = true;
    }

    public void SetDamage(float damage)
    {
        if (!IsAlive) return;

        health -= damage;

        if (CheckAlive())
            Kill();
    }

    public bool CheckAlive()
    {
        return health <= 0;
    }

    public void Kill()
    {
        gameObject.layer = 10;

        foreach (Transform child in transform)
            child.gameObject.layer = 10;

        if (Rigidbody != null)
            Rigidbody.isKinematic = false;
        else
            Destroy(gameObject);
        
        DeathEvent.Invoke();

        IsAlive = false;
    }

}
