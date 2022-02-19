using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private UnityEvent EnterEvent;
    [SerializeField] private UnityEvent ExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        EnterEvent.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        ExitEvent.Invoke();
    }

    public void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    
}
