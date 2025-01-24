using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;

public class GameStartState : ComponentState
{
    private void OnEnable()
    {
        ChangeState(Next());
    }
}
