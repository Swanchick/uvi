using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health")]
    public float health = 100;
    [SerializeField] private RectTransform Background;
    [SerializeField] private RectTransform HealthBar;
    [SerializeField] private float speed = 5f;

    public float GetHealth()
    {
        return health;
    }

    [Header("Player")]
    private Player Player;
    private CharacterController Controller;
    
    [Header("Camera")]
    private Camera Camera;
    private Rigidbody CameraRigidBody;
    private BoxCollider CameraCollider;
    
    private void Start()
    {
        Player = GetComponent<Player>();
        Controller = GetComponent<CharacterController>();
        Camera = Camera.main;
    }

    private void Update()
    {   
        float healthDelta = health*2;

        Background.sizeDelta = Vector2.Lerp(Background.sizeDelta, new Vector2(healthDelta, Background.sizeDelta.y), 2f * Time.deltaTime);
        HealthBar.sizeDelta = Vector2.Lerp(HealthBar.sizeDelta, new Vector2(healthDelta, HealthBar.sizeDelta.y), speed * Time.deltaTime);
    }

    public void SetHealth(float count)
    {
        health = count;
    }

    public void SetDamage(float damage)
    {
        if (Player.IsDead) return;
        
        StartCoroutine(Player.CameraShake(0.1f, 0.5f));
        
        health -= damage;

        if (CheckPlayerAlive())
            Player.KillPlayer();
    }

    private bool CheckPlayerAlive()
    {
        if (health <= 0) return true;

        return false;
    }
}