using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class FireDebuff : Debuff
{
    float _damagePerTick = 0;
    float tickInterval = 0;
    public FireDebuff(float duration, float damage) : base(duration)
    {
        _damagePerTick = damage;
    }

    public override void Apply()
    {
        base.Apply();
    }
    public override void Remove()
    {
        base.Remove();
    }

    public override IEnumerator StartDebuff()
    {
        Apply();

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            enemy.TakeDamage(_damagePerTick);  // 매 tick마다 데미지를 줌
            yield return new WaitForSeconds(tickInterval);
            elapsedTime += tickInterval;
        }

        Remove();
    }
}
