using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class RegularEnemy : LivingBeing
{

    void Start()
    {
        
    }
    
    void FixedUpdate()
    {
        if (ProcessKnockback())
        {
            
        }
    }
    
}
