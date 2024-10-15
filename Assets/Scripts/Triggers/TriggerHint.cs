using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerHint : MonoBehaviour
{
    [SerializeField] private GameObject Text;
    [SerializeField] private TextMesh TextMesh;
    [SerializeField] private AudioClip FadeSound;
    [SerializeField] private float Speed = 1;
    [SerializeField] private UnityEvent Events;

    private AudioSource AudioSource;

    private float Alpha = 0;

    private bool destroy = false;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();

        AudioSource.clip = FadeSound;

        TextMesh.color = new Color(255, 255, 255, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        Alpha = 2f;

        AudioSource.Play();
    }

    private void Update()
    {
        float _alpha = Mathf.Lerp(TextMesh.color.a, Alpha, Speed);

        TextMesh.color = new Color(1, 1, 1, _alpha);

        if (_alpha <= 0 && destroy)
            Destroy(gameObject);
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        destroy = true;
        Alpha = -1f;
        Events.Invoke();
    }
}
