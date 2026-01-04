using UnityEngine;

public class SkillObject_AnimationTrigger : MonoBehaviour
{
    private SkillObject_TimeEcho timeEcho;

    private void Awake()
    {
        timeEcho = GetComponentInParent<SkillObject_TimeEcho>();
    }

    private void AttackTrigger()
    {
        timeEcho.PerformAttack();
    }

    private void TryTerminated(int currentAttackIndex)
    {
        if (currentAttackIndex == timeEcho.maxAttack)
        {
            timeEcho.HandleDeath();
        }
    }

}
