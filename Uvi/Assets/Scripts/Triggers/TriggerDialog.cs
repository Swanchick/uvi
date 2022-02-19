using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TriggerDialog : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    
    [SerializeField] private Text AvtorUI;
    [SerializeField] private Text TextUI;

    [SerializeField] private string AvtorText;
    [SerializeField] private string MainText;

    [SerializeField] private float LifeTime;

    [SerializeField] private UnityEvent Event;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        Panel.SetActive(true);

        AvtorUI.text = AvtorText;
        TextUI.text = MainText;

        Player ply = other.GetComponent<Player>();

        StartCoroutine(Wait(LifeTime));
    }

    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Panel.SetActive(false);
        Destroy(gameObject);
    }
}
