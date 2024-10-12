using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlayerSynergy : Synergy
{
    [SerializeField] float _increaseDamage;
    [SerializeField] int _targetHp;

    [SerializeField] float _enhancedIncreaseDamage;
    [SerializeField] int _enhancedTargetHp;
    protected override void AddSynergyEffect(Turret turret)
    {
        turret.synergyDebuffs.Add(new SlayerDebuff(0, _increaseDamage, _targetHp));
        Debug.Log("Active Slayer Synergy");
    }
    protected override void AddEnhancedSynergyEffect(Turret turret)
    {
        turret.synergyDebuffs.Add(new SlayerDebuff(0, _enhancedIncreaseDamage, _enhancedTargetHp));
        Debug.Log("Enhance Slayer Synergy");
    }
}
