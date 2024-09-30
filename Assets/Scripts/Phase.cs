using UnityEngine;

public class Phase : MonoBehaviour
{
    [SerializeField] protected PhaseManager phaseManager;
    [SerializeField] protected Phase nextPhase;
    public virtual void Enter() { }

    public virtual void Exit() { }

    public void ChangePhase()
    {
        phaseManager.ChangePhase(nextPhase);
    }
}
