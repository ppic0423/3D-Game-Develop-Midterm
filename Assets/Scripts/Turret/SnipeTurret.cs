using System.Collections.Generic;
using UnityEngine;

public class SnipeTurret : Turret
{
    [Header("�ѱ� ��ġ")]
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

        GetComponent<AudioSource>().PlayOneShot(_fireSound);

        //sSoundManager.Instance.PlaySound(_fireSound);

        // ������Ʈ Ǯ���� �Ѿ� ��������
        GameObject bulletGo = GrenadePool.Instance.pool.Get();
        // �Ѿ� ������Ʈ �߰�
        bulletGo.AddComponent<CommonBullet>().Init(_target, _damage * _synergyDamagePlus, _bulletSpeed);

        AddDebuffOnBullet(bulletGo);

        // �Ѿ� ��ġ ����
        bulletGo.transform.position = transform.TransformPoint(muzzlePos);

        _attackIntervalDelta = 0;
    }
}
