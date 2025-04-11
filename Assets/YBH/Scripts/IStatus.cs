using UnityEngine;

public interface IStatus
{
    float HP { get; set; }
    void TakeDamage(int Damage);
    void Die();
}