using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowEffectPool : PoolBase
{
    #region Singleton
    private static slowEffectPool _instance;
    public static slowEffectPool Instance
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
