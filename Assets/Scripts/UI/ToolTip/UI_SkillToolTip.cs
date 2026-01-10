using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;

    [Space]
    [SerializeField] private string metConitionHex;
    [SerializeField] private string notMetConditionHex;
    [SerializeField] private string importantInfoHex;
    [SerializeField] private Color exampleColor;
    [SerializeField] private string lockedSkillText = "You've taken a different path - this skill is now locked.";

    private Coroutine textEffectCo;

    protected override void Awake()
    {
        base.Awake();
        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);
    }

    public override void ShowToolTip(bool show, RectTransform targetRect)
    {
        base.ShowToolTip(show, targetRect);
    }

    public void ShowToolTip(bool show, RectTransform targetRect, Skill_DataSO skillData, UI_TreeNode node)
    {
        base.ShowToolTip(show, targetRect);

        if (!show)
        {
            return;
        }

        skillName.text = skillData.displayName;
        skillDescription.text = skillData.description;

        if (node == null)
        {
            skillRequirements.text = "";
            return;
        }

        string skillLockedText = $"<color={importantInfoHex}>{lockedSkillText}</color>";
        string requirements = node.isLocked ? skillLockedText : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);

        skillRequirements.text = requirements;
    }

    public void LockedSkillEffect()
    {
        StopLockedSkillEffect();
        textEffectCo = StartCoroutine(TextBlinkEffect(skillRequirements, .15f, 3));
    }

    public void StopLockedSkillEffect()
    {
        if (textEffectCo != null)
        {
            StopCoroutine(textEffectCo);
        }
    }

    private IEnumerator TextBlinkEffect(TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = GetColoredText(notMetConditionHex, lockedSkillText);

            yield return new WaitForSeconds(blinkInterval);

            text.text = GetColoredText(importantInfoHex, lockedSkillText);

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Requirements:");

        string costColor = skillTree.EnoughSkillPoints(skillCost) ? metConitionHex : notMetConditionHex;
        string costText = $"- {skillCost} skill point(s)";
        string finalCostText = GetColoredText(costColor, costText);

        sb.AppendLine(finalCostText);

        foreach (var node in neededNodes)
        {
            if (node == null) continue;
            string nodeColor = node.isUnlocked ? metConitionHex : notMetConditionHex;
            sb.AppendLine($"<color={nodeColor}> - {node.skillData.displayName} </color>");
        }

        if (conflictNodes.Length <= 0)
        {
            return sb.ToString();
        }

        sb.AppendLine(); // spacing
        sb.AppendLine($"<color={importantInfoHex}>- Locks out: </color>");

        foreach (var node in conflictNodes)
        {
            if (node == null) continue;
            sb.AppendLine(GetColoredText(importantInfoHex, node.skillData.displayName));
        }


        return sb.ToString();
    }


  
}
