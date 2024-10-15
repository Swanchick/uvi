using UnityEngine;
using UnityEngine.UI;

public interface IUse
{
    Sprite Crosshair { get; set; }

    void Use(Player ply);
    void UseOnObject(Player ply);
    void UseDown(Player ply);
    void UseUp(Player ply);
    bool CanUse();
}