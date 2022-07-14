using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected Player Player;
    [SerializeField] public bool EnemyActivate = false;

    [SerializeField] protected float Distance;
    [SerializeField] protected float MaxDistance = 20f;
    [SerializeField] protected float MinDistance = 5f;
    [SerializeField] private float Axis = 60f;

    [SerializeField] protected Transform PlayerDetector;

    private void Update()
    {
        if (EnemyActivate)
            Activate();
        else
            Disabled();

        PlayerDetector.LookAt(Player.transform);
    }

    protected virtual void Activate() { }

    protected virtual void Disabled()
    {
        Ray ray = new Ray(PlayerDetector.position, PlayerDetector.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, PlayerDetector.forward * MaxDistance);

        

        if (PlayerDetector.localRotation.eulerAngles.y > Axis && PlayerDetector.localRotation.eulerAngles.y < 360 - Axis) return;

        if (Physics.Raycast(ray, out hit, MaxDistance))
        {
            if (hit.collider.tag == "Player")
                
                EnemyActivate = true;
        }
    }
}
