using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : WeaponBase
{   
    public override void PrimaryAttack()
    {
        if (!CanShoot) return;
        base.PrimaryAttack();
    }
}
