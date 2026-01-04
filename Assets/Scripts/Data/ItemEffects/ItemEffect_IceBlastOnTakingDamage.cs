using UnityEngine;


[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Ice blast", fileName = "Item effect data - Ice blast on taking damage")]
public class ItemEffect_IceBlastOnTakingDamage : ItemEffect_DataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;

    [Space]
    [SerializeField] private float healthPercentTrigger = .25f;
    [SerializeField] private float cooldown;
    private float lastTimeUsed = -999;

    [Header("Vfx Objects")]
    [SerializeField] private GameObject iceBlastVfx;
    [SerializeField] private GameObject onHitVfx;

    public override void ExcuteEffect()
    {
        bool noCoolDown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHealthPercent() <= healthPercentTrigger;

        Debug.Log("WHAT");
        Debug.Log(reachedThreshold);
        Debug.Log(noCoolDown);

        if (noCoolDown && reachedThreshold)
        {
            player.vfx.CreateEffectOf(iceBlastVfx, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    }

    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach (var target in enemies)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null) continue;

            bool targetGotHit = damagable.TakeDamage(0, iceDamage, ElementTypes.Ice, player.transform);

            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementTypes.Ice, effectData);

            if (targetGotHit)
            {
                player.vfx.CreateEffectOf(onHitVfx, target.transform);
            }
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);

        lastTimeUsed = -999;
        player.health.OnTakingDamage += ExcuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.OnTakingDamage -= ExcuteEffect;
        player = null;
    }
}
