using UnityEngine;
using TMPro;

public class Phase : MonoBehaviour
{
    [SerializeField] protected PhaseManager phaseManager;
    [SerializeField] protected Phase nextPhase;
    [SerializeField] protected TextMeshProUGUI phaseText;
    public virtual void Enter() { }

    public virtual void Exit() { }

    public void ChangePhase()
    {
        phaseManager.ChangePhase(nextPhase);
    }
}
