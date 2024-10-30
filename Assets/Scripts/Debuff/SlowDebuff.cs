using UnityEngine;

public class SlowDebuff : Debuff
{
    float _slowAmount;
    GameObject effect;
    float tempValue;

    public SlowDebuff(float duration, float slowAmount) : base(duration)
    {
        this._slowAmount = slowAmount;
    }

    public override void Apply()
    {
        // ȿ�� ����
        tempValue = enemy.MoveSpeed * _slowAmount;
        enemy.MoveSpeed -= tempValue;
        
        // ����Ʈ ����
        effect = SlowDebuffPool.Instance.pool.Get();
        effect.transform.parent = enemy.transform;
        effect.transform.localPosition = Vector3.zero;
        base.Apply();
    }
    public override void Remove()
    {
        base.Remove();
        // ȿ�� ����
        enemy.MoveSpeed += tempValue;

        // ����Ʈ ����
        SlowDebuffPool.Instance.pool.Release(effect);
    }
}
