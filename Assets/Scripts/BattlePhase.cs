using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePhase : Phase
{
    [SerializeField] EnemySpawner enemySpawner;

    // 배틀 페이즈 시작 : 스테이지 세팅, 적 나오기 시작, 건설 불가능
    public override void Enter()
    {
        enemySpawner.Init();
    }
    
    public override void Exit()
    {
    }
}
