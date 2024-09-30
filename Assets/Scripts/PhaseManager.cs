using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    #region Singleton
    private static PhaseManager _instance;
    public static PhaseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("@PhaseManager");
                _instance = go.AddComponent<PhaseManager>();
            }
            DontDestroyOnLoad(_instance);
            return _instance;
        }
    }
    #endregion
    [SerializeField] Phase currentPhase;

    BattlePhase battlePhase;
    SetupPhase setupPhase;

    private void Awake()
    {
        battlePhase = GetComponentInChildren<BattlePhase>();
        setupPhase = GetComponentInChildren<SetupPhase>();
    }
    private void Start()
    {
        // temp
        ChangePhase(battlePhase);
    }

    public void NextStage()
    {
        ChangePhase(setupPhase);
    }
    public void ChangePhase(Phase phase)
    {
        if (currentPhase != null)
        {
            currentPhase.Exit();
        }

        currentPhase = phase;

        if (currentPhase != null)
        {
            currentPhase.Enter();
        }
    }
}
