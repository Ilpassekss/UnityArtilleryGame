using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyController enemyController, EnemyStateMachine enemyStateMachine) : base(enemyController, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        enemyController.enemyAttackBaseInstance.Enter();
    }

    public override void ExitState()
    {
        base.ExitState();

        enemyController.enemyAttackBaseInstance.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemyController.enemyAttackBaseInstance.Update();
    }

    public override void AnimationTriggerEvent(EnemyController.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

        enemyController.enemyAttackBaseInstance.AnimationTriggerEvent(triggerType);
    }
}
