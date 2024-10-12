using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidSynergy : Synergy
{
    [SerializeField] float increaseAmount;
    [SerializeField] float enhancedIncreaseAmount;

    protected override void AddSynergyEffect(Turret turret)
    {
        turret._synergyAttackInterval += increaseAmount;
        Debug.Log("Active Rapid Synergy");
    }
    protected override void AddEnhancedSynergyEffect(Turret turret)
    {
        turret._synergyAttackInterval += enhancedIncreaseAmount;
        Debug.Log("Enhance Rapid Synergy");
    }
}
