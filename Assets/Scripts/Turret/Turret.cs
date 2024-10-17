using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class Turret : MonoBehaviour
{
    #region 터렛 능력치
    [Header("터렛 능력치")]
    [SerializeField] protected int _damage; // 공격력
    [SerializeField] protected float _findRange = 5f;  // 탐색 범위
    [SerializeField] protected float _attackInterval;
    protected float _attackIntervalDelta = 0;
    protected float _bulletSpeed = 30f;
    public int Cost
    {
        get { return _cost; }
        set { _cost = value; }
    }
    [SerializeField] protected int _cost;
    #endregion
    #region 업그레이드
    [Header("업그레이드")]
    [HideInInspector] public int currentLevel = 0; // 현재 레벨
    public List<TurretUpgrade> upgrades = new List<TurretUpgrade>(); // 업그레이드 리스트
    #endregion
    [Header("태그")]
    [SerializeField] public List<Define.Synergy> synergys = new List<Define.Synergy>();
    [HideInInspector] public List<Debuff> synergyDebuffs = new List<Debuff>();
    [HideInInspector] public float _synergyAttackInterval = 0;
    [HideInInspector] public float _synergyDamagePlus = 1;
    [SerializeField] public AudioClip _fireSound;
    protected List<Debuff> debuffs = new List<Debuff>();
    protected Transform _target;

    protected virtual void Init()
    {
    }
    protected virtual void Tick()
    {
        _attackIntervalDelta += Time.deltaTime;

        FindObjectsInRange();
        RotateTowardsTarget();
        Attack();
    }

    protected abstract void Attack();
    protected void AddDebuffOnBullet(GameObject bulletGo)
    {
        // 총알에 디버프 추가
        Bullet bullet = bulletGo.GetComponent<Bullet>();

        foreach (Debuff debuff in debuffs)
        {
            bullet.GetComponent<Bullet>().AddDebuffs(debuff);
        }
        foreach (Debuff debuff in synergyDebuffs)
        {
            bullet.GetComponent<Bullet>().AddDebuffs(debuff);
        }
    }

    public virtual void Upgrade()
    {
        if (currentLevel >= upgrades.Count || ResourceManager.Instance.Gold < upgrades[currentLevel].cost)
            return;

        ResourceManager.Instance.UseGold(upgrades[currentLevel].cost);
        currentLevel++;
        Debug.Log(currentLevel);
    }
    protected virtual void FindObjectsInRange()
    {
        // 범위 내에 있는 모든 콜라이더를 가져옴
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _findRange, enemyLayer);
        float maxDistanceTravelled = float.MinValue;

        // 범위 내에 적이 없을 경우
        if (hitColliders.Length == 0)
        {
            maxDistanceTravelled = 0;
            _target = null;
            return;
        }

        // 각 콜라이더의 오브젝트 이름을 출력
        foreach (Collider collider in hitColliders)
        {
            if (collider.GetComponent<Enemy>().GetDistanceTravelled() > maxDistanceTravelled)
            {
                _target = collider.transform;
                maxDistanceTravelled = collider.GetComponent<Enemy>().GetDistanceTravelled();
            }
        }
    }
    protected void RotateTowardsTarget()
    {
        if (_target == null) return;

        // 타겟을 바라보는 방향을 계산
        Vector3 direction = (_target.transform.position - transform.position).normalized;

        // 타겟을 향한 회전값을 계산 (y축 회전만 적용)
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    public void ClearSynergy()
    {
        synergyDebuffs.Clear();

        _synergyAttackInterval = 0;
        _synergyDamagePlus = 1;
    }
    void OnDrawGizmos()
    {
        // 편집기에서 시각적으로 범위를 확인하기 위해 그리기
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _findRange);
    }
}
