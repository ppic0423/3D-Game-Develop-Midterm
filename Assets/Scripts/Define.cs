using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum Layer
    {
        Default = 0,
        Cell = 3,
        Enemy = 7,
    }

    public enum Synergy
    {
        Fire,
        Rapid,
        Slayer,
        Fortune,
        Delay
    }
}
