using System.Collections.Generic;
using UnityEngine;

public class MineTurret : Turret
{
    [Header("특수 능력")]
    [SerializeField] float radius;
    List<MineBullet> mines; // 생성한 지뢰 목록

    [Header("총구 위치")]
    [SerializeField] Vector3 muzzlePos;

    private void Start()
    {
        Init();

        mines = new List<MineBullet>();
    }
    private void Update()
    {
        _attackIntervalDelta += Time.deltaTime;

        Attack();
    }

    protected override void Attack()
    {
        if (_attackInterval - _synergyAttackInterval > _attackIntervalDelta || mines.Count >= 20)
            return;

        GetComponent<AudioSource>().PlayOneShot(_fireSound);

        GameObject mineGo = MinePool.Instance.pool.Get();
        mineGo.transform.position = transform.TransformPoint(muzzlePos);

        transform.LookAt(mineGo.GetComponent<MineBullet>().Init(null, _damage * _synergyDamagePlus, _bulletSpeed, radius));

        AddDebuffOnBullet(mineGo);

        MineBullet mine = mineGo.GetComponent<MineBullet>();
        mine.m_List = mines;
        mines.Add(mine);

        _attackIntervalDelta = 0;
    }
    public override void Upgrade()
    {
        base.Upgrade();
        TurretUpgrade upgrade = upgrades[currentLevel];
        _damage += upgrade.damage;
        radius += upgrade.range;
    }
}