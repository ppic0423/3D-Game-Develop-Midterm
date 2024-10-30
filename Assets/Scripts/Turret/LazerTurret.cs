using System.Collections.Generic;
using UnityEngine;

public class LazerTurret : Turret
{
    [Header("레이저 위치")]
    [SerializeField] private Transform startPoint;   // 레이저 시작점 오브젝트
    [SerializeField] private Transform endPoint;     // 레이저 끝점 오브젝트
    [SerializeField] private Transform laserBody;    // 레이저 본체

    [Header("총구 위치")]
    [SerializeField] private Vector3 muzzlePos;

    private void Start()
    {
        Init();

        upgrades = new List<TurretUpgrade>
        {
            new TurretUpgrade { attackInterval = 0.1f, damage = 4, cost = 45 },
            new TurretUpgrade { attackInterval = 0.3f, damage = 10, cost = 90 }
        };

        ToggleLaser(false);
    }

    private void Update()
    {
        Tick();
    }

    protected override void Attack()
    {
        if (_target == null )
        {
            ToggleLaser(false);
            return;
        }

        UpdateLaser();

        _target = FindLowestHpTarget();

        if (_target != null && _attackInterval - _synergyAttackInterval > _attackIntervalDelta)
        {
            Debug.Log(_attackIntervalDelta);
            // SoundManager.Instance.PlaySound(_fireSound);
            ApplyDamageAndDebuffs(_target.GetComponent<Enemy>());
            _attackIntervalDelta = 0;
        }
    }

    private void UpdateLaser()
    {
        Vector3 startPosition = startPoint.position;
        Vector3 endPosition = _target.position;

        endPoint.position = endPosition;
        UpdateLaserBody(startPosition, endPosition);

        ToggleLaser(true);
    }

    private Transform FindLowestHpTarget()
    {
        // 범위 내에 있는 모든 콜라이더를 가져옴
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _findRange, enemyLayer);
        float lowestHp = float.MaxValue;

        // 범위 내에 적이 없을 경우
        if (hitColliders.Length == 0)
        {
            lowestHp = float.MaxValue;
            _target = null;
            return null;
        }

        // Hp가 가장 적은 적을 탐색
        foreach (Collider collider in hitColliders)
        {
            if (collider.GetComponent<Enemy>().Hp < lowestHp)
            {
                lowestHp = collider.GetComponent<Enemy>().Hp;
                return _target;
            }
        }

        return null;
    }

    private void ApplyDamageAndDebuffs(Enemy enemy)
    {
        float bonusDamage = 0;
        if(enemy.Hp <= enemy.MaxHp) 
        {
            bonusDamage = _damage / 2;
        }
        enemy.TakeDamage((_damage + bonusDamage) * _synergyDamagePlus);
        foreach (Debuff debuff in debuffs)
        {
            enemy.ApplyDebuff(debuff);
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();

        TurretUpgrade upgrade = upgrades[currentLevel];
        _attackInterval -= upgrade.attackInterval;
        _damage += upgrade.damage;
    }

    private void UpdateLaserBody(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 midPoint = (startPosition + endPosition) / 2;
        laserBody.position = midPoint;

        float distance = Vector3.Distance(startPosition, endPosition);
        laserBody.localScale = new Vector3(laserBody.localScale.x, distance / 2, laserBody.localScale.z);

        Vector3 direction = (endPosition - startPosition).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);
        laserBody.rotation = rotation;
    }

    private void ToggleLaser(bool isActive)
    {
        startPoint.gameObject.SetActive(isActive);
        endPoint.gameObject.SetActive(isActive);
        laserBody.gameObject.SetActive(isActive);
    }
}