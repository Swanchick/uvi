using UnityEngine;

public interface IWeapon
{
    bool Using { get; set; }

    void PrimaryAttack();
    void Reload();
    void Take( Transform weaponPos, Player ply );
    void Drop();
}
