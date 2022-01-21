using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManagers : SaveLoadManager
{
    [SerializeField] private GameManagerCollection GameManager;


    void Start()
    {
        if (!GameManager.LoadSavesState) return;
        print("Hello World");
        LoadGame();
    }
}
