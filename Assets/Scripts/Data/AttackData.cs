using System;
using UnityEngine;

[Serializable]
public class AttackData
{
    public float physicalDamage;
    public float elementalDamage;
    public bool isCrit;
    public ElementTypes element;

    public ElementalEffectData effectData;

    public AttackData(Entity_Stats entityStats, DamageScaleData scaleData)
    {
        physicalDamage = entityStats.GetPhyiscalDamage(out isCrit, scaleData.physical);
        elementalDamage = entityStats.GetElementalDamge(out element, scaleData.elemental);

        effectData = new ElementalEffectData(entityStats, scaleData);
    }
}
