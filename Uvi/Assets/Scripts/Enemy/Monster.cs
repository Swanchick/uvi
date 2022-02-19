using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Monster : EnemyBase, IHealth
{
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private UnityEvent DeathEvent;

    public float health { get; set; } = 100f;
    public bool IsAlive { get; set; } = true;

    [SerializeField] private float SpeedAttack = 2f;
    private float delay = 0;

    

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    protected override void Activate()
    {   
        if (!IsAlive) return;

        Debug.DrawRay(transform.position, PlayerDetector.forward * 20f);

        PlayerDetector.LookAt(Player.transform);

        transform.rotation = Quaternion.Lerp(transform.rotation, transform.localRotation, 4f * Time.deltaTime);

        Ray ray = new Ray(transform.position, PlayerDetector.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, MaxDistance))
        {
            if (hit.collider.tag == "Player")
                Agent.SetDestination(Player.transform.position);
            else
                EnemyActivate = false;
        }

        TakeDamage();

        base.Activate();
    }

    private void TakeDamage()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, MinDistance))
        {

            if (hit.collider.tag == "Player")
            {
                if (delay >= SpeedAttack)
                {
                    Player.health.SetDamage(Random.Range(20f, 50f));
                    delay = 0;
                }

                delay += Time.deltaTime;
            }
        }
    }

    protected override void Disabled()
    {
        base.Disabled();
    }

    public void SetDamage(float damage)
    {
        if (!IsAlive) return;

        health -= damage;

        if (CheckAlive())
            Kill();
    }

    public bool CheckAlive()
    {   
        return health <= 0;
    }

    public void Kill()
    {
        gameObject.layer = 10;
        
        foreach (Transform child in transform)
            child.gameObject.layer = 10;

        DeathEvent.Invoke();

        IsAlive = false;
        Agent.enabled = false;
        Rigidbody.constraints = RigidbodyConstraints.None;
        Rigidbody.isKinematic = false;
    }
}