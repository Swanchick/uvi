using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class UsableBase : MonoBehaviour
{
    [SerializeField] private UnityEvent Event;
    public bool CanUse = true;
    public Sprite CrosshairImage;

    public virtual void Use()
    {
        Event.Invoke();
    }
        
    public virtual void DownUse()
    {
        
    }

    public virtual void InitLoad()
    {

    }
}
