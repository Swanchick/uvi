using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    
    private GameObject FindInstanceID(int id)
    {
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

        foreach (object obj in allGameObjects)
        {
            GameObject ent = (GameObject)obj;

            if (ent.GetInstanceID() == id)
            {
                return ent;
            }
        }

        return new GameObject();
    }


    private void Start()
    {
        int value = -1582;

        GameObject obj = FindInstanceID(value);
    }   
}
