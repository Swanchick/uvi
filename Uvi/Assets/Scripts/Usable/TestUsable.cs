using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUsable : UsableBase
{
    public override void Use()
    {
        print("Hello World");
        
        base.Use();
    }
}
