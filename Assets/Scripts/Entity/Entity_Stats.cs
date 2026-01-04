using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatSetup;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;
    public Stat_ResourceGroup resources;
    public Stat_MajorGroup major;

    protected virtual void Awake()
    {

    }

    public AttackData GetAttackData(DamageScaleData scaleData)
    {
        return new AttackData(this, scaleData);
    }
    public float GetElementalDamge(out ElementTypes element, float scaleFactor = 1)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();
        float bonusElementalDamage = major.intelligence.GetValue();

        float highestDamage = fireDamage;
        element = ElementTypes.Fire;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementTypes.Ice;
        }

        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            element = ElementTypes.Lightning;
        }

        if (highestDamage <= 0)
        {
            element = ElementTypes.None;
            return 0;
        }

        float bonusFire = (element == ElementTypes.Fire) ? 0 : fireDamage * .5f;
        float bonusIce = (element == ElementTypes.Ice) ? 0 : iceDamage * .5f;
        float bonusLightning = (element == ElementTypes.Lightning) ? 0 : lightningDamage * .5f;

        float weakerElementalDamage = bonusFire + bonusIce + bonusLightning;
        float finalDamage = highestDamage + weakerElementalDamage +bonusElementalDamage;

        return finalDamage * scaleFactor;

    }

    public float GetElementalResistance(ElementTypes element)
    {
        float baseResistance = 0;
        float bonusResistance = major.intelligence.GetValue() * .5f;

        switch (element)
        {
            case ElementTypes.Fire:
                baseResistance = defense.fireRes.GetValue();
                break;
            case ElementTypes.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;
            case ElementTypes.Lightning:
                baseResistance = defense.lightingRes.GetValue();
                break;
        }

        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f; // 75%
        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap) / 100;

        return finalResistance;
    }
    public float GetPhyiscalDamage(out bool isCrit, float scaleFactor = 1)
    {
        float baseDamage = GetBaseDamage();
        float critChance = GetCritChance();
        float critPower = GetCritPower();

        isCrit = Random.Range(0, 100) < critChance;
        float finalDamage = isCrit ? baseDamage * critPower : baseDamage;

        return finalDamage * scaleFactor;
    }

    public float GetBaseDamage() => offense.damage.GetValue() + major.strength.GetValue(); // bonus dmg from str: +1
    public float GetCritChance() => offense.critChance.GetValue() + (major.agility.GetValue() * .3f); // bonus crit chance from agi: + 0.3%
    public float GetCritPower() => offense.critPower.GetValue() + (major.strength.GetValue() *.5f); // bonus crit power from str: + 0.5%

    public float GetMaxHealth()
    {
        float baseHp = resources.maxHealth.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;
        float finalMaxHealth = baseHp + bonusHp;
        
        return finalMaxHealth;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float totalArmor = GetBaseArmor();

        float reductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMultiplier;

        float mitigation = totalArmor / (totalArmor + 150);
        float mitigationCap = .85f;
        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    public float GetBaseArmor() => defense.armor.GetValue() + major.vitality.GetValue();

    public float GetArmorReduction()
    {
        float finalReduction = offense.armorReduction.GetValue() / 100;

        return finalReduction;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * .5f;

        float evasionCap = 85f;

        float totalEvasion =  baseEvasion + bonusEvasion;

        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;

            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;

            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritChance: return offense.critChance;
            case StatType.CritPower: return offense.critPower;
            case StatType.ArmorReduction: return offense.armorReduction;

            case StatType.FireDamage: return offense.fireDamage;
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;

            case StatType.FireResistance: return defense.fireRes;
            case StatType.IceResistance: return defense.iceRes;
            case StatType.LightningResistance: return defense.lightingRes;

            default:
                Debug.LogWarning($"StatType {type} not implemented yet.");
                return null;
        }
    }

    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log("No default stat setup assigned");
            return;
        }

        resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);

        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critPower.SetBaseValue(defaultStatSetup.critPower);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);

        offense.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offense.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offense.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);

        defense.fireRes.SetBaseValue(defaultStatSetup.fireResistance);
        defense.iceRes.SetBaseValue(defaultStatSetup.iceResistance);
        defense.lightingRes.SetBaseValue(defaultStatSetup.lightningResistance);


    }
}
