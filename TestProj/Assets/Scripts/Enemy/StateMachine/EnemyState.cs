using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyController enemyController;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(EnemyController enemyController, EnemyStateMachine enemyStateMachine)
    {
        this.enemyController = enemyController;
        this.enemyStateMachine = enemyStateMachine;
    }

    public virtual void EnterState() {}
    public virtual void ExitState() {}
    public virtual void Update() {}
    public virtual void AnimationTriggerEvent(EnemyController.AnimationTriggerType triggerType) {}
}
