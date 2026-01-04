using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVfx;
    private Entity_Stats entityStats;
    private ElementTypes currentEffect = ElementTypes.None;
    private Entity_Health entityHealth;

    [Header("Electrify effect details")]
    [SerializeField] private GameObject lightingStrikeVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1;
    private Coroutine electrifyCo;

    private void Awake()
    {
        entityStats = GetComponent<Entity_Stats>();
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        entityHealth = GetComponent<Entity_Health>();
    }

    public void RemoveAllNegativeEffects()
    {
        StopAllCoroutines();
        currentEffect = ElementTypes.None;
        entityVfx.StopAllVfx();
    }

    public void ApplyStatusEffect(ElementTypes element, ElementalEffectData effectData)
    {
        if (element == ElementTypes.Ice && CanbeApplied(ElementTypes.Ice))
        {
            ApplyChillEffect(effectData.chillDuration, effectData.chillSlowMultiplier);
        }

        if (element == ElementTypes.Fire && CanbeApplied(ElementTypes.Fire))
        {
            ApplyBurnEffect(effectData.burnDuration, effectData.totalBurnDamage);
        }

        if (element == ElementTypes.Ice && CanbeApplied(ElementTypes.Ice))
        {
            ApplyShockEffect(effectData.shockDuration, effectData.shockDamage, effectData.shockCharge);
        }
    }

    private void ApplyShockEffect(float duration, float damage, float charge)
    {
        float lightningResistance = entityStats.GetElementalResistance(ElementTypes.Lightning);
        float finalCharge = charge * (1 - lightningResistance);

        currentCharge += finalCharge;

        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(damage);
            StopShockEffect();
            return;
        }

        if (electrifyCo != null)
        {
            StopCoroutine(electrifyCo);
        }

        electrifyCo = StartCoroutine(ShockEffectCo(duration));
    }

    private void StopShockEffect()
    {
        currentEffect = ElementTypes.None;
        currentCharge = 0;
        entityVfx.StopAllVfx();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightingStrikeVfx, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(damage);
    }

    private IEnumerator ShockEffectCo(float duration)
    {
        currentEffect = ElementTypes.Lightning;
        entityVfx.PlayOnStatusVfx(duration, ElementTypes.Lightning);

        yield return new WaitForSeconds(duration);

        StopShockEffect();
    }

    private void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = entityStats.GetElementalResistance(ElementTypes.Fire);
        float finalDamage = fireDamage * (1 - fireResistance);

        StartCoroutine(BurnEffectCo(duration, finalDamage));
    }

    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementTypes.Fire;
        entityVfx.PlayOnStatusVfx(duration, ElementTypes.Fire);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);

        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;
        
        for (int i = 0; i < tickCount; i++)
        {
            entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementTypes.None;
    
    }

    private void ApplyChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = entityStats.GetElementalResistance(ElementTypes.Ice);
        float finalDuration = duration * (1 - iceResistance); 

        StartCoroutine(ChillEffectCo(finalDuration, slowMultiplier));
    }

    private IEnumerator ChillEffectCo(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        currentEffect = ElementTypes.Ice;
        entityVfx.PlayOnStatusVfx(duration, ElementTypes.Ice);

        yield return new WaitForSeconds(duration);

        currentEffect = ElementTypes.None;
    }

    public bool CanbeApplied(ElementTypes element)
    {
        if (element == ElementTypes.Lightning && currentEffect == ElementTypes.Lightning)
            return true;

        return currentEffect == ElementTypes.None;
    }
}
