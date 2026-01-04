using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();
    public override bool TakeDamage(float damage, float elementalDamage, ElementTypes element, Transform damageDealer)
    {
        if (!canTakeDamage)
        {
            return false;
        }
        
        bool wasHit = base.TakeDamage(damage, elementalDamage, element, damageDealer);

        if (!wasHit)
        {
            return false;
        }

        if (damageDealer.GetComponent<Player>() != null) 
        {
            enemy.TryEnterBattleState(damageDealer);
        }

        return true;
    }
}
