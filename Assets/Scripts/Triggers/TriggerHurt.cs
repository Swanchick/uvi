using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHurt : MonoBehaviour
{
    private Health PlayerHealth;
    private Player Player;
    [SerializeField] private float delay;
    [SerializeField] private float damage = 10f;
    private float duration;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        PlayerHealth = other.GetComponent<Health>();
        Player = other.GetComponent<Player>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (PlayerHealth == null || Player.IsDead) return;
        
        if (duration > delay)
        {
            PlayerHealth.SetDamage(damage);
            duration = 0;
        }

        duration += Time.deltaTime;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        PlayerHealth = null;
        duration = 0;
    }
}