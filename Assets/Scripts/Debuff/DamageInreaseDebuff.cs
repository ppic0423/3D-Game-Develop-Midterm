using UnityEngine;

public class DamageInreaseDebuff : Debuff
{
    float _damageIncreaseAmount;
    GameObject effect;

    public DamageInreaseDebuff(float duration, float damageIncreaseAmount) : base(duration)
    {
        _damageIncreaseAmount = damageIncreaseAmount;
    }

    public override void Apply()
    {
        // 이펙트
        effect = WeaknessEffectPool.Instance.pool.Get();
        effect.transform.parent = enemy.transform;
        effect.transform.localPosition = Vector3.zero;

        // 효과 적용
        enemy.DamageIncrease += _damageIncreaseAmount;
        base.Apply();
    }
    public override void Remove()
    {
        base.Remove();
        // 효과 제거
        enemy.DamageIncrease -= _damageIncreaseAmount;

        // 이펙트 제거
        WeaknessEffectPool.Instance.pool.Release(effect);
    }
}
