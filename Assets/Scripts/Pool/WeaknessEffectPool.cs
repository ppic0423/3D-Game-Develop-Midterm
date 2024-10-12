using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessEffectPool : PoolBase
{
    #region Singleton
    private static WeaknessEffectPool _instance;
    public static WeaknessEffectPool Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        _instance = this;
    }
}
