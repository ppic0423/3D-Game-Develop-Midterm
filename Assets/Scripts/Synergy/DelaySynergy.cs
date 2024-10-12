using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DelaySynergy : Synergy
{
    [SerializeField] float _slowDuration;
    [SerializeField] float _slowAmount;

    [SerializeField] float _enhancedSlowDuration;
    [SerializeField] float _enhancedSlowAmount;
    protected override void AddSynergyEffect(Turret turret)
    {
        turret.synergyDebuffs.Add(new SlowDebuff(_slowDuration, _slowAmount));
        Debug.Log("Active Delay Synergy");
    }
    protected override void AddEnhancedSynergyEffect(Turret turret)
    {
        turret.synergyDebuffs.Add(new SlowDebuff(_enhancedSlowDuration, _enhancedSlowAmount));
        Debug.Log("Enhance Delay Synergy");
    }
}
