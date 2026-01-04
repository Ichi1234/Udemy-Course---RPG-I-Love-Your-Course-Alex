using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;

    [Header("Attak Upgrades")]
    [SerializeField] private int maxAttack = 3;
    [SerializeField] private float duplicateChance = .3f;

    [Header("Heal wisp Upgrades")]
    [SerializeField] private float damagePercentHealed = .3f;
    [SerializeField] private float cooldownReducedInSeconds;

    public float GetPercentOfDamageHealed()
    {
        if (!ShouldBeWisp() && upgradeType != SkillUpgradeType.TimeEcho_HealWisp)
        {
            return 0;
        }
        return damagePercentHealed;
    }

    public float GetCooldownReduceInSecond()
    {
        if (upgradeType != SkillUpgradeType.TimeEcho_CooldownWisp)
        {
            return 0;
        }
        return cooldownReducedInSeconds;
    }

    public bool CanRemoveNegativeEffects() => upgradeType == SkillUpgradeType.TimeEcho_CleanWisp;

    public bool ShouldBeWisp()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_HealWisp
            || upgradeType == SkillUpgradeType.TimeEcho_CleanWisp
            || upgradeType == SkillUpgradeType.TimeEcho_CooldownWisp;
    }

    public float GetDuplicateChance()
    {
        if (upgradeType != SkillUpgradeType.TimeEcho_ChanceToDuplicate)
        {
            return 0;
        }

        return duplicateChance;
    }

    public int GetMaxAttack()
    {
        if (upgradeType == SkillUpgradeType.TimeEcho_SingleAttack || upgradeType == SkillUpgradeType.TimeEcho_ChanceToDuplicate)
        {
            return 1;
        }

        if (upgradeType == SkillUpgradeType.TimeEcho_MultiAttack)
        {
            return maxAttack;
        }

        return 0;
    }

    public float GetEchoDuration()
    {
        return timeEchoDuration;
    }

    public override void TryUseSkill()
    {
        if (!CanUseSkill())
        {
            return;
        }

        CreateTimeEcho();
    }

    public void CreateTimeEcho(Vector3? targetPosition = null)
    {
        Vector3 position = targetPosition ?? transform.position;

        GameObject timeEcho = Instantiate(timeEchoPrefab, position, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetupEcho(this);
    }
}
