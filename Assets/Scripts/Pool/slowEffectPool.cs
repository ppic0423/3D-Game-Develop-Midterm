using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffectPool : PoolBase
{
    #region Singleton
    private static SlowEffectPool _instance;
    public static SlowEffectPool Instance
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
