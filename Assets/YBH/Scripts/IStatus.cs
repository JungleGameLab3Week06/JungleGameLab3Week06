using System;
using UnityEngine;

public interface IStatus
{
    int Health { get;}
    void TakeDamage(int Damage);

    void Die();
}