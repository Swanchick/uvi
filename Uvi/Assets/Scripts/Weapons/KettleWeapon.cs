using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleWeapon : WeaponBase
{
    [SerializeField] private float delay;
    [SerializeField] private bool CanUseWeapon = true;
    [SerializeField] private float Distance = 3f;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    public override void Take(Transform weaponPos)
    {
        StartCoroutine(Delay());

        base.Take(weaponPos);
    }

    public override void PrimaryAttack()
    {
        if (!CanUseWeapon) return;
        
        Shoot();
        StartCoroutine(Delay());

        base.PrimaryAttack();
    }

    private void Shoot()
    {
        Animator.SetTrigger("Shoot");

        Ray ray = new Ray(transform.position, camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Distance))
        {
            if (hit.collider.name == "Cup")
            {
                print("Hello World");
            }
        }
    }

    private IEnumerator Delay()
    {
        CanUseWeapon = false;

        yield return new WaitForSeconds(delay);

        CanUseWeapon = true;
    }
}
