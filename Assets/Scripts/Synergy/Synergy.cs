using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Synergys", menuName = "ScriptableObjects/Synergys", order = 3)]
public abstract class Synergy : MonoBehaviour
{
    [SerializeField] Define.Synergy synergy;
    [SerializeField] protected int effectByCount;
    [SerializeField] protected int enhanceEffectByCount;
    protected int effectCount;

    public virtual void ActivateSynergy(Turret turret, List<Turret> nearTurret)
    {
        turret.ClearSynergy();
        
        // 시너지 개수 확인
        if(turret.synergys.Contains(synergy))
        {
            effectCount++;
        }

        foreach(Turret _turret in nearTurret)
        {
            if(_turret.synergys.Contains(synergy))
                effectCount++;
        }

        // 효과 적용
        if(effectCount >= enhanceEffectByCount)
        {
            AddEnhancedSynergyEffect(turret);
        }
        else if(effectCount >= effectByCount)
        {
            AddSynergyEffect(turret);
        }
    }

    protected abstract void AddSynergyEffect(Turret turret);
    protected abstract void AddEnhancedSynergyEffect(Turret turret);
}