using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisiablePool : PoolBase
{
    #region Singleton
    private static InvisiablePool _instance;
    public static InvisiablePool Instance
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
