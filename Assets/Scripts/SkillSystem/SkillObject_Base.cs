using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] private GameObject onHitVfx;
    [Space]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Entity_Stats playerStats;
    protected DamageScaleData damageScaleData;
    protected ElementTypes usedElement;
    protected bool targetGotHit;
    protected Transform lastTarget;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected Collider2D[] GetEnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in GetEnemiesAround(t, radius))
        {
            IDamagable damageable = target.GetComponent<IDamagable>();

            if (damageable == null)
            {
                continue;
            }

            ElementalEffectData effectData = new ElementalEffectData(playerStats, damageScaleData);

            float physDamage = playerStats.GetPhyiscalDamage(out bool isCrit, damageScaleData.physical);
            float eleDamage = playerStats.GetElementalDamge(out ElementTypes element, damageScaleData.elemental);

            targetGotHit = damageable.TakeDamage(physDamage, eleDamage, element, transform);

            if (element != ElementTypes.None)
            {
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, effectData);
            }

            if (targetGotHit)
            {
                lastTarget = target.transform;
                Instantiate(onHitVfx, target.transform.position, Quaternion.identity);
            }

            usedElement = element;
        }
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closetDistance = Mathf.Infinity;

        foreach (var enemy in GetEnemiesAround(transform, 10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            
            if (distance < closetDistance)
            {
                target = enemy.transform;
                closetDistance = distance;
            }
        
        }

        return target;
    }

    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
        {
            targetCheck = transform;
        }

        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);   
    }
}
