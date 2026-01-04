using UnityEngine;

public class Firework_AnimationTrigger : SkillObject_AnimationTrigger
{
    private SkillObject_FireworkRocketHead sh => GetComponentInParent<SkillObject_FireworkRocketHead>();
    private void FireworkExplode()
    {
        sh.DeleteFireworkHead();
    }
}
