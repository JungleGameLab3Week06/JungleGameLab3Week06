using System;
using UnityEngine;

public interface IStatus
{
    int Health { get; set; }
    void TakeDamage(int Damage);
    
    event Action OnDie;
}