 using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager { get; private set; }
    public Player player { get; private set; }
    public DamageScaleData damageScaleData;

    [Header("General Details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        skillManager = GetComponentInParent<Player_SkillManager>();
        player = GetComponentInParent<Player>();
        lastTimeUsed -= cooldown;
        damageScaleData = new DamageScaleData();
    }

    public virtual void TryUseSkill()
    {

    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        damageScaleData = upgrade.damageScale;
        ResetCooldown();
    }

    public void RemoveSkillUpgradeType() => upgradeType = SkillUpgradeType.None;

    public virtual bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None)
        {
            return false;
        }

        if (OnCoolDown())
        {
            Debug.LogWarning("On Cooldown");
            return false;
        }
        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;
    protected bool OnCoolDown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ReduceCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;
    public void ResetCooldown() => lastTimeUsed = Time.time - cooldown;
}
