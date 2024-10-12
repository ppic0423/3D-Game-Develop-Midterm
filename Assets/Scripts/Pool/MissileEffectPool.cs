using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileEffectPool : PoolBase
{
    #region Singleton
    private static MissileEffectPool _instance;
    public static MissileEffectPool Instance
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
