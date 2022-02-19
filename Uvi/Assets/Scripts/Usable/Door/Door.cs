using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : UsableBase
{
    [SerializeField] private float Speed = 100f;

    private Rigidbody Rigidbody;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public override void DownUse()
    {
        if (!CanUse) return;

        float y = Input.GetAxisRaw("Mouse Y") * Speed;

        Rigidbody.AddTorque(Vector3.up * y);

        base.DownUse();
    }
}
