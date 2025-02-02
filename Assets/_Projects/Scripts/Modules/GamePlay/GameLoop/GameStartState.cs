using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;

public class GameStartState : ComponentState
{
    private void OnEnable()
    {
        StartCoroutine(DelayChangeState(0.2f));
    }

    IEnumerator DelayChangeState(float delayTime)
    {
        yield return Yielders.Get(delayTime);
        ChangeState(Next());
    }
}
