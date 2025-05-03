using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentEnemyState { get; set; }

    public void Init(EnemyState startingState)
    {
        currentEnemyState = startingState;
        currentEnemyState.EnterState();
    }

    public void ChangeState(EnemyState state)
    {
        currentEnemyState.ExitState();
        currentEnemyState = state;
        currentEnemyState.EnterState();
    }
}
