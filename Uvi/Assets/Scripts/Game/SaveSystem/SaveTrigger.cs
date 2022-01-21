using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : SaveLoadManager
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        SaveGameByTrigger(gameObject);
        Destroy(gameObject);
    }
}
