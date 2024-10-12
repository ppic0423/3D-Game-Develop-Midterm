using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyManager : MonoBehaviour
{
    #region Singleton
    private static SynergyManager _instance;
    public static SynergyManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    [SerializeField] List<Synergy> synergies = new List<Synergy>();
    private void Awake()
    {
        _instance = this;
    }

    public void CheckAndApplySynergy(Turret turret, List<Turret> nearTurret)
    {
        foreach(Synergy synergy in synergies) 
        { 
            synergy.ActivateSynergy(turret, nearTurret);
        }
    }
}