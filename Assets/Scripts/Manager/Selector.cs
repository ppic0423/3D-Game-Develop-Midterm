using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    [HideInInspector]public MouseInput mouseInput;
    [HideInInspector] public GameObject target;

    public virtual void Enter() { }

    public virtual void Tick() { }

    public virtual void Exit() { }
}
