using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : UsableBase
{
    [SerializeField] private float Speed = 100f;

    private BoxCollider BoxCollider;

    private void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
    }

    public override void DownUse()
    {
        if (!CanUse) return;

        float y = Input.GetAxisRaw("Mouse Y");

        y = Mathf.Clamp(y, -5, 5);

        if ((transform.localRotation.eulerAngles.y > 350 && transform.localRotation.eulerAngles.y < 360) && y < 0)
        {
            transform.localRotation = Quaternion.identity;
            return;
        } 

        if ((transform.localRotation.eulerAngles.y > 90 && transform.localRotation.eulerAngles.y < 350) && y > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 90, 0);
            return;
        }

        y *= Speed;

        float rotationY = (transform.localRotation.y + y) * Time.deltaTime;
        transform.Rotate(0, rotationY, 0);

        base.DownUse();
    }
}
