using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [Header("Options")]
    [SerializeField] private float Damage = 20;
    [SerializeField] private float Distance = 2f;
    [SerializeField] private float NextAttackTime = 0.3f;
    [SerializeField] private GameObject BulletHole;
    [SerializeField] private GameObject BulletEffect;

    private bool canAttcak = true;

    public override void PrimaryAttack()
    {
        if (!canAttcak || !CanShoot) return;
        
        Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Distance))
            Hit(hit);
        else
            Animator.SetTrigger("Fire");

        StartCoroutine(NextPrimaryAttack(NextAttackTime));
    }

    private void Hit(RaycastHit hit)
    {
        if (hit.collider.GetComponent<Rigidbody>() != null)
        {
            hit.rigidbody.AddForceAtPosition(Camera.transform.forward * 1000, hit.point);
        }

        if (hit.collider.GetComponent<IHealth>() != null)
        {
            IHealth health = hit.collider.GetComponent<IHealth>();
            health.SetDamage(Damage);
        }

        if (hit.collider.gameObject.isStatic)
        {
            Transform _bulletHole = Instantiate(BulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)).transform.GetChild(0);

            _bulletHole.localPosition += Vector3.up * 0.01f;
            _bulletHole.localRotation = Quaternion.Euler(_bulletHole.localRotation.x, UnityEngine.Random.Range(0, 360), _bulletHole.localRotation.z);
        }

        Instantiate(BulletEffect, hit.point, Quaternion.LookRotation(hit.normal));

        Animator.SetTrigger("Hit");

        StartCoroutine(Player.CameraShake(0.1f, 0.2f));
    }

    private IEnumerator NextPrimaryAttack(float seconds)
    {
        canAttcak = false;
        yield return new WaitForSeconds(seconds);
        canAttcak = true;
    }
}
