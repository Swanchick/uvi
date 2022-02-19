using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    public bool Using { get; set; } = false;
    public bool CanUse = true;

    public Sprite CrosshairSprite;

    public string WeaponClass = string.Empty;
    public int Ammo = 0;

    protected bool CanShoot = false;

    protected Transform WeaponPos;

    private Rigidbody Rigidbody;
    private BoxCollider BoxCollider;
    protected Animator Animator;

    protected Camera Camera;

    private LayerMask WeaponMask = 6;

    public virtual void PrimaryAttack() { }

    public virtual void Reload() { }

    protected Player Player;

    private void Start()
    {   
        Rigidbody = GetComponent<Rigidbody>();
        BoxCollider = GetComponent<BoxCollider>();
        Animator = GetComponent<Animator>();

        if (Animator != null)
            Animator.enabled = false;

        Camera = Camera.main;

        WeaponInit();
    }

    private void Update()
    {
        if (!Using) return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, WeaponPos.localPosition, Time.deltaTime * 10f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, WeaponPos.localRotation, Time.deltaTime * 10f);
    }

    public virtual void WeaponInit() { }

    private void SetChildMask(LayerMask layer)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.layer = layer;
        }
    }

    public virtual void Take(Transform weaponPos, Player ply)
    {
        if (Using) return;

        WeaponPos = weaponPos;
        Player = ply;
        gameObject.layer = WeaponMask;
        SetChildMask(WeaponMask);
        
        Using = true;

        Rigidbody.isKinematic = true;
        BoxCollider.enabled = false;
        transform.SetParent(weaponPos, true);

        StartCoroutine(CanShootWait());
    }

    public virtual void Drop()
    {
        if (!Using) return;

        if (Animator != null)
            Animator.enabled = false;

        gameObject.layer = 0;

        SetChildMask(0);

        Using = false;

        transform.SetParent(null, true);

        Rigidbody.isKinematic = false;
        BoxCollider.enabled = true;

        Vector3 velocity = Camera.transform.forward * 10f;

        Rigidbody.velocity = velocity;

        transform.rotation = Quaternion.Euler(Camera.transform.rotation.eulerAngles + Vector3.up * -90);
        Player = null;
        CanShoot = false;
    }

    private IEnumerator CanShootWait()
    {
        yield return new WaitForSeconds(0.5f);
        if (Animator != null)
            Animator.enabled = true;

        CanShoot = true;
    }
}
