using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;

    [Header("Unlock Details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;

    [Header("Skill Details")]
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private int skillCost;

    [SerializeField] private Image skillIcon;
    [SerializeField]  private string lockedColorHex = "#8D8D8D";
    private Color lastColor;
    public bool isUnlocked;
    public bool isLocked;
    private UI_TreeConnectHandler connectHandler;


    private void GetNeededComponents()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>(true);
        connectHandler = GetComponentInParent<UI_TreeConnectHandler>(true);
    }

    private void Start()
    {
        UpdateIconColor(GetColorByHex(lockedColorHex));
        UnlockDefaultSkills();
    }

    public void UnlockDefaultSkills()
    {
        GetNeededComponents();
        if (skillData.unlockByDefault)
        {
            Unlock();
        }
    }

    private void Unlock()
    {
        if (isUnlocked)
        {
            Debug.Log("Skill is already unlocked");
            return;
        }

        isUnlocked = true;
        UpdateIconColor(Color.white);
        LockConflictNodes();

        skillTree.RemoveSkillPoints(skillData.cost);
        connectHandler.UnlockConnectionImage(true);

        skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData);
    }

    public void Refund()
    {
        if (!isUnlocked || skillData.unlockByDefault)
        {
            return;
        }

        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));

        skillTree.AddSkillPoints(skillData.cost);
        connectHandler.UnlockConnectionImage(false);

        skillTree.skillManager.GetSkillByType(skillData.skillType).RemoveSkillUpgradeType();
    }

    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
        {
            return false;
        }

        if (skillTree.EnoughSkillPoints(skillData.cost) == false)
        {
            return false;
        }

        foreach (var node in neededNodes)
        {
            if (!node.isUnlocked)
            {
                return false;
            }
        }


        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
            {
                return false;
            }
        }

        return true;
    }

    private void LockChildNodes()
    {
        isLocked = true;

        foreach (var node in connectHandler.GetChildNodes())
        {
            node.LockChildNodes();      
        }
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
        {
            return;
        }
        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
        {
            Unlock();
        }
        else if (isLocked)
        {
            ui.skillToolTip.LockedSkillEffect();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, skillData, this);


        if (isUnlocked || isLocked)
        {
            return;
        }
        ToggleNodeHighlight(true);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, rect);


        if (isUnlocked || isLocked)
        {
            return;
        }
        ToggleNodeHighlight(false);
        
    }

    private void ToggleNodeHighlight(bool highlight)
    {
        Color highLightColor = Color.white * .9f; highLightColor.a = 1;
        Color colorToApply = highlight ? highLightColor : lastColor;

        UpdateIconColor(colorToApply);
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);
        
        return color;
    }

    private void OnDisable()
    {
        if (isLocked)
        {
            UpdateIconColor(GetColorByHex(lockedColorHex));
        }

        if (isUnlocked)
        {
            UpdateIconColor(Color.white);
        }
    }

    private void OnValidate()
    {
        if (skillData == null)
        {
            return;
        }

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
}
