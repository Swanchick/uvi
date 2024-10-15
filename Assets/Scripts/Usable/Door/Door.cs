using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : UseBase
{
    [SerializeField] private float Speed = 100f;
    [SerializeField] private bool CanOpen = true;
    

    private Rigidbody Rigidbody;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public override void Use(Player ply)
    {
        if (!CanOpen) return;
        
        float mouseY = Input.GetAxis("Mouse Y") * Speed;

        Rigidbody.AddTorque(Vector3.up * mouseY);
    }
}
