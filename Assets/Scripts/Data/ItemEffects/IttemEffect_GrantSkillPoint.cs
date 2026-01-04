using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Grant skill point", fileName = "Item effect data - Grant Skill Point")]
public class IttemEffect_GrantSkillPoint : ItemEffect_DataSO
{
    [SerializeField] private int pointsToAdd;

    public override void ExcuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoints(pointsToAdd);
    }
}
