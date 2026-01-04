using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;
    private Entity_VFX vfx;
    private Entity_Stats stats;

    public DamageScaleData basicAttackScale;

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }
    public void PerformAttack()
    {
        
        foreach (var target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
            {
                continue;
            }

            AttackData attackData = stats.GetAttackData(basicAttackScale);

            float physDamage = attackData.physicalDamage;
            float elementalDamge = attackData.elementalDamage;
            ElementTypes element = attackData.element;

            float damage = stats.GetPhyiscalDamage(out bool isCrit);
            bool targetGotHit = damagable.TakeDamage(damage, elementalDamge, element, transform);


            if (element != ElementTypes.None)
            {
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, attackData.effectData);
            }


            //if (element != ElementTypes.None)
            //{
            //    ApplyStatusEffect(target.transform, element);
            //}

            if (targetGotHit)
            {
                OnDoingPhysicalDamage?.Invoke(physDamage);
                vfx.CreateOnHitVFX(target.transform, attackData.isCrit, element);
            }
        }
    }

    //public void ApplyStatusEffect(Transform target, ElementTypes element, float scaleFactor = 1)
    //{
    //    Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

    //    if (statusHandler == null)
    //    {
    //        return;
    //    }

    //    if (element == ElementTypes.Ice && statusHandler.CanbeApplied(ElementTypes.Ice))
    //    {
    //        statusHandler.ApplyChillEffect(defaultDuration, chillSlowMultiplier);
    //    }

    //    if (element == ElementTypes.Fire && statusHandler.CanbeApplied(ElementTypes.Fire))
    //    {
    //        scaleFactor = fireScale;
    //        float fireDamage = stats.offense.fireDamage.GetValue() * scaleFactor;
    //        statusHandler.ApplyBurnEffect(defaultDuration, fireDamage);
    //    }

    //    if (element == ElementTypes.Lightning && statusHandler.CanbeApplied(ElementTypes.Lightning))
    //    {
    //        scaleFactor = lightningScale;
    //        float lightningDamage = stats.offense.lightningDamage.GetValue() * scaleFactor;
    //        statusHandler.ApplyShockEffect(defaultDuration, lightningDamage, electrifyChargeBuildUp);
    //    }
    //}

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
