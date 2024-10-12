using System.Collections.Generic;
using UnityEngine;

public class SnipeTurret : Turret
{
    [Header("총구 위치")]
    [SerializeField] Vector3 muzzlePos;

    private void Start()
    {
        Init();

        upgrades = new List<TurretUpgrade>
        {
            new TurretUpgrade { damage = 2, cost = 33 },
            new TurretUpgrade { damage = 5, cost = 66 },
        };
    }

    private void Update()
    {
        Tick();
    }
    public override void Upgrade()
    {
        base.Upgrade();

        TurretUpgrade upgrade = upgrades[currentLevel - 1];

        _damage += upgrade.damage;
        _attackInterval += upgrade.attackInterval;
    }
    protected override void Attack()
    {
        if (_target == null || _attackInterval - _synergyAttackInterval > _attackIntervalDelta)
            return;

        // 오브젝트 풀에서 총알 가져오기
        GameObject bulletGo = GrenadePool.Instance.pool.Get();
        // 총알 컴포넌트 추가
        bulletGo.AddComponent<CommonBullet>().Init(_target, _damage * _synergyDamagePlus, _bulletSpeed);

        AddDebuffOnBullet(bulletGo);

        // 총알 위치 변경
        bulletGo.transform.position = transform.TransformPoint(muzzlePos);

        _attackIntervalDelta = 0;
    }
}
