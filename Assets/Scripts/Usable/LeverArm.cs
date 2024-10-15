using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverArm : UseBase
{
    [Header("Events")]
    [SerializeField] private UnityEvent OnActivate;
    
    [Header("Options")]
    [SerializeField] private float speed = 100f;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip Ambience;

    [Header("Can transmit")]
    [SerializeField] private bool WithOutPrevius = true;
    
    [Header("Electricity")]
    public bool PreviusActivate = false;
    [SerializeField] private LeverArm Next;

    private bool Activated = false;

    public override void UseDown(Player ply)
    {
        if (Activated) return;

        ply.CanCameraRotate = false;
    }

    public override void Use(Player ply)
    {
        if (Activated) return;
        
        if (!PreviusActivate && !WithOutPrevius) return;

        float mouseY = -Input.GetAxis("Mouse Y") * speed;

        Rigidbody.AddTorque(Vector3.right * mouseY);
    }

    public override void UseUp(Player ply)
    {
        ply.CanCameraRotate = true;

        if (transform.rotation.eulerAngles.z >= 89)
        {
            ActivateNext();
        }
    }

    private void ActivateNext()
    {
        if (Activated) return;
        
        Activated = true;

        AudioSource.clip = Ambience;

        AudioSource.Play();

        OnActivate.Invoke();

        if (Next == null) return;
        
        Next.PreviusActivate = true;
    }
}
