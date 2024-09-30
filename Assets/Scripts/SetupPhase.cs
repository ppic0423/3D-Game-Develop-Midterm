using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPhase : Phase
{
    [SerializeField] GameObject mouseInput;

    // 준비 페이즈 시작 : 건설 가능
    public override void Enter()
    {
        mouseInput.SetActive(true);
    }

    public override void Exit()
    {
        mouseInput.SetActive(false);
    }
}
