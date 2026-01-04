using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UI_StatToolTip : UI_ToolTip
{
    private Player_Stats playerStats;
    private TextMeshProUGUI statToolTipText;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindFirstObjectByType<Player_Stats>();
        statToolTipText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowToolTip(bool show, RectTransform targetRect, StatType statType)
    {
        base.ShowToolTip(show, targetRect);

        statToolTipText.text = GetStatTextByType(statType);
    }

    public string GetStatTextByType(StatType type)
    {
        switch (type)
        {
            // Major Stats
            case StatType.Strength:
                return "Increase physical damage by 1 per points." + "\n Increase critical power by 0.5% per point.";
            
            case StatType.Agility:
                return "Increase critical chance by 0.3% per points." + "\n Increase evasion by 0.5% per point.";

            case StatType.Vitality:
                return "lazy to type";

            case StatType.Intelligence:
                return "lazy to type";


            // Offense Stats

            case StatType.Damage:
                return "lazy to type";

            case StatType.CritChance:
                return "lazy to type";

            case StatType.CritPower:
                return "lazy to type";

            case StatType.ArmorReduction:
                return "lazy to type";

            case StatType.AttackSpeed:
                return "lazy to type";

            // Defense
            case StatType.MaxHealth:
                return "lazy to type";

            case StatType.HealthRegen:
                return "lazy to type";

            case StatType.Evasion:
                return "lazy to type";

            case StatType.Armor:
                return "lazy to type";


            // Elemental
            case StatType.IceDamage:
                return "lazy to type";

            case StatType.FireDamage:
                return "lazy to type";

            case StatType.LightningDamage:
                return "lazy to type";

            case StatType.ElementalDamge:
                return "lazy to type";


            // Elemental Res
            case StatType.IceResistance:
                return "lazy to type";

            case StatType.FireResistance:
                return "lazy to type";

            case StatType.LightningResistance:
                return "lazy to type";

            default:
                return "No tootip avaliable for this stat.";

        }
    }
}
