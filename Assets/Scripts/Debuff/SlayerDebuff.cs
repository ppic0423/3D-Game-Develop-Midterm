using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlayerDebuff : Debuff
{
    int _targetHp;
    float _damageIncreaseAmount;

    public SlayerDebuff(float duration, float damageIncreaseAmount, int targetHp) : base(duration)
    {
        _damageIncreaseAmount = damageIncreaseAmount;
        _targetHp = targetHp;
    }

    public override void Apply()
    {
        base.Apply();
        if(enemy.MaxHp >= _targetHp)
            enemy.DamageIncrease += _damageIncreaseAmount;
    }
    public override void Remove()
    {
    }
    public override IEnumerator StartDebuff()
    {
        Apply();
        yield return null;
    }
}
