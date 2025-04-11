using System;
using UnityEngine;

public interface IStatus
{
    float Health { get; set; }
    void TakeDamage(float Damage);
    
    event Action OnDie;
}