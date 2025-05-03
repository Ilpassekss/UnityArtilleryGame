using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyController enemyController, EnemyStateMachine enemyStateMachine) : base(enemyController, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        enemyController.enemyChaseBaseInstance.Enter();
    }

    public override void ExitState()
    {
        base.ExitState();

        enemyController.enemyChaseBaseInstance.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemyController.enemyChaseBaseInstance.Update();
    }

    public override void AnimationTriggerEvent(EnemyController.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

        enemyController.enemyChaseBaseInstance.AnimationTriggerEvent(triggerType);
    }
}
