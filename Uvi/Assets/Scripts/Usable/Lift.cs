using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private bool Activate = false;
    [SerializeField] private float speed = 10f;

    [SerializeField] private Transform LiftPos;

    private Transform ToPos;

    private Transform StartPos;

    [SerializeField] private Transform EndPos;

    private void Start()
    {
        StartPos = LiftPos;

        ToPos = StartPos;
    }

    private void Update()
    {
        LiftPos.localPosition = Vector3.Lerp(LiftPos.position, ToPos.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || !Activate) return;

        ToPos = EndPos;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player" || !Activate) return;

        ToPos = StartPos;
    }

    public void ChangeActivate()
    {
        Activate = true;
    }
}
