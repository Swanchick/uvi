using UnityEngine;

public abstract class UseBase : MonoBehaviour, IUse
{
    [SerializeField] public Sprite Crosshair { get; set; }
    [SerializeField] public Sprite CrosshairImage;
    public bool LongUse = false;

    private void Start()
    {
        Crosshair = CrosshairImage;
    }

    public virtual void Use(Player ply)
    {

    }

    public virtual void UseOnObject(Player ply)
    {

    }

    public virtual void UseDown(Player ply)
    {

    }

    public virtual void UseUp(Player ply)
    {

    }

    public virtual bool CanUse()
    {
        return true;
    }
}