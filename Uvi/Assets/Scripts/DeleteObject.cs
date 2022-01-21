using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    [SerializeField] private float TimeToDelete;

    void Start()
    {
        StartCoroutine(WaitForDelete(TimeToDelete));
    }

    private IEnumerator WaitForDelete(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(gameObject);
    }
}
