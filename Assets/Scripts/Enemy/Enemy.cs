using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 스포너
    EnemySpawner _spawner;

    // 디버프
    public List<Debuff> Debuffs
    {
        get { return _debuffs; }
        set { _debuffs = value; }
    }
    List<Debuff> _debuffs;

    [Header("몬스터 스탯")]
    [SerializeField] private float _maxHp;
    public float MaxHp
    {
        get { return _maxHp; }
        set { _maxHp = value; }
    }
    [SerializeField] private float _hp;
    public float Hp
    {
        get { return _hp; }
        set { _hp = value; }
    }
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }
    [SerializeField] private int _damage;
    [SerializeField] private int _dropGold;
    float _damageInrease = 1;
    public float DamageIncrease
    {
        get { return _damageInrease; }
        set { _damageInrease = value; }
    }

    [Header("이동 관련")]
    [SerializeField] float rotationSpeed = 5;
    int waypointIndex = 0;
    float distanceTravelled = 0;
    WayPoint _wayPoint;

    public void Init(EnemySpawner spawner, EnemyStats enemyStats, float enemyStatsWeight, WayPoint waypoint)
    {
        _spawner = spawner; // 부모 스포너
        _wayPoint = waypoint; // 웨이 포인트

        // 몬스터 스탯
        _maxHp = enemyStats.hp * enemyStatsWeight;
        _hp = _maxHp;
        _moveSpeed = enemyStats.moveSpeed * enemyStatsWeight;
        _damage = enemyStats.damage * (int)enemyStatsWeight;
        _dropGold = enemyStats.dropGold * (int)enemyStatsWeight;

        Debuffs = new List<Debuff>();
    }

    private void FixedUpdate()
    {
        MoveAlongPath();
    }

    public void ApplyDebuff(Debuff debuff)
    {
        // 이미 같은 종류의 디버프가 있는지 확인
        Debuff existingDebuff = Debuffs.Find(d => d.GetType() == debuff.GetType());

        // 만약 같은 디버프가 없다면 새로 추가
        if (existingDebuff == null)
        {
            StartCoroutine(debuff.StartDebuff());
        }
        else
        {
            // 이미 같은 디버프가 있을 경우, 남은 지속 시간 갱신
            existingDebuff.duration = Mathf.Max(existingDebuff.duration, debuff.duration);
        }
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage * _damageInrease;

        if( _hp <= 0 ) 
        {
            Dead();
        }
    }
    void Dead()
    {
        // 골드 드롭
        ResourceManager.Instance.AddGold(_dropGold);
        // 몬스터 풀에 다시 넣음
        _spawner.OnReleaseEnemy(this.gameObject);
    }
    #region 이동 관련
    // 길을 따라 이동
    void MoveAlongPath()
    {
        if(waypointIndex < _wayPoint.points.Length)
        {
            Vector3 targetWayPoint = _wayPoint.points[waypointIndex];
            Vector3 direction = targetWayPoint - transform.position;

            if(direction.magnitude < 0.1f)
            {
                waypointIndex++;
                ArriveNexus();   
            }
            else
            {
                transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
                RotateTowards(direction);
                distanceTravelled += _moveSpeed * Time.deltaTime;
            }
        }
    }
    void ArriveNexus()
    {
        // 넥서스에 도착했을 경우
        if (waypointIndex >= _wayPoint.points.Length)
        {
            ResourceManager.Instance.TakeDamage(_damage); // 체력 감소
            _spawner.OnReleaseEnemy(this.gameObject); // 오브젝트 풀에 다시 넣기
        }
    }

    // 이동 방향으로 회전
    void RotateTowards(Vector3 direction)
    {
        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // 현재까지 이동한 거리 반환
    public float GetDistanceTravelled()
    {
        return distanceTravelled;
    }
    #endregion
}