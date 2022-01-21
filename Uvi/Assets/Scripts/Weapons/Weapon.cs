using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : WeaponBase
{
    [Header("Damge")]
    [SerializeField] private float Damage;

    [Header("Ammo setup")]
    [SerializeField] private int MaxAmmo;
    [SerializeField] private GameObject AmmoBox;
    
    [Header("Other settings")]
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject Light;
    [SerializeField] private float delayTime = 1f;
    [SerializeField] private ParticleSystem ParticleSystem;
    [SerializeField] private AudioClip ShootSound;
    [SerializeField] private GameObject BulletHole;
    [SerializeField] private GameObject BulletEffect;

    private AudioSource AudioSource;
    private bool delayShoot = true;

    public override void WeaponInit()
    {
        AudioSource = GetComponent<AudioSource>();

        ParticleSystem.Stop();
    }

    public override void PrimaryAttack()
    {
        if (!CanShoot || !delayShoot) return;

        Ray ray = new Ray( Camera.transform.position, Camera.transform.forward );

        RaycastHit hit;

        ParticleSystem.Play();
        Animator.SetTrigger("Shoot");
        AudioSource.clip = ShootSound;
        AudioSource.Play();

        if (Physics.Raycast(ray, out hit))
            Shoot(hit);

        StartCoroutine(NextPrimaryAttack(delayTime));
    }

    private void Shoot(RaycastHit hit)
    {
        if (!delayShoot) return;


        if (hit.collider.GetComponent<Rigidbody>() != null)
        {
            hit.rigidbody.AddForceAtPosition(Camera.transform.forward * 1000, hit.point);
            
        } else if (hit.collider.tag != "Use")
        {   
            Transform _bulletHole = Instantiate(BulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)).transform.GetChild(0);

            _bulletHole.localPosition += Vector3.up * 0.01f;
            _bulletHole.localRotation = Quaternion.Euler(_bulletHole.localRotation.x, Random.Range(0, 360), _bulletHole.localRotation.z);
        }

        Instantiate(BulletEffect, hit.point, Quaternion.LookRotation(hit.normal));
    }

    public override void Take(Transform weaponPos)
    {
        base.Take(weaponPos);
    }

    public override void Reload()
    {
        base.Reload();
    }

    private IEnumerator NextPrimaryAttack( float seconds )
    {
        delayShoot = false;
        Light.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        Light.SetActive(false);
        yield return new WaitForSeconds(seconds - 0.1f);
        delayShoot = true;
    }
}
