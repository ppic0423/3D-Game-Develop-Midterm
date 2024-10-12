using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireSynergy : Synergy
{
    [SerializeField] int _duration;
    [SerializeField] int _damage;

    [SerializeField] int _enhancedDuration;
    [SerializeField] int _enhancedDamage;
    protected override void AddSynergyEffect(Turret turret)
    {
        turret.synergyDebuffs.Add(new FireDebuff(_duration, _damage));
        Debug.Log($"Active Fire Synergy");
    }
    protected override void AddEnhancedSynergyEffect(Turret turret)
    {
        turret.synergyDebuffs.Add(new FireDebuff(_enhancedDuration, _enhancedDamage));
        Debug.Log("Enhance Fire Synergy");
    }
}
