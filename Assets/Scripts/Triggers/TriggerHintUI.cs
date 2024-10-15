using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerHintUI : MonoBehaviour
{
    public CanvasGroup Panel;
    public Text HadderTextUI;
    public Text MainTextUI;
    public string HadderText = "";
    public string MainText = "";

    public Button Button;

    private float alpha = -1;
    private Player player;
    private bool destroy = false;

    private void Start()
    {
        HadderTextUI.text = "";
        MainTextUI.text = "";

        Panel.alpha = 0f;
    }

    private void Update()
    {
        Panel.alpha = Mathf.Lerp(Panel.alpha, alpha, 0.01f);

        if (Panel.alpha <= 0 && destroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        HadderTextUI.text = HadderText;
        MainTextUI.text = MainText;

        player = other.GetComponent<Player>();

        player.IsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Button.onClick.AddListener(Close);

        alpha = 2;
    }

    private void Close()
    {
        alpha = -1f;

        HadderTextUI.text = "";
        MainTextUI.text = "";

        player.IsPaused = false;
        player = null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Button.onClick.RemoveAllListeners();

        destroy = true;
    }
}
