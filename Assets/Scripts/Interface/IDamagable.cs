using UnityEngine;

public interface IDamagable
{
    public bool TakeDamage(float damage, float elementalDamage, ElementTypes element, Transform damageDealer);
}
