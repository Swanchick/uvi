using System;
using System.Collections.Generic;
using UnityEngine;

interface IHealth
{
    float health { get; set; }
    bool IsAlive { get; set; }

    void SetDamage(float damage);

    bool CheckAlive();
    void Kill();
}