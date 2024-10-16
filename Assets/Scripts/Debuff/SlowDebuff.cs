using UnityEngine;

public class SlowDebuff : Debuff
{
    float _slowAmount;
    GameObject effect;

    public SlowDebuff(float duration, float slowAmount) : base(duration)
    {
        this._slowAmount = slowAmount;
    }

    public override void Apply()
    {
        // 효과 적용
        enemy.MoveSpeed *= _slowAmount;
        
        // 이펙트 적용
        effect = SlowDebuffPool.Instance.pool.Get();
        effect.transform.parent = enemy.transform;
        effect.transform.localPosition = Vector3.zero;
        base.Apply();
    }
    public override void Remove()
    {
        base.Remove();
        // 효과 제거
        enemy.MoveSpeed /= _slowAmount;

        // 이펙트 제거
        SlowDebuffPool.Instance.pool.Release(effect);
    }
}
