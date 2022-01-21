using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Manager", menuName = "Create Game Manager")]
public class GameManager : ScriptableObject
{   
    [Header("Background object for menu")]
    public List<GameObject> Backgrounds;
    public int BackgroundMenuID = 0;

    [Header("Load State")]
    public bool LoadSavesState = false;

    [Header("Weapons classes for init in project")]
    public List<GameObject> Weapons;
}
