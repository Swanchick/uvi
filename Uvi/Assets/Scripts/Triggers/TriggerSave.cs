using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSave : MonoBehaviour
{
    [SerializeField] private SaveLoadManager SaveLoadManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        print("Hello WOrld");

        SaveLoadManager.SaveGameByTrigger(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        
    }
}
