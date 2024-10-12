using UnityEngine;

[System.Serializable]
public class TurretUpgrade
{
    public int level;
    public int damage;          // 공격력 증가량
    public float rangeIncrease;         // 탐색 범위 증가량
    public float attackInterval;        // 공격 속도 증가량
    public float slowAmount;
    public float slowDuration;
    public float damageIncrease;
    public float restTime;
    public float range;                 // 폭발 타워, 지뢰
    public int cost;                    // 업그레이드 비용
}